namespace BR.MadenIlan.Clients.Shared.Models
{
    public record ApiResponse<T>(bool IsSuccessful,T Success = null, ErrorDto Fail = null) where T : class
    {
        public string GetErrors(string separator="<br/>")
        {
            return string.Join(separator,Fail.Errors);
        }
    }
}
