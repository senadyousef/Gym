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
    public class Branches : Entity
    {
        #region Parameters 
        [Required]
        [MaxLength(50)]
        public string NameEn { get; set; } 
        [Required]
        [MaxLength(50)]
        public string NameAr { get; set; }  
        #endregion

        #region Relations  
        public List<Events> Events { get; set; }
        #endregion
    }
}
