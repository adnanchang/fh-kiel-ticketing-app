using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Contributors
    {
        [Key]
        [Required]
        public int recordID { get; set; }

        public virtual User User { get; set; }

        public virtual Ticket Ticket { get; set; }

        [Required(ErrorMessage = "Status of the member is required")]
        [DisplayName("Status")]
        public string status { get; set; }
        [Required]
        public string Role { get; set; }
        public int FuckYou { get; set; }
    }
}