using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Artifacts
    {
        [Key]
        public int RecID { get; set; }

        public virtual Ticket ticket { get; set; }
        public string content { get; set; }
        public DateTime creationDate { get; set; }
        public virtual User user { get; set; }
        public virtual ArtifactTemplate ArtifactTemplate { get; set; }


    }
}