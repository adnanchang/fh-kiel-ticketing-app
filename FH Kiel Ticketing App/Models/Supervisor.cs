﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public string userType { get; set; }

        [DisplayName("Email Frequency (Days)")]
        [DataType(DataType.Text)]
        public string daysForReport { get; set; }
        

        [NotMapped]
        public int[] selectedFields { get; set; }
        public virtual ICollection<Fields> Fields { get; set; }
    }
}