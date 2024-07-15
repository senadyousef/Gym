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
    public class AllServices : Entity
    {
        #region Parameters   
        [Required]
        public string NameAr { get; set; }
        [Required]
        public string NameEn { get; set; }
        #endregion

        #region Relations
        public List<UserAllServices> UserAllServices { get; set; }
        #endregion
    }
}
