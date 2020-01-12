using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Models
{
    public class InvoiceViewModel
    {
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Cannot be empty")]     
        public string RecipientId { get; set; }

        [Required(ErrorMessage = "Cannot be empty")]     
        public string SupplierId { get; set; }

        [Required(ErrorMessage="Cannot be empty")]   
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Cannot be empty")]    
        public int DuckId { get; set; }

        [Required(ErrorMessage = "Cannot be empty")]
        [Range(0.01, int.MaxValue, ErrorMessage = "Must be >0")]
        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        
        public EmployeeViewModel Employee { get; set; }
        public RecipientViewModel Recipient { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public DuckViewModel Duck { get; set; }
        /*
        public SelectList EmployeeList { get; set; }
        public SelectList RecipientList { get; set; }
        public SelectList SupplierList { get; set; }
        public SelectList DuckList { get; set; }
        */
        

    }
}