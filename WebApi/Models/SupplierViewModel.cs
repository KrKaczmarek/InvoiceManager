using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Models
{
    public class SupplierViewModel
    {
        [Required(ErrorMessage = "Cannot be empty")]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "Max 2 characters")]
        public string SupplierId { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = "Max 40 characters")]
        public string SupplierName { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        public string SupplierCountryId { get; set; }
        public CountryViewModel SupplierCountry { get; set; }

        public SelectList CountryList { get; set; }
    }
}