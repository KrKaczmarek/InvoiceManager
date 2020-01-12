using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class CountryViewModel
    {
        [Required(ErrorMessage = "Cannot be empty")]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "Max 2 characters")]
        public string CountryId { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Max 60 characters")]
        public string CountryName { get; set; }
    }
}