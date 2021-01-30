using Clienta.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clienta.Models
{
    public class Employee : Person
    {
        [Display(Name = "Employee Number")]
        public int EmployeeNumber { get; set;  } = new Random().Next(100000, 999999);
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual List<Client> Clients { get; set; }

        /// <summary>
        /// Get the Employee's Start Date formatted for readable display
        /// </summary>
        /// <returns>string - Formatted string representing short-form Date</returns>
        public string GetFormattedStartDate()
        {
            var startDateString = FormattingService.ConvertToShortUkFormat(this.StartDate);
            return $"{startDateString}";
        }
    }
}
