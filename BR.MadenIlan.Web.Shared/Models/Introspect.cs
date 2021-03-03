using System.Text.Json.Serialization;

namespace BR.MadenIlan.Web.Shared.Models
{
    public class Introspect
    {
        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}
