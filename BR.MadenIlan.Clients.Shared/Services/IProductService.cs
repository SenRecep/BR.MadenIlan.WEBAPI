using System.Collections.Generic;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.DTOs.Product;

namespace BR.MadenIlan.Clients.Shared.Services
{
    public interface IProductService
    {
         Task<List<ProductDTO>> GetProducts();
    }
}
