using System.ComponentModel.DataAnnotations;

namespace FacilityServices.Model
{
    public class FacilityModel
    {
        public int Id { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string FacilityImage { get; set; }
        public string FacilityMaxCap { get; set; }
        public bool IsOpen { get; set; }
    }
}
