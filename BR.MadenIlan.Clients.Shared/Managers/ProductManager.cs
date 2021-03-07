using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.DTOs.Product;
using BR.MadenIlan.Clients.Shared.Models;
using BR.MadenIlan.Clients.Shared.Services;

namespace BR.MadenIlan.Clients.Shared.Managers
{
    public class ProductManager : IProductService
    {
        private readonly IApiResourceHttpClient apiResourceHttpClient;

        public ProductManager(IApiResourceHttpClient apiResourceHttpClient)
        {
            this.apiResourceHttpClient = apiResourceHttpClient;
        }

        public async Task<List<ProductDTO>> GetProducts()
        {
            var httpClient = apiResourceHttpClient.GetHttpClient();
            var response = await httpClient.GetFromJsonAsync<ODataModel<ProductDTO>>("odata/products");
            return response.Value;
        }
    }
}
