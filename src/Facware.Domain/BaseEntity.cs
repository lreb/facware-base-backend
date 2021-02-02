using System;
using System.ComponentModel.DataAnnotations;

namespace Facware.Domain
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }

    public class BaseEntityAudit : BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
    }
}
