using Boilerplate.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Entities
{
    public class UserEvents : Entity
    {
        #region Parameters
        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Events))]
        public int EventsId { get; set; }
        #endregion

        #region Relations
        #endregion
    }
}
