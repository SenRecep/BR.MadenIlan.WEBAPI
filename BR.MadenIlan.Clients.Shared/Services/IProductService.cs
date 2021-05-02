using System.Collections.Generic;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.DTOs.Product;
using BR.MadenIlan.Clients.Shared.Models;

namespace BR.MadenIlan.Clients.Shared.Services
{
    public interface IProductService
    {
        Task<ApiResponse<List<ProductDTO>>> GetProductsAsync();
    }
}
