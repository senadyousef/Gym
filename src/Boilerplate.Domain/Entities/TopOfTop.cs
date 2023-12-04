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
    public class TopOfTop : Entity
    {
        #region Parameters  
        [Required] 
        public int ItemId { get; set; }

        [Required]
        public string ItemType { get; set; }

        [Required]
        public string NameEn { get; set; }

        public string NameAr { get; set; }

        [Required]
        public string DescriptionEn { get; set; }

        public string DescriptionAr { get; set; }

        public string Highlight { get; set; }
         
        public string PhotoUri { get; set; }
        #endregion

        #region Relations
        #endregion
    }
}
