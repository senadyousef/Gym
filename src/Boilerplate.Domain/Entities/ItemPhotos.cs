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
    public class ItemPhotos : Entity
    {
        #region Parameters
        [Required]
        [ForeignKey(nameof(Items))]
        public int ItemsId { get; set; }

        [Required] 
        public string PhotoUri { get; set; }
        #endregion

        #region Relations
        #endregion
    }
}
