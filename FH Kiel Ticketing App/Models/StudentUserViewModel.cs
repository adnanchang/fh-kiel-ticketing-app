using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class StudentUserViewModel
    {
        public User user { get; set; }
        public Student student { get; set; }
        public Ticket ticket { get; set; }
        public List<Idea> availableIdeas { get; set; }
    }
}