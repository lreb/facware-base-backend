namespace Facware.Domain.Entities
{
    /// <summary>
    /// Demo entity
    /// </summary>
    public class Demo : BaseEntityFull
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}