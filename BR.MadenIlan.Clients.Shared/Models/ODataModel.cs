using System.Collections.Generic;

namespace BR.MadenIlan.Clients.Shared.Models
{
    public class ODataModel<T>
    {
        public List<T> Value { get; set; }
    }
}
