
using BR.MadenIlan.Web.Shared.Models;

using IdentityModel.Client;

using Newtonsoft.Json;

namespace BR.MadenIlan.AuthBootLoader.Helpers
{
    public static class ApiHelper
    {
        public static ApiResponse<T> Result<T>(ProtocolResponse res,T val=null,string path=null,bool isShow=false) where T:class
        {
            if (res.IsError)
            {
                var str = res.HttpResponse.Content.ReadAsStringAsync().Result;
                var error = JsonConvert.DeserializeObject<ErrorDto>(str);
                if (error?.Errors.Count==0)
                    return new(false, null, new(isShow, (int)res.HttpStatusCode, path, res.Error));
                return new(false, null, error);
            }

            return new(true, val);
        }
    }
}
