using ECommerce.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserActor.Interfaces;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        [HttpGet("{userId}")]
        public async Task<ApiBasket> Get(string userId)
        {
            IUserActor actor = GetActor(userId);

            Dictionary<Guid, int> products = await actor.GetBasket();

            return new ApiBasket()
            {
                UserId = userId,
                Items = products.Select(
                    p => new ApiBasketItem {  ProductId = p.Key.ToString(), Quantity = p.Value }).ToArray()
            };
        }

        [HttpPost("{userId}")]
        public async Task Add(string userId, [FromBody] ApiBasketAddRequest request)
        {
            IUserActor actor = GetActor(userId);

            await actor.AddToBasket(request.ProductId, request.Quantity);
        }

        [HttpDelete("{userId}")]
        public async Task Delete(string userId)
        {
            IUserActor actor = GetActor(userId);

            await actor.ClearBasket();
        }

        private IUserActor GetActor(string userId)
        {
            return ActorProxy.Create<IUserActor>(new ActorId(userId), new Uri("fabric:/ECommerce/UserActorService"));
        }
    }
}
