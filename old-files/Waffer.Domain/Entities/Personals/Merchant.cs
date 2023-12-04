using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Waffer.Domain.Abstractions;

namespace Waffer.Domain.Entities.Personals
{
    public class Merchant : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name_en { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name_ar { get; set; }

        // One to many relation 
        public ICollection<Offer> Offers { get; set; }

        public Merchant()
        {
            Offers = new HashSet<Offer>();

        }
    }
}
