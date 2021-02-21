﻿using System.Text.Json.Serialization;

namespace BR.MadenIlan.Web.Shared.Models
{
    public class Token
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
