using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Model;
using ECommerce.CheckoutService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;

namespace ECommerce.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class CheckoutController : ControllerBase
   {
      private static readonly Random rnd = new Random(DateTime.UtcNow.Second);

      [Route("{userId}")]
      public async Task<ApiCheckoutSummary> CheckoutAsync(string userId)
      {
         CheckoutSummary summary = 
            await GetCheckoutService().CheckoutAsync(userId);

         return ToApiCheckoutSummary(summary);
      }

      [Route("history/{userId}")]
      public async Task<IEnumerable<ApiCheckoutSummary>> GetHistoryAsync(
         string userId)
      {
         IEnumerable<CheckoutSummary> history = 
            await GetCheckoutService().GetOrderHitoryAsync(userId);

         return history.Select(ToApiCheckoutSummary);
      }


      private ApiCheckoutSummary ToApiCheckoutSummary(CheckoutSummary model)
      {
         return new ApiCheckoutSummary
         {
            Products = model.Products.Select(p => new ApiCheckoutProduct
            {
               ProductId = p.Product.Id,
               ProductName = p.Product.Name,
               Price = p.Price,
               Quantity = p.Quantity
            }).ToList(),
            Date = model.Date,
            TotalPrice = model.TotalPrice
         };
      }

      private ICheckoutService GetCheckoutService()
      {
         long key = LongRandom();

         var proxyFactory = new ServiceProxyFactory(
            c => new FabricTransportServiceRemotingClientFactory());

         return proxyFactory.CreateServiceProxy<ICheckoutService>(
            new Uri("fabric:/ECommerce/ECommerce.CheckoutService"),
            new ServicePartitionKey(key));
      }

      private long LongRandom()
      {
         byte[] buf = new byte[8];
         rnd.NextBytes(buf);
         long longRand = BitConverter.ToInt64(buf, 0);
         return longRand;
      }
   }
}