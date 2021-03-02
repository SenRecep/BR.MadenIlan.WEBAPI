namespace BR.MadenIlan.Web.Shared.Models
{
    public class SignUpDto
    {
        public SignUpDto(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
