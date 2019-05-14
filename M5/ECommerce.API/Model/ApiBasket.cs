using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ECommerce.API.Model
{
   public class ApiBasket
   {
      [JsonProperty("userId")]
      public string UserId { get; set; }

      [JsonProperty("item")]
      public ApiBasketItem[] Items { get; set; }
   }

   public class ApiBasketItem
   {
      [JsonProperty("productId")]
      public string ProductId { get; set; }

      [JsonProperty("quantity")]
      public int Quantity { get; set; }
   }
}
