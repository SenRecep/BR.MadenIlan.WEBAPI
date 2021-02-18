using System.Collections.Generic;

namespace BR.MadenIlan.Shared.Models
{
    public class ErrorDto
    {
        public ErrorDto()
        {
            Errors = new List<string>();
        }
        public List<string> Errors { get; set; }
        public int StatusCode { get; set; }
        public bool IsShow { get; set; }
        public string Path { get; set; }
    }
}
