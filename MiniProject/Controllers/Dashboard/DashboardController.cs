using Microsoft.AspNetCore.Mvc;
using MiniProject.Models.Ticket;
using MiniProject.Service;

namespace MiniProject.Controllers.Dashboard
{
    public class DashboardController : Controller
    {
        private readonly IFacilityService _facilityService;
        private readonly ITicketService _ticketService;

        public DashboardController(IFacilityService facilityService, ITicketService ticketService)
        {
            _facilityService = facilityService;
            _ticketService = ticketService;
        }

        public async Task<IActionResult> Index()
        {
            var facilitiesGet = await _facilityService.GetAll();
            var facilities = facilitiesGet.data;

            var tickets = await _ticketService.GetAll();

            List<TicketModel> ticketModels = new();
            foreach (TicketModel ticket in tickets.data)
            {
                TicketModel m = ticket;
                m.FacilityName = facilities.Where(x => x.Id == m.FacilityId).FirstOrDefault().FacilityName;

                ticketModels.Add(m);
            }


            return View(ticketModels);
        }
    }
}
