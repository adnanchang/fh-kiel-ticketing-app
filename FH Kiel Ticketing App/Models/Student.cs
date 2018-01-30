using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class Student
    {
        [Key, ForeignKey("User")]
        public int recordID { get; set; }

        [Required(ErrorMessage = "Matrkel Number is Required")]
        [DisplayName("Matrikel Number")]
        [DataType(DataType.Text)]
        public int matrikelNumber { get; set; }

        [DisplayName("Beginning Semester")]
        public string beginningSemesterSeason { get; set; }

        [DisplayName("Beginning Year")]
        [DataType(DataType.Text)]
        public int beginningSemesterYear { get; set; }

        public string userType { get; set; }

        public virtual User User { get; set; }
    }
}