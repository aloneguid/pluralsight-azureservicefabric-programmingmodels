using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Model;
using ECommerce.CheckoutService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace ECommerce.API.Controllers
{
   [Route("api/[controller]")]
   public class CheckoutController : Controller
   {
      private static readonly Random rnd = new Random(DateTime.UtcNow.Second);

      [Route("{userId}")]
      public async Task<ApiCheckoutSummary> Checkout(string userId)
      {
         CheckoutSummary summary = await GetCheckoutService().Checkout(userId);

         return ToApiCheckoutSummary(summary);
      }

      [Route("history/{userId}")]
      public async Task<IEnumerable<ApiCheckoutSummary>> GetHistory(string userId)
      {
         IEnumerable<CheckoutSummary> history = await GetCheckoutService().GetOrderHitory(userId);

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

         return ServiceProxy.Create<ICheckoutService>(
                new Uri("fabric:/ECommerce/CheckoutService"),
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
