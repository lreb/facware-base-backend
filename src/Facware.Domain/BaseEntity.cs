using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Facware.Domain
{
    /// <summary>
    /// Enable model identfier, basic for every model
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
    }

    /// <summary>
    /// Enable soft delete
    /// </summary>
    public class BaseEntitySoft: BaseEntity
    {
        /// <summary>
        /// Define if a item is active or not
        /// </summary>
        /// <value>By default all items are active</value>
        public bool Active { get; set; }
    }

    /// <summary>
    /// Enable audit fields
    /// </summary>
    public class BaseEntityFull : BaseEntitySoft
    {
        /// <summary>
        /// When the record is created
        /// </summary>
        /// <value>Current date time</value>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Who created the record
        /// </summary>
        /// <value>User id</value>
        public int CreatedBy { get; set; }
        /// <summary>
        /// When the record is saved
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Who updated the record
        /// </summary>
        /// <value>user id</value>
        public int? UpdatedBy { get; set; }
    }
}
