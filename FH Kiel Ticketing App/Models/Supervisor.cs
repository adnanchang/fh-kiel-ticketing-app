using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Supervisor
    {
        [Key, ForeignKey("User")]
        public int recordID { get; set; }

        public virtual User User { get; set; }

        public string field { get; set; }
    }
}