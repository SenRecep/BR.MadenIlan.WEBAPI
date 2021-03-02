

namespace BR.MadenIlan.Web.Shared.Models
{
    public class ApiResponse<T> where T : class
    {
        public ApiResponse(bool isSuccessful, T success = null, ErrorDto fail=null)
        {
            IsSuccessful = isSuccessful;
            Success = success;
            Fail = fail;
        }
        public bool IsSuccessful { get; set; }
        public T Success { get; set; }
        public ErrorDto Fail { get; set; }
    }
}
