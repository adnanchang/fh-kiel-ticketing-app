using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Notification
    {
        [Key]
        [Required]
        public int recordID { get; set; }

        [Required(ErrorMessage = "A Notification Message is required")]
        public string message { get; set; }

        [Required(ErrorMessage = "Notification URL is required")]
        public string url { get; set; }

        public bool isRead { get; set; }

        public virtual User User { get; set; }
    }
}