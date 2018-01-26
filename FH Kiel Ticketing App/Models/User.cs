using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class User
    {
        [Required]
        [Key]
        public int recordID { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [DisplayName("First Name")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [DisplayName("Last Name")]
        public string lastName { get; set; }

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email.")]
        [Required(ErrorMessage = "Email is Required")]
        [DisplayName("Email")]
        public string email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to retype your password")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [NotMapped]
        [Compare("password", ErrorMessage = "It seems like the passwords don't match, please retype.")]
        public string confirmPassword { get; set; }

        [Required(ErrorMessage = "Matrkel Number is Required")]
        [DisplayName("Matrikel Number")]
        [DataType(DataType.Text)]
        public int matrikelNumber { get; set; }

        public bool isEmailVerified { get; set; }

        public System.Guid activationCode { get; set; }
    }
}