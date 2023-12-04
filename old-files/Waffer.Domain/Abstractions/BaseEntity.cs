using System;

namespace Waffer.Domain.Abstractions
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public bool? Disabled { get; set; }

        public DateTimeOffset? DisabledOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
    }
}
