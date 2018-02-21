using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Idea
    {
        [Key]
        [Required]
        public int recordID { get; set; }

        [Required(ErrorMessage = "The Idea needs a title")]
        [DisplayName("Title")]
        public string title { get; set; }

        [Required(ErrorMessage = "The Idea needs a small description")]
        [DisplayName("Description")]
        public string description { get; set; }

        [Required(ErrorMessage = "The Idea needs a type")]
        public string type { get; set; }

        public virtual User User { get; set; } //The person who created the idea

        [Required(ErrorMessage = "The Idea needs a field")]
        [DisplayName("Fields")]
        public string field { get; set; }

        

        //The connection to proposal file goes here
    }
}