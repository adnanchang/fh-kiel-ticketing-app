using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Submission
    {
        [Key]
        public int RecID { get; set; }
        public virtual User User { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual ICollection<Files> Files{get; set;}
        public DateTime submissionDate { get; set; }
    }
}