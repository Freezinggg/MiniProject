using MiniProject.Models;
using MiniProject.Models.Facility;
using MiniProject.Models.Ticket;
using Newtonsoft.Json;
using RestSharp;

namespace MiniProject.Service
{
    public interface ITicketService
    {
        public Task<ApiResponse<List<TicketModel>>> GetAll();
        public Task<ApiResponse<TicketModel>> GetById(int id);
        public Task<ApiResponse<TicketModel>> Book(TicketModel model);
    }
    public class TicketService : ITicketService
    {
        private readonly IFacilityService _facilityService;
        private readonly RestClient _client;

        public TicketService(IFacilityService _service)
        {
            _client = new RestClient("http://localhost:5003/api/Ticket/");
            _facilityService = _service;
        }
        public async Task<ApiResponse<List<TicketModel>>> GetAll()
        {
            var request = new RestRequest("Get");
            var response = await _client.ExecuteGetAsync(request);

            return JsonConvert.DeserializeObject<ApiResponse<List<TicketModel>>>(response.Content);
        }

        public async Task<ApiResponse<TicketModel>> GetById(int id)
        {
            var request = new RestRequest($"Get/{id}");
            var response = await _client.ExecuteGetAsync(request);

            return JsonConvert.DeserializeObject<ApiResponse<TicketModel>>(response.Content);
        }

        public async Task<ApiResponse<TicketModel>> Book(TicketModel model)
        {
            //Get the facility info
            var request = new RestRequest();
            var responseFacility = await _facilityService.GetById(model.FacilityId);
            if (responseFacility != null && Int32.Parse(responseFacility.data.FacilityMaxCap) < model.Pax)
            {
                return new ApiResponse<TicketModel>()
                {
                    status = 400,
                    message = "Pax cannot exceed facility capacity."
                };
            }
            else
            {
                request = new RestRequest($"GetBookings/{model.FacilityId}");
                var responseBooking = await _client.ExecuteGetAsync(request);
                ApiResponse<List<TicketModel>?> listTicketModel = JsonConvert.DeserializeObject<ApiResponse<List<TicketModel>?>>(responseBooking.Content);

                int ticketAvailability = Int32.Parse(responseFacility.data.FacilityMaxCap) - listTicketModel.data.Where(x => x.FacilityId == model.FacilityId).Sum(x => x.Pax);
                if(IsAvailable(ticketAvailability, model.Pax))
                {
                    request = new RestRequest("Book");
                    request.AddJsonBody(model, contentType: "application/json");
                    var response = await _client.ExecutePostAsync(request);

                    return JsonConvert.DeserializeObject<ApiResponse<TicketModel>>(response.Content);
                }
                else
                {
                    return new ApiResponse<TicketModel>()
                    {
                        status = 400,
                        message = $"Ticket is not available for current pax amount. Available ticket: {ticketAvailability}",
                    };
                }
            }
        }

        public bool IsAvailable(int ticketAvailability, int pax)
        {
            if (ticketAvailability > 0 && pax <= ticketAvailability) return true;
            else return false;
        }
    }
}
