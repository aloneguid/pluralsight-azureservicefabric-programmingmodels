using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using UserActor.Interfaces;

namespace ECommerce.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class BasketController : ControllerBase
   {
      [HttpGet("{userId}")]
      public async Task<ApiBasket> GetAsync(string userId)
      {
         IUserActor actor = GetActor(userId);

         BasketItem[] products = await actor.GetBasket();

         return new ApiBasket()
         {
            UserId = userId,
            Items = products.Select(
                 p => new ApiBasketItem
                 {
                    ProductId = p.ProductId.ToString(),
                    Quantity = p.Quantity
                 })
                 .ToArray()
         };
      }

      [HttpPost("{userId}")]
      public async Task AddAsync(
         string userId,
         [FromBody] ApiBasketAddRequest request)
      {
         IUserActor actor = GetActor(userId);

         await actor.AddToBasket(request.ProductId, request.Quantity);
      }

      [HttpDelete("{userId}")]
      public async Task DeleteAsync(string userId)
      {
         IUserActor actor = GetActor(userId);

         await actor.ClearBasket();
      }

      private IUserActor GetActor(string userId)
      {
         return ActorProxy.Create<IUserActor>(
            new ActorId(userId),
            new Uri("fabric:/ECommerce/UserActorService"));
      }
   }
}
