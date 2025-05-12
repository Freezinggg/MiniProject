using Microsoft.AspNetCore.Mvc;
using MiniProject.Models.Ticket;
using MiniProject.Service;
using System.Threading.Tasks;

namespace MiniProject.Controllers.Ticket
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IFacilityService _facilityService;
        public TicketController(ITicketService ticketService, IFacilityService facilityService)
        {
            _ticketService = ticketService;
            _facilityService = facilityService;
        }

        public async Task<IActionResult> Index()
        {
            var facilities = await _facilityService.GetAll();
            return View(facilities.data.Where(x => x.IsOpen).OrderBy(x => x.FacilityName));
        }

        public async Task<IActionResult> Create(int facilityId)
        {
            var facility = await _facilityService.GetById(facilityId);
            return PartialView("/Views/Ticket/_BookTicket.cshtml", new TicketModel() { FacilityId = facilityId, FacilityName = facility.data.FacilityName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book([FromForm] TicketModel ticket)
        {
            ticket.TicketID = Guid.NewGuid().ToString();
            var response = await _ticketService.Book(ticket);
            return Json(response);
        }
    }
}
