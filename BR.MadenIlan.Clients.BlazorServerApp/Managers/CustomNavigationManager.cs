using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.Models;
using BR.MadenIlan.Clients.Shared.Services;

using Microsoft.AspNetCore.Components;

namespace BR.MadenIlan.Clients.BlazorServerApp.Managers
{
    public class CustomNavigationManager : INavigationService
    {
        private readonly NavigationManager navigationManager;

        public CustomNavigationManager(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
        }
        public void NavigateLogin<T>(ApiResponse<T> apiResponse) where T : class
        {
            if (!apiResponse.IsSuccessful && apiResponse.Fail != null && apiResponse.Fail.StatusCode==(int)HttpStatusCode.Unauthorized && apiResponse.Fail?.Errors.Count > 0)
            {
                var url = apiResponse.Fail.Errors.FirstOrDefault();
                navigationManager.NavigateTo(url);
            }
        }

        public void NavigateTo(string url) => navigationManager.NavigateTo(uri: url);
    }
}
