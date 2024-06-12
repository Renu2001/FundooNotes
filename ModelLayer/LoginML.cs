using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class LoginML
    {
        [Required]
        [EmailAddress]
        public string email {  get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, and one digit")]
        [Required]
        public string password { get; set; }

    }
}
