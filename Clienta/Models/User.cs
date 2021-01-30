using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        public string Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Your password must be longer than 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool PersistSession { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastLogin { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
