using FacilityServices.Interface;
using FacilityServices.Model;
using FacilityServices.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacilityServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacilityController : ControllerBase
    {
        private readonly ILogger<FacilityController> _logger;
        private readonly FacilityDbContext _facilityDbContext;
        private readonly IFacilityService _service;
        public FacilityController(ILogger<FacilityController> logger, FacilityDbContext facilityDbContext, IFacilityService service)
        {
            _logger = logger;
            _facilityDbContext = facilityDbContext;
            _service = service;
        }

        [Route("/api/Facility/Get/{id}")]
        [HttpGet]
        public async Task<IActionResult> FacilityGet(int id)
        {
            Facility? f = await _service.GetById(id);

            if (f == null)
                return BadRequest(new ApiResponse<object>(false, 400, "Facility not found.", null));

            return Ok(new ApiResponse<Facility>(true, 200, "Successs", f));
        }

        [Route("/api/Facility/Get")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Facility>? facilities = await _service.GetAll();

            return Ok(new ApiResponse<List<Facility>>(true, 200, "Successs", facilities));
        }

        [Route("/api/Facility/Save")]
        [HttpPost]
        public async Task<IActionResult> FacilitySave([FromBody] FacilityModel m)
        {
            try
            {
                //Check duplicate facility
                if (IsDuplicate(m.FacilityCode, m.Id)) return BadRequest(new ApiResponse<object>(false, 400, "Duplicate facility, please use another code.", null));

                Facility f = await _service.Save(new()
                {
                    FacilityCode = m.FacilityCode,
                    FacilityName = m.FacilityName,
                    FacilityImage = m.FacilityImage,
                    FacilityMaxCap = m.FacilityMaxCap,
                    IsOpen = m.IsOpen,
                    CreatedDate = DateTime.Now
                });

                if (f.Id == 0) return BadRequest(new ApiResponse<Facility>(false, 400, "Save facility failed, please make sure all the data are correct.", f));
                return Ok(new ApiResponse<Facility>(true, 200, "Save facility success.", f));
            }
            catch
            {
                return BadRequest(new ApiResponse<object>(false, 400, "Save facility failed, please try again.", null));
            }
        }

        [Route("/api/Facility/Update")]
        [HttpPut]
        public async Task<IActionResult> FacilityUpdate([FromBody] FacilityModel m)
        {
            try
            {
                if (IsDuplicate(m.FacilityCode, m.Id)) return BadRequest(new ApiResponse<object>(false, 400, "Duplicate facility, please use another code.", null));

                Facility f;
                bool success = await _service.Update(m.Id, f = new()
                {
                    FacilityName = m.FacilityName,
                    FacilityImage = m.FacilityImage,
                    FacilityMaxCap = m.FacilityMaxCap,
                    IsOpen = m.IsOpen
                });

                if (!success) BadRequest(new ApiResponse<Facility>(false, 400, "Update facility failed, please make sure all the data are correct.", f));

                return Ok(new ApiResponse<Facility>(true, 200, "Update Facility success.", await _service.GetById(m.Id)));
            }
            catch
            {
                return BadRequest(new ApiResponse<object>(false, 400, "Update facility failed, please try again.", null));
            }
        }


        [Route("/api/Facility/Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> FacilityDelete(int id)
        {
            try
            {
                if (!await IsExist(id)) return BadRequest(new ApiResponse<object>(false, 400, "Delete facility failed, please try again.", null));

                //Delete the data first, then check if it still lingers in the db.
                await _service.Delete(id);
                Facility exist = await _service.GetById(id);

                if (exist != null) return BadRequest(new ApiResponse<object>(false, 400, "Delete facility failed, please try again.", null));
                return Ok(new ApiResponse<object>(true, 200, "Delete facility success.", null));

            }
            catch
            {
                return BadRequest(new ApiResponse<object>(false, 400, "Delete facility failed, please try again.", null));
            }
        }

        public bool IsDuplicate(string facilityCode, int facilityId)
        {
            if (_facilityDbContext.Facilities.Where(x => x.FacilityCode == facilityCode && x.Id != facilityId).FirstOrDefault() != null) return true;
            else return false;
        }

        public async Task<bool> IsExist(int id)
        {
            Facility facility = await _service.GetById(id);
            if (facility == null) return false;
            else return true;
        }
    }
}
