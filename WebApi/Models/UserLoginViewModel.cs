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
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max 50 characters")]        
        public string UserName { get; set; }
        [Required(ErrorMessage = "Cannot be empty.Try Aladeen or Green")]      
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max 50 characters")]
        public string UserPassword { get; set; }
        
    }
}