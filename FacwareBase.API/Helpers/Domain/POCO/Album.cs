using System.Collections.Generic;

namespace FacwareBase.API.Helpers.Domain.POCO
{
  /// <summary>
  /// Demo model
  /// </summary>
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
