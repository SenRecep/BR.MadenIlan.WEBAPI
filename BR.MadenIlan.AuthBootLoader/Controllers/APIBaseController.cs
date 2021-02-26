
using BR.MadenIlan.Web.Shared.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.AuthBootLoader.Controllers
{
    public class APIBaseController : ControllerBase
    {
        public IActionResult ApiResponseAction<T>(ApiResponse<T> val) where T : class
        {
            if (val.IsSuccessful)
                return Ok(val);
            return StatusCode(val.Fail?.StatusCode ?? StatusCodes.Status400BadRequest, val);
        }
    }
}
