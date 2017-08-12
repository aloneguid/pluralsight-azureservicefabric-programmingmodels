using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace UserActor.Interfaces
{
    public interface IUserActor : IActor
    {
        Task AddToBasket(Guid productId, int quantity);

        Task<Dictionary<Guid, int>> GetBasket();

        Task ClearBasket();
    }
}
