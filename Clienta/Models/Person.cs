using Clienta.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Models
{
    public abstract class Person
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Forename maximum length reached")]
        public string Forename { get; set; }
        [Required]
        [MaxLength(255, ErrorMessage = "Surname maximum length reached")]
        public string Surname { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Get the Person's full (concatenated) name
        /// </summary>
        /// <returns>string - Concatenated names of Person</returns>
        public string GetFullname()
        {
            return $"{this.Forename} {this.Surname}";
        }

        /// <summary>
        /// Get the Person's Date of Birth formatted for readable display
        /// </summary>
        /// <returns>string - Formatted string representing short-form Date</returns>
        public string GetFormattedDateOfBirth()
        {
            var dobString = FormattingService.ConvertToShortUkFormat(this.DateOfBirth);
            return $"{dobString}";
        }
    }
}
