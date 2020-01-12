using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max 50 characters")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max 50 characters")]
        public string EmployeeSurname { get; set; }

        [Required(ErrorMessage = "Cannot be empty")]
        [Range(0.001, int.MaxValue, ErrorMessage = "Must be >0")]
        public decimal EmployeeSalary { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        public string Gender { get; set; }

    }
}