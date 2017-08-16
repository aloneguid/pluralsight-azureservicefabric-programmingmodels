using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ECommerce.ProductCatalog.Model
{
   public interface IProductCatalogService : IService
   {
      Task<IEnumerable<Product>> GetAllProducts();

      Task AddProduct(Product product);

      Task<Product> GetProduct(Guid productId);
   }
}
