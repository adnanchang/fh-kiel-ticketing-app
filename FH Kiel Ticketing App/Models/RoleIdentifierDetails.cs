using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class RoleIdentifierDetails
    {
        [Key]
        [Required]
        public int recordID { get; set; }

        [Required(ErrorMessage ="An identifier for a role is required")]
        [DisplayName("Identifier")]
        public string identifier { get; set; }

        public virtual RoleIdentifier RoleIdentifier { get; set; }
    }
}