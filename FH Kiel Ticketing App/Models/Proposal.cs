using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Proposal
    {
        [Key]
        public int recordID { get; set; }

        [DisplayName("Name of the Project")]
        public string nameOfProject { get; set; }

        [DisplayName("Abstract")]
        public string abstrac { get; set; }

        [DisplayName("Introduction")]
        public string introduction { get; set; }

        [DisplayName("Overall Description")]
        public string overallDescription { get; set; }

        [DisplayName("Function Requirements")]
        public string functionalRequirements { get; set; }

        [DisplayName("Non-Function Requirements")]
        public string nonFunctionalRequirements { get; set; }

        [DisplayName("Project Technologies")]
        public string projectTechnologies  { get; set; }

        [DisplayName("Result")]
        public string result { get; set; }

        //public virtual User User { get; set; }
    }
}