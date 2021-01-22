using System.ComponentModel.DataAnnotations;

namespace Facware.Domain
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
