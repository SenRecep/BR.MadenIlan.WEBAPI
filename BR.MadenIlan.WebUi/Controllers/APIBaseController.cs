using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Web.Shared.Models;

using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.WebUi.Controllers
{
    public class APIBaseController:ControllerBase
    {
        public IActionResult ApiResponseAction<T>(ApiResponse<T> val) where T:class
        {
            if (val.IsSuccessful)
                return Ok(val);
            return StatusCode(val.Fail.StatusCode,val);
        }
    }
}
