namespace TicketServices.Model
{
    public class TicketModel
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public string TicketID { get; set; }
        public string TicketHolderName { get; set; }
        public string TicketHolderEmail { get; set; }
        public DateTime TicketValidFrom { get; set; }
        public DateTime TicketValidTo { get; set; }
        public int Pax { get; set; }
    }
}
