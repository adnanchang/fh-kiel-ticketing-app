﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class TicketStatus
    {
        [Key]
        public int recordID { get; set; }

        [DisplayName("Ticket Status")]
        public string ticketStatus { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}