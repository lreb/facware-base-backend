using System;

namespace Facware.Base.Models.Definitions
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool Enabled { get; set; }
        
    }
}
