using Microsoft.AspNetCore.Identity;
using System;
using Waffer.Domain.Abstractions;

namespace Waffer.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<int>, IEntity
    {
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public bool? Disabled { get; set; }

        public DateTimeOffset? DisabledOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
    }



}
