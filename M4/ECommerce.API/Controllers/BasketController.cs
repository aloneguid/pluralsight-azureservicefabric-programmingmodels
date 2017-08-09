using ECommerce.API.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        [HttpGet("{userId}")]
        public async Task<ApiBasket> Get(string userId)
        {
            return new ApiBasket() {UserId = userId.ToString()};
        }

        [HttpPost("{userId}")]
        public async Task Add([FromBody] ApiBasketAddRequest request)
        {
            int i = 0;
        }

        [HttpDelete("{userId}")]
        public async Task Delete(int userId)
        {
            int i = 0;
        }
    }
}
