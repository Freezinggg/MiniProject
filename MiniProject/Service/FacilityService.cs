using MiniProject.Models;
using MiniProject.Models.Facility;
using Newtonsoft.Json;
using RestSharp;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace MiniProject.Service
{
    public interface IFacilityService
    {
        public Task<ApiResponse<List<FacilityModel>>> GetAll();
        public Task<ApiResponse<FacilityModel>> GetById(int id);
        public Task<ApiResponse<object>> Delete(int id);
        public Task<ApiResponse<FacilityModel>> Save(FacilityModel model);
    }

    public class FacilityService : IFacilityService
    {
        private readonly RestClient _client;

        public FacilityService()
        {
            _client = new RestClient("http://localhost:5002/api/Facility/");
        }

        public async Task<ApiResponse<object>> Delete(int id)
        {
            try
            {
                var request = new RestRequest($"Delete/{id}");
                var response = await _client.ExecuteDeleteAsync(request);

                return JsonConvert.DeserializeObject<ApiResponse<object>>(response.Content);
            }
            catch
            {
                return new ApiResponse<object>()
                {
                    message = "Error deleting facility."
                };
            }
        }

        public async Task<ApiResponse<List<FacilityModel>>> GetAll()
        {
            try
            {
                var request = new RestRequest("Get");
                var response = await _client.ExecuteGetAsync(request);
                return JsonConvert.DeserializeObject<ApiResponse<List<FacilityModel>>>(response.Content);
            }
            catch
            {
                return new ApiResponse<List<FacilityModel>>()
                {
                    message = "Error get facilities.",
                    data = new List<FacilityModel>(),
                };
            }
        }

        public async Task<ApiResponse<FacilityModel>> GetById(int id)
        {
            try
            {
                var request = new RestRequest($"Get/{id}");
                var response = await _client.ExecuteGetAsync(request);

                return JsonConvert.DeserializeObject<ApiResponse<FacilityModel>>(response.Content);
            }
            catch
            {
                return new ApiResponse<FacilityModel>()
                {
                    message = "Error get facility.",
                    data = null
                };
            }
        }

        public async Task<ApiResponse<FacilityModel>> Save(FacilityModel model)
        {
            try
            {
                var request = new RestRequest();
                var response = new RestResponse();

                request.AddJsonBody(model, contentType: "application/json");
                if (model.Id == 0)
                {
                    request.Resource = "Save";
                    response = await _client.ExecutePostAsync(request);
                }
                else
                {
                    request.Resource = "Update";
                    response = await _client.ExecutePutAsync(request);
                }

                return JsonConvert.DeserializeObject<ApiResponse<FacilityModel>>(response.Content);
            }
            catch
            {
                return new ApiResponse<FacilityModel>()
                {
                    message = "Failed to save facility.",
                    data = null,
                };
            }
        }
    }
}
