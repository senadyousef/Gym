using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.DTOs.User
{
    public class UpdateMobileDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile is required.")]
        //[StringLength(255, ErrorMessage = "Must be between 9 and 255 characters", MinimumLength = 9)]
        [DataType(DataType.Text)]
        public string Mobile { get; set; }
    }
}
