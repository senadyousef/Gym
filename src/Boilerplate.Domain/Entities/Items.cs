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
    public class Items : Entity
    {
        #region Parameters    
        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [Required]
        public string NameEn { get; set; }
        [Required]
        public string NameAr { get; set; }
        public float Price { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; } 
        #endregion

        #region Relations 
        #endregion
    }
}
