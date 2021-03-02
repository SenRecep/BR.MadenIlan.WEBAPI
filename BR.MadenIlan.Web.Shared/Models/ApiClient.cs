namespace BR.MadenIlan.Web.Shared.Models
{

    //public const string AuthBaseUrl = "http://10.0.2.2:4456";
    //public const string ApiBaseUrl = "http://10.0.2.2:4466";
    //public const string PhotoBaseUrl = "http://10.0.2.2:4467";
    public class ApiClient
    {
        public string AuthBaseUrl { get; set; }
        public  string ApiBaseUrl { get; set; }
        public  string PhotoBaseUrl { get; set; }

        public  string ClientCredentialGrantType { get; set; }
        public  string ResourceOwnerPasswordCredentialGrantType { get; set; }
        public  string RefreshTokenCredentialGrantType { get; set; }

        public string BasicUserName { get; set; }
        public string BasicPassword { get; set; }

        public Client IdentityClient { get; set; }
        public Client WebClient { get; set; }

        public class Client
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
        }
    }
}
