using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketServices.Model;
using TicketServices.Service;

namespace TicketServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly TicketDBContext _ticketDbContext;
        private readonly ITicketService _service;

        public TicketController(ILogger<TicketController> logger, TicketDBContext ticketDbContext, ITicketService service)
        {
            _logger = logger;
            _ticketDbContext = ticketDbContext;
            _service = service;
        }

        [Route("/api/Ticket/Get")]
        [HttpGet]
        public async Task<IActionResult> TicketGetAll()
        {
            return Ok(new ApiResponse<List<Ticket>>(true, 200, "Successs", await _service.GetAll()));
        }

        [Route("/api/Ticket/Get/{id}")]
        [HttpGet]
        public async Task<IActionResult> TicketGet(int id)
        {
            Ticket? ticket = await _service.GetById(id);
            if(ticket == null) return BadRequest(new ApiResponse<object>(false, 400, "Ticket not found.", null));

            return Ok(new ApiResponse<Ticket>(true, 200, "Successs", ticket));
        }

        [Route("/api/Ticket/GetBookings/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetBookingCountByDayAndFacility(int id)
        {
            List<Ticket?> tickets = new List<Ticket?>();
            tickets = await _service.GetBookingCountByDayAndFacility(id);

            return Ok(new ApiResponse<List<Ticket>?>(true, 200, "Successs", tickets));
        }

        [Route("/api/Ticket/Book")]
        [HttpPost]
        public async Task<IActionResult> Book([FromBody] TicketModel model)
        {
            try
            {
                Ticket t = new()
                {
                    FacilityId = model.FacilityId,
                    TicketID = model.TicketID,
                    TicketHolderName = model.TicketHolderName,
                    TicketHolderEmail = model.TicketHolderEmail,
                    TicketValidFrom = DateTime.Now,
                    TicketValidTo = DateTime.Now.AddHours(5),
                    Pax = model.Pax
                };

                await _service.Book(t);

                if (t.Id == 0) return BadRequest(new ApiResponse<Ticket>(false, 400, "Booking ticket failed.", t));
                else return Ok(new ApiResponse<Ticket>(true, 200, "Booking ticket success.", t));
            }
            catch
            {
                return BadRequest(new ApiResponse<Ticket>(false, 400, "Booking ticket failed.", null));
            }
            
        }
    }
}
