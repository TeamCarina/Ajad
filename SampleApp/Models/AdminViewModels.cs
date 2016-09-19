using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SampleApp.Models
{
    public class AccountSettingViewModel
    {
      
        [Display(Name = "AppId")]
        public string AppId { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
    }

    public class AddCampiagnViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "URL is required")]
        [Display(Name = "URL")]
        [Url(ErrorMessage = "Please enter a valid url")]
        public string URL { get; set; }
    }
}