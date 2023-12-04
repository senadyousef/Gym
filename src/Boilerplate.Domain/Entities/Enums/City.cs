using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Entities.Enums
{
    public enum City
    {
        [Display(Name = "Abu Dhabi")]
        AbuDhabi = 1,
        Dubai = 2,
        Sharjah = 3,
        Ajman = 4,
        [Display(Name = "Umm Al Quwain")]
        UmmAlQuwain = 5,
        [Display(Name = "Ras Al Khaimah")]
        RasAlKhaimah = 6,
        Fujairah = 7

    }
}
