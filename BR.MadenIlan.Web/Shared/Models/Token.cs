namespace BR.MadenIlan.Web.Shared.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string Scope { get; set; }
        public string RefreshToken { get; set; }
    }
}
