using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Ticket
    {
        [Key, ForeignKey("Idea")]
        [Required]
        public int recordID { get; set; }

        [Required(ErrorMessage = "The ticket needs a title")]
        [DisplayName("Title")]
        public string title { get; set; }

        [DisplayName("Status")]
        public string status { get; set; }

        [DisplayName("Times Ticket Rejected")]
        public int timesRejected { get; set; }

        public virtual ICollection<Contributors> Contributors { get; set; }

        public virtual Idea Idea { get; set; }

    }
}