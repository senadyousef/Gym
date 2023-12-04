using System.ComponentModel.DataAnnotations;
using Waffer.Domain.Abstractions;

namespace Waffer.Domain.Entities.Personals
{
    public class Offer : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name_en { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name_ar { get; set; }
    }
}
