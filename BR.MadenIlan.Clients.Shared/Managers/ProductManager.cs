using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.DTOs.Product;
using BR.MadenIlan.Clients.Shared.ExtensionMethods;
using BR.MadenIlan.Clients.Shared.Helpers;
using BR.MadenIlan.Clients.Shared.Models;
using BR.MadenIlan.Clients.Shared.Services;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace BR.MadenIlan.Clients.Shared.Managers
{
    public class ProductManager : IProductService
    {
        private readonly IApiResourceHttpClient apiResourceHttpClient;
        private readonly ILogger<ProductManager> logger;

        public ProductManager(IApiResourceHttpClient apiResourceHttpClient, ILogger<ProductManager> logger)
        {
            this.apiResourceHttpClient = apiResourceHttpClient;
            this.logger = logger;
        }

        public async Task<ApiResponse<List<ProductDTO>>> GetProductsAsync()
        {
            var httpClient = apiResourceHttpClient.GetHttpClient();
            var response = await httpClient.GetAsync("odata/products");

            var result = await ApiHelper.Result(response, () =>
            {
                var resultModel =  response.Content.ReadFromJsonAsync<ODataModel<ProductDTO>>().Result;
                return resultModel.Value;
            }, "ProductManager/GetProductsAsync");
            logger.LogApiResponse(result);
            return result;
        }
    }
}
