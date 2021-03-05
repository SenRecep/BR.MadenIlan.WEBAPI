namespace BR.MadenIlan.Clients.Shared.Models
{
    public class ApiClient
    {
        public bool IsLocal { get; set; }

        public string GetAuthBaseUrl => IsLocal ? LocalAuthBaseUrl : AuthBaseUrl;

        public string AuthBaseUrl { get; set; }
        public string ApiBaseUrl { get; set; }
        public string PhotoBaseUrl { get; set; }

        public string LocalAuthBaseUrl { get; set; }
        public string LocalApiBaseUrl { get; set; }
        public string LocalPhotoBaseUrl { get; set; }

        public string ClientCredentialGrantType { get; set; }
        public string ResourceOwnerPasswordCredentialGrantType { get; set; }
        public string RefreshTokenCredentialGrantType { get; set; }

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
