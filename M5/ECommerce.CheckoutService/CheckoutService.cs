using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using ECommerce.CheckoutService.Model;

namespace ECommerce.CheckoutService
{
   /// <summary>
   /// An instance of this class is created for each service replica by the Service Fabric runtime.
   /// </summary>
   internal sealed class CheckoutService : StatefulService, ICheckoutService
   {
      public CheckoutService(StatefulServiceContext context)
          : base(context)
      { }

      public Task<CheckoutSummary> Checkout(string userId)
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<CheckoutSummary>> GetOrderHitory(string userId)
      {
         throw new NotImplementedException();
      }

      protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
      {
         return new[]
         {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
         };
      }
   }
}
