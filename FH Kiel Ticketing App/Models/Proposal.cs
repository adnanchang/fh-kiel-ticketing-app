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

        [Required(ErrorMessage = "Name of the Project is Required")]
        [DisplayName("Name of the Project")]
        public string nameOfProject { get; set; }

        [Required(ErrorMessage = "Abstract is Required")]
        [DisplayName("Abstract")]
        public string abstrac { get; set; }

        [Required(ErrorMessage = "Introduction is Required")]
        [DisplayName("Introduction")]
        public string introduction { get; set; }

        [Required(ErrorMessage = "Overall Description is Required")]
        [DisplayName("Overall Description")]
        public string overallDescription { get; set; }

        [Required(ErrorMessage = "Function Requirements is Required")]
        [DisplayName("Function Requirements")]
        public string functionalRequirements { get; set; }


        [Required(ErrorMessage = "Non-Function Requirements is Required")]
        [DisplayName("Non-Function Requirements")]
        public string nonFunctionalRequirements { get; set; }

        [Required(ErrorMessage = "Project Technologies is Required")]
        [DisplayName("Project Technologies")]
        public string projectTechnologies { get; set; }

        [Required(ErrorMessage = "Result is Required")]
        [DisplayName("Result")]
        public string result { get; set; }

        
        [Required]
        public virtual Idea Idea { get; set; }

        public virtual User User { get; set; }
    }
}