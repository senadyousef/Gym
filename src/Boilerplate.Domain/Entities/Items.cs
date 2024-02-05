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
        [MaxLength(50)]
        public string NameEn { get; set; } 
        [MaxLength(50)]
        public string NameAr { get; set; } 
        public decimal Price { get; set; } 
        public string Description { get; set; }  
        #endregion

        #region Relations 
        public List<ItemPhotos> ItemPhotos { get; set; } 
        public List<UserItems> UserItems { get; set; } 
        public List<Carts> Carts { get; set; } 
        #endregion
    }
}
