using System.Collections.Generic;

namespace FacwareBase.API.Helpers.Domain.POCO
{
  public class Album
  {
      public Album()
      {
          Songs = new List<Song>();
      }
      public int Id { get; set; }
      public string Name { get; set; }
      public ICollection<Song> Songs { get; set; }
  }
}
