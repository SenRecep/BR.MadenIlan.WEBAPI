using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.MadenIlan.Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string PhotoPath { get; set; }

    }
}
