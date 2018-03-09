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
        [Key]
        [Required]
        public int recordID { get; set; }

        [Required(ErrorMessage = "The ticket needs a title")]
        [DisplayName("Title")]
        public string title { get; set; }

        [DisplayName("Times Ticket Rejected")]
        public int timesRejected { get; set; }
        public string tickettype { get; set; } //Thesis, project,

        public DateTime creationDate { get; set; }

        public virtual ICollection<Comments> Comments { get; set; }

        public virtual Idea idea { get; set; }
        public virtual User User { get; set; }
        public virtual TicketStatus ticketStatus { get; set; }

        public ICollection<Artifacts> artifacts { get; set; }
        


    }
}