using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class UserEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First letter should be capital")]
        public string firstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First letter should be capital")]
        public string lastName { get; set; }

        [Required]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format")]
        public string email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, and one digit")]
        public string password { get; set; }

        


    }
}
