using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.DTOs.User
{
    public class UpdateDisplayNameDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Display Name is required.")]
        //[StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Text)]
        public string DisplayName { get; set; }
    }
}
