using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class StudentTicketViewModel 
    {
        public class ContributingUsers
        {
            public string lastName { get; set; }
            public string Role { get; set; }
            
        }
        public List<Artifacts> artifacts { get; set; }
        public List<ArtifactTemplate> artifactsTemplet { get; set; }
        
        public Ticket ticket { get; set; }
        public User user { get; set; }
        public Student student { get; set; }
        public List<ContributingUsers> contributorsName { get; set; }
        public Contributors  contributors { get; set; }
        public List<Idea> availableIdeas { get; set; }
        public Proposal proposal { get; set; }
        public List<Comments> comments { get; set; }
        public Comments theComment { get; set; }

        public List<FileInfo> files { get; set; }

        public Submission submission { get; set; }



    }
}