using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.CheckoutService.Model;
using ECommerce.ProductCatalog.Model;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using UserActor.Interfaces;

namespace ECommerce.CheckoutService
{
   internal sealed class CheckoutService : StatefulService, ICheckoutService
   {
      public CheckoutService(StatefulServiceContext context)
          : base(context)
      { }

      public async Task<CheckoutSummary> CheckoutAsync(string userId)
      {
         var result = new CheckoutSummary();
         result.Date = DateTime.UtcNow;
         result.Products = new List<CheckoutProduct>();

         //call user actor to get the basket
         IUserActor userActor = GetUserActor(userId);
         BasketItem[] basket = await userActor.GetBasket();

         //get catalog client
         IProductCatalogService catalogService = GetProductCatalogService();

         //constuct CheckoutProduct items by calling to the catalog
         foreach(BasketItem basketLine in basket)
         {
            Product product = await catalogService.GetProductAsync(basketLine.ProductId);
            var checkoutProduct = new CheckoutProduct
            {
               Product = product,
               Price = product.Price,
               Quantity = basketLine.Quantity
            };
            result.Products.Add(checkoutProduct);
         }

         //generate total price
         result.TotalPrice = result.Products.Sum(p => p.Price);

         //clear user basket
         await userActor.ClearBasket();

         await AddToHistoryAsync(result);

         return result;
      }

      public async Task<CheckoutSummary[]> GetOrderHitoryAsync(string userId)
      {
         var result = new List<CheckoutSummary>();
         IReliableDictionary<DateTime, CheckoutSummary> history = 
            await StateManager.GetOrAddAsync<IReliableDictionary<DateTime, CheckoutSummary>>("history");

         using(ITransaction tx = StateManager.CreateTransaction())
         {
            IAsyncEnumerable<KeyValuePair<DateTime, CheckoutSummary>> allProducts = 
               await history.CreateEnumerableAsync(tx, EnumerationMode.Unordered);
            using(IAsyncEnumerator<KeyValuePair<DateTime, CheckoutSummary>> enumerator = allProducts.GetAsyncEnumerator())
            {
               while(await enumerator.MoveNextAsync(CancellationToken.None))
               {
                  KeyValuePair<DateTime, CheckoutSummary> current = enumerator.Current;

                  result.Add(current.Value);
               }
            }
         }

         return result.ToArray();
      }

      private IUserActor GetUserActor(string userId)
      {
         return ActorProxy.Create<IUserActor>(
            new ActorId(userId),
            new Uri("fabric:/ECommerce/UserActorService"));
      }

      private IProductCatalogService GetProductCatalogService()
      {
         var proxyFactory = new ServiceProxyFactory(
            c => new FabricTransportServiceRemotingClientFactory());

         return proxyFactory.CreateServiceProxy<IProductCatalogService>(
            new Uri("fabric:/ECommerce/ECommerce.ProductCatalog"),
            new ServicePartitionKey(0));
      }

      private async Task AddToHistoryAsync(CheckoutSummary checkout)
      {
         IReliableDictionary<DateTime, CheckoutSummary> history = 
            await StateManager.GetOrAddAsync<IReliableDictionary<DateTime, CheckoutSummary>>("history");

         using(ITransaction tx = StateManager.CreateTransaction())
         {
            await history.AddAsync(tx, checkout.Date, checkout);

            await tx.CommitAsync();
         }
      }


      protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
      {
         return new[]
         {
            new ServiceReplicaListener(context =>
               new FabricTransportServiceRemotingListener(context, this))
         };
      }
   }
}
