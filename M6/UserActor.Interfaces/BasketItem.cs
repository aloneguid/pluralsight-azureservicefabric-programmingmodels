using System;
using System.Collections.Generic;
using System.Text;

namespace UserActor.Interfaces
{
   public class BasketItem
   {
      public Guid ProductId { get; set; }

      public int Quantity { get; set; }
   }
}
