using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Models
{
    public class CreateViewModel
    {
      
        public InvoiceViewModel Invoice { get; set; }              
      
        public SelectList EmployeeList { get; set; }
        public SelectList RecipientList { get; set; }
        public SelectList SupplierList { get; set; }
        public SelectList DuckList { get; set; }
   
    
    }
}