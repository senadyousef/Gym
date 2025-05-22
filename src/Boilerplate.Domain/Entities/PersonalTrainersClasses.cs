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
    public class PersonalTrainersClasses : Entity
    {
        #region Parameters 
        [Required] 
        public int PersonalTrainer { get; set; }
        
        [Required] 
        public int Trainee { get; set; }
        
        [Required] 
        public DateTime Time { get; set; }
        #endregion

        #region Relations  
        #endregion
    }
}
