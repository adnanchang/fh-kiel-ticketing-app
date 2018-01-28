using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class SupervisorUserViewModel
    {
        public User user { get; set; }
        public Supervisor supervisor { get; set; }
    }
}