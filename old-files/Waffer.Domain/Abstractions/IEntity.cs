using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waffer.Domain.Abstractions
{
    public interface IEntity
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public bool? Disabled { get; set; }

        public DateTimeOffset? DisabledOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }
    }
}
