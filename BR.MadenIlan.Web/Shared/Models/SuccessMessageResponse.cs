namespace BR.MadenIlan.Web.Shared.Models
{
    public class SuccessMessageResponse{
        public SuccessMessageResponse(string message)
        {
            this.Message = message;
        }
        public string Message { get; set; }
    }
}
