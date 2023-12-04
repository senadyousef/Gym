using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boilerplate.Domain.Core.Entities
{
    public abstract class Entity
    {
        [Key]

        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

        public string DisabledBy { get; set; }
        public DateTime? DisabledOn { get; set; }
        public bool IsDisabled { get; set; }
    }
}
