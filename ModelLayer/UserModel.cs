using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class UserModel
    {
        [Required(ErrorMessage = "FirstName is required.")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")]
        public string lastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email")]
        public string email { get; set; }
        
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, and one digit")]
        [Required]
        public string password { get; set; }
        
    }
}
