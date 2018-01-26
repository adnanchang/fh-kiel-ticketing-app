using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class RoleIdentifier
    {
        [Key]
        [Required]
        public int recordID { get; set; }

        [Required(ErrorMessage = "A Role name is required")]
        [DisplayName("Role")]
        public string role { get; set; }

        public virtual ICollection<RoleIdentifierDetails> RoleIdentifierDetails { get; set; }
    }
}