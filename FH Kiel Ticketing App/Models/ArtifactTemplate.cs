using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class ArtifactTemplate
    {
        [Key]
        public int RecID { get; set; }
        public string name { get; set; }
        public ICollection<Artifacts> artifacts { get; set; }
        public virtual User user { get; set; }
        public virtual Fields Field { get; set; }

    }
}