using BR.MadenIlan.Clients.Shared.Models;

namespace BR.MadenIlan.Clients.Shared.Services
{
    public interface INavigationService
    {
        void NavigateTo(string  url);
        void NavigateLogin<T>(ApiResponse<T> apiResponse) where T:class;
    }
}
