using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clienta.Models
{
    public class Client : Person
    {
        [Display(Name = "Consultant")]
        public int? EmployeeId { get; set; }
        [JsonIgnore]
        public virtual Employee Employee { get; set; }
        public virtual List<Address> Addresses { get; set; }
    }
}
