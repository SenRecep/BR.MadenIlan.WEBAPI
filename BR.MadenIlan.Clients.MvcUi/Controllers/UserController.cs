using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Core.StringInfo;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.Clients.MvcUi.Controllers
{
    [Authorize]
    public class UserController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
