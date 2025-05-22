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
    public class Events : Entity
    {
        #region Parameters   
        [Required] 
        public int UserId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; } 
        public string PhotoUri { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Capacity { get; set; }
        public int Booked { get; set; }
        public string Type { get; set; }     
        public User User  { get; set; }
        #endregion

        #region Relations 
        #endregion
    }
}
