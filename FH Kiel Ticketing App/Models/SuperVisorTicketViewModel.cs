using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class SuperVisorTicketViewModel
    {
        public User user { get; set; }
        public Supervisor supervisor { get; set; }
        public Ticket ticket { get; set; }
        public List<Ticket> tickets { get; set; }
        public List<Idea> availableIdeas { get; set; }
    }
}