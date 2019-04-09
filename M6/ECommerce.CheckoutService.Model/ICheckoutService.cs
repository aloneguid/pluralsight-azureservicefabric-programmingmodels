using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ECommerce.CheckoutService.Model
{
   public interface ICheckoutService : IService
   {
      Task<CheckoutSummary> Checkout(string userId);

      Task<IEnumerable<CheckoutSummary>> GetOrderHitory(string userId);
   }
}