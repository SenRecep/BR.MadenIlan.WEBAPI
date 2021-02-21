
namespace BR.MadenIlan.Web.Shared.Models
{
    public record ApiResponse<T>(bool IsSuccessful, T Success = null, ErrorDto Fail = null) where T : class;
}
