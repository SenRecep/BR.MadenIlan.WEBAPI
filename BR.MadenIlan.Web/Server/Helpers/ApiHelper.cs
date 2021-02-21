using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Web.Shared.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.Web.Server.Helpers
{
    public static class ApiHelper
    {
        public static ErrorDto ErrorHandler(bool isShow=false, string path= "/api/Auth/CheckToken", int statusCode= StatusCodes.Status500InternalServerError,string massage= "Sunucu bazlı bir hata gerçekleşti lütfen daha sonra tekar deneyiniz.")
        {
            return new ErrorDto()
            {
                Errors = new() { massage },
                IsShow = isShow,
                Path = path,
                StatusCode = statusCode
            };
        }
    }
}
