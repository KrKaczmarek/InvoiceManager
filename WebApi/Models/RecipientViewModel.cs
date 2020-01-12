using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Models
{
    public class RecipientViewModel
    {
        [Required(ErrorMessage = "Cannot be empty")]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "Max 2 characters")]
        public string RecipientId { get; set; }

        [Required(ErrorMessage = "Cannot be empty")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max 50 characters")]
        public string RecipientName { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        public string RecipientCountryId { get; set; }
        public CountryViewModel RecipientCountry { get; set; }
        public SelectList CountryList { get; set; }
    }
}