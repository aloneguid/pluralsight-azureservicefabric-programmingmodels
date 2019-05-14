using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class BasketController : ControllerBase
   {
      [HttpGet("{userId}")]
      public async Task<ApiBasket> GetAsync(string userId)
      {

      }

      [HttpPost("{userId}")]
      public async Task AddAsync(
         string userId,
         [FromBody] ApiBasketAddRequest request)
      {

      }

      [HttpDelete("{userId}")]
      public async Task DeleteAsync(string userId)
      {

      }
   }
}
