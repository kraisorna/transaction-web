using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace TransactionWeb.Models
{
    public class UploadViewModel
    {
        public string Note { get; set; }

        [Required]
        [Display(Name = "File")]
        public IFormFile File { set; get; }
    }
}
