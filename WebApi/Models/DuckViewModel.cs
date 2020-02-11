using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Models
{
    public class DuckViewModel
    {
        public int DuckId { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = "Max 40 characters")]
        public string DuckType { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        [Range(0.001,int.MaxValue, ErrorMessage = "Must be >0")]
        public decimal DuckPrice { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        public string DuckCountryId { get; set; }
        public CountryViewModel DuckCountry { get; set; }
        public SelectList CountryList { get; set; }
    }
}