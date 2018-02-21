using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FH_Kiel_Ticketing_App.Models
{
    public class ProposalIdeaFieldViewModel
    {
        public User user { get; set; }
        public Student student { get; set; }
        public Ticket ticket { get; set; }
        public List<Idea> availableIdeas { get; set; }
        public Proposal proposal { get; set; }
        public Idea idea { get; set; }
        public List<Fields> AllFields { get; set; }
        public List<Supervisor> AllSupervisor { get; set; }
        public int supervisor { get; set; }

    }
}