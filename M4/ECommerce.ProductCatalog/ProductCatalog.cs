using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.ProductCatalog.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ECommerce.ProductCatalog
{
   /// <summary>
   /// An instance of this class is created for each service replica by the Service Fabric runtime.
   /// </summary>
   internal sealed class ProductCatalog : StatefulService
   {
      private IProductRepository _repo;

      public ProductCatalog(StatefulServiceContext context)
          : base(context)
      { }

      /// <summary>
      /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
      /// </summary>
      /// <remarks>
      /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
      /// </remarks>
      /// <returns>A collection of listeners.</returns>
      protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
      {
         return new ServiceReplicaListener[0];
      }

      /// <summary>
      /// This is the main entry point for your service replica.
      /// This method executes when this replica of your service becomes primary and has write status.
      /// </summary>
      /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
      protected override async Task RunAsync(CancellationToken cancellationToken)
      {
         _repo = new ServiceFabricProductRepository(this.StateManager);

         var product1 = new Product
         {
            Id = Guid.NewGuid(),
            Name = "Dell Monitor",
            Description = "Computer Monitor",
            Price = 500,
            Availability = 100
         };

         var product2 = new Product
         {
            Id = Guid.NewGuid(),
            Name = "Surface Book",
            Description = "Microsoft's Latest Laptop, i7 CPU, 1Tb SSD",
            Price = 2200,
            Availability = 15
         };

         var product3 = new Product
         {
            Id = Guid.NewGuid(),
            Name = "Arc Touch Mouse",
            Description = "Computer Mouse, bluetooth, requires 2 AAA batteries",
            Price = 60,
            Availability = 30
         };

         await _repo.AddProduct(product1);
         await _repo.AddProduct(product2);
         await _repo.AddProduct(product3);

         IEnumerable<Product> all = await _repo.GetAllProducts();

      }
   }
}