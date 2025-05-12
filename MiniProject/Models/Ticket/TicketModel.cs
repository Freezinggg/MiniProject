using System.ComponentModel.DataAnnotations;

namespace MiniProject.Models.Ticket
{
    public class TicketModel
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string TicketID { get; set; }

        [Required, Display(Name = "Name")]
        public string TicketHolderName { get; set; }

        [Required, Display(Name = "Email"), EmailAddress]
        public string TicketHolderEmail { get; set; }

        public DateTime TicketValidFrom { get; set; }
        public DateTime TicketValidTo { get; set; }
        public int Pax { get; set; }
    }
}
