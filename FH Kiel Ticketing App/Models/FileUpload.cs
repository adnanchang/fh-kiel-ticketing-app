using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace FH_Kiel_Ticketing_App.Models
{
    public class FileUpload
    {
        [Required]
        [Display(Name="Title")]
        [StringLength(60,MinimumLength =3)]
        public string Title { get; set; }

        [Required]
        [Display(Name ="File")]
        public IFormFile UploadFile { get; set; }
    }
}