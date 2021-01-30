using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clienta.Models
{
    public class Address
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First line of address is required")]
        [MaxLength(255, ErrorMessage = "Line cannot exceed 255 characters")]
        [Display(Name = "Line one")]
        public string LineOne { get; set; }
        [MaxLength(255, ErrorMessage = "Line cannot exceed 255 characters")]
        [Display(Name = "Line two")]
        public string LineTwo { get; set; }
        [Required(ErrorMessage = "City is required")]
        [MaxLength(255, ErrorMessage = "City cannot exceed 255 characters")]
        public string City { get; set; }
        [Required(ErrorMessage = "County is required")]
        [MaxLength(255, ErrorMessage = "County cannot exceed 255 characters")]
        public string County { get; set; }
        /// <summary>
        /// UK Government suggested REGEX for UK Postcodes 
        ///     <see cref="https://stackoverflow.com/a/164994/5012644"/>
        ///     <see cref="https://webarchive.nationalarchives.gov.uk/+/http://www.cabinetoffice.gov.uk/media/291370/bs7666-v2-0-xsd-PostCodeType.htm"/>
        /// </summary>
        [Required(ErrorMessage = "Postcode is required")]
        [MaxLength(8)]
        [RegularExpression(
            @"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})", 
            ErrorMessage = "Invalid postcode"
        )]
        public string Postcode { get; set; }
        public int ClientId { get; set; }
        [JsonIgnore]
        public virtual Client Client { get; set; }

        /// <summary>
        /// Obtain a concatenated string of all Address properties
        /// </summary>
        /// <returns>string - The concatenated Address</returns>
        public override string ToString()
        {
            return $"{this.LineOne}, {this.LineTwo}, {this.City}, {this.County}, {this.Postcode}";
        }
    }
}
