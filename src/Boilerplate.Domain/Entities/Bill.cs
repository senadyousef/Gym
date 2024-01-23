using Boilerplate.Domain.Core.Entities; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Boilerplate.Domain.Entities
{
    public class Bill : Entity
    {
        #region Parameters
        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [Required]
        public float Amount { get; set; } 
        public string Status { get; set; }
        #endregion

        #region Relations
        #endregion
    }
} 