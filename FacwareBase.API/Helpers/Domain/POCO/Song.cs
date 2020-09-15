namespace FacwareBase.API.Helpers.Domain.POCO
{
    /// <summary>
    /// Demo model
    /// </summary>
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Album Album { get; set; }
    }
}