﻿using System;
using System.Collections.Generic;

namespace BR.MadenIlan.Clients.Shared.Models
{
    public class ErrorDto
    {
        public ErrorDto() => Errors = new List<string>();

        public ErrorDto(bool isShow, int statusCode, string path, params string[] massages)
        {
            IsShow = isShow;
            StatusCode = statusCode;
            Path = path;
            Errors = new List<string>(massages);
        }
        public List<string> Errors { get; set; }
        public int StatusCode { get; set; }
        public bool IsShow { get; set; }
        public string Path { get; set; }
    }
}
