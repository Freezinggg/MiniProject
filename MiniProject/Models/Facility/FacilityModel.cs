using System.ComponentModel.DataAnnotations;

namespace MiniProject.Models.Facility
{
    public class FacilityModel
    {
        public int Id { get; set; }

        [Required, Display(Name = "Code")]
        public string FacilityCode { get; set; }

        [Required, Display(Name = "Name")]
        public string FacilityName { get; set; }

        [Required, Display(Name = "Image")]
        public string FacilityImage { get; set; }

        [Required, Display(Name = "Capacity")]
        public string FacilityMaxCap { get; set; }

        //[Required]
        [Display(Name = "Open")]
        public bool IsOpen { get; set; }

        public IFormFile? UploadedImage { get; set; }
    }
}
