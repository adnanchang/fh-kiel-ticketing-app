using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Files
    {
        [Key]
        [Required]
        public int RecID { get; set; }
        [Required]
        public virtual User User { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public string File { get; set; }

    }
}