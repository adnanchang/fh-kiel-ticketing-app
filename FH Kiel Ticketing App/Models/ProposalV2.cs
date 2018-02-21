using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FH_Kiel_Ticketing_App.Models
{
    public class ProposalV2
    {
        [Key]
        public int RecID { get; set; }

        public virtual User UserRecID { get; set; }
        public virtual Idea IdeaRecID { get; set; }
    }
}