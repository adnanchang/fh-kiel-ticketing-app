using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class SupervisorUserFieldViewModel
    {
        public User user { get; set; }
        public Supervisor supervisor { get; set; }
        public List<Fields> AllFields { get; set; }
        public List<Fields> SupervisorFields { get; set; }
        public int[] selectedFields { get; set; }
    }
}