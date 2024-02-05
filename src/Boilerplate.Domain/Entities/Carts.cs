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
    public class Carts : Entity
    {
        #region Parameters 
        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Items))]
        public int ItemsId { get; set; }
        
        [Required]
        [ForeignKey(nameof(Bill))]
        public int BillId { get; set; }

        public int Quantity { get; set; }

        public Items Item { get; set; }
        #endregion

        #region Relations   
        #endregion 
    }
}
