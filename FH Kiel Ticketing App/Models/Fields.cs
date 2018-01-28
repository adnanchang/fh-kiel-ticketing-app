using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Fields
    {
        [Key]
        [Required]
        public int recordID { get; set; }

        [DisplayName("Field")]
        public string field { get; set; }

        public virtual ICollection<Supervisor> Supervisor { get; set; }

    }
}