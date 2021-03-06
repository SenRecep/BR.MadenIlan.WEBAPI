
using BR.MadenIlan.Clients.Shared.DTOs.Interfaces;

namespace BR.MadenIlan.Clients.Shared.DTOs.Auth
{
    public class SignInDTO : IDTO
    {
        public SignInDTO() { }
        
        public SignInDTO(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
