using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ECommerce.API.Model
{
   public class ApiProduct
   {
      [JsonProperty("id")]
      public Guid Id { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("description")]
      public string Description { get; set; }

      [JsonProperty("price")]
      public double Price { get; set; }

      [JsonProperty("isAvailable")]
      public bool IsAvailable { get; set; }
   }
}
