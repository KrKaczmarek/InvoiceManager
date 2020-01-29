using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web.Security;

namespace WebApi.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Cannot be empty. Try Haffaz or Eva")]    
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Max 20 characters")]        
        public string UserName { get; set; }
        [Required(ErrorMessage = "Cannot be empty.Try Aladeen or Green")]      
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Max 30 characters")]
        public string UserPassword { get; set; }

        public string UserRole { get; set; }



    }
}