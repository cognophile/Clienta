using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@".*@example.co.uk$", ErrorMessage = "Email must under the @example.co.uk domain")]
        [MaxLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        public string Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Your password must be longer than 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Your password must be longer than 8 characters")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hmm... looks like those passwords don't match!")]
        public string ConfirmPassword { get; set; }
    }
}
