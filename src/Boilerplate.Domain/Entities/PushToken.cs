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
    public class PushToken : Entity
    {
        [Required]
        [MaxLength(450)]
        public string Token { get; set; }

        public bool Valid { get; set; }

        //[ForeignKey(nameof(User))]
        [MaxLength(450)]
        [Required]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
