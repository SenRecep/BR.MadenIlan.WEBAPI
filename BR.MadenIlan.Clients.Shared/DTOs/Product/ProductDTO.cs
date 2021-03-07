using BR.MadenIlan.Clients.Shared.DTOs.Interfaces;

namespace BR.MadenIlan.Clients.Shared.DTOs.Product
{
    public class ProductDTO:IDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string PhotoPath { get; set; }
    }
}
