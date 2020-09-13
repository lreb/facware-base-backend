namespace FacwareBase.API.Helpers.Domain.POCO
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Album Album { get; set; }
    }
}