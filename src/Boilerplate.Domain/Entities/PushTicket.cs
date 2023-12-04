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
    public class PushTicket : Entity
    {
        [Required]
        [MaxLength(450)]
        public string ReceiptId { get; set; }
        public string Title { get; set; }
        public string MessageBody { get; set; }
        public string Url { get; set; }


        //[ForeignKey(nameof(User))]
        [MaxLength(450)]
        [Required]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
