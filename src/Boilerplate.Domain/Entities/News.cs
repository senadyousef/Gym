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
    public class News : Entity
    {
        #region Parameters   
        [Required]
        public string DescriptionEn { get; set; } 
        public string DescriptionAr { get; set; }
        public DateTime NewsDate { get; set; }
        public string Highlight { get; set; } 
        public string PhotoUri { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        #endregion

        #region Relations
        #endregion
    }
}
