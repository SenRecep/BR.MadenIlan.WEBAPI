using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.Models;

using IdentityModel.Client;

using Newtonsoft.Json;

namespace BR.MadenIlan.Clients.Shared.Helpers
{
    public static class ApiHelper
    {
        public static async Task<ApiResponse<T>> Result<T>(ProtocolResponse res, T val = null, string path = null, bool isShow = false) where T : class
        {
            if (res.IsError)
            {
                var str = await res.HttpResponse.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorDto>(str);
                if (error?.Errors.Count == 0)
                    return new ApiResponse<T>(false, null, new ErrorDto(isShow, (int)res.HttpStatusCode, path, res.Error));
                return new ApiResponse<T>(false, null, error);
            }

            return new ApiResponse<T>(true, val);
        }
    }
}
