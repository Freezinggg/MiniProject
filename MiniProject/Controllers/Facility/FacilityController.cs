using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Models.Facility;
using MiniProject.Service;

//using MiniProject.Service;
using System.Threading.Tasks;

namespace MiniProject.Controllers.Facility
{
    public class FacilityController : Controller
    {
        private readonly IFacilityService _facilityService;


        public FacilityController(IFacilityService facilitySservice)
        {
            _facilityService = facilitySservice;
        }

        public async Task<ActionResult> Index()
        {
            var facilities = await _facilityService.GetAll();
            return View(facilities.data.OrderBy(x => x.FacilityName));
        }

        public ActionResult Create()
        {
            return PartialView("/Views/Facility/_FacilityForm.cshtml", new FacilityModel());
        }

        public async Task<ActionResult> Edit(int id)
        {
            var facility = await _facilityService.GetById(id);
            return PartialView("/Views/Facility/_FacilityForm.cshtml", facility.data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save([FromForm] FacilityModel m)
        {
            try
            {
                string base64 = "";

                if (m.UploadedImage != null && m.UploadedImage.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        m.UploadedImage.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        base64 = Convert.ToBase64String(fileBytes);
                    }
                }
                m.FacilityImage = base64;

                var response = await _facilityService.Save(m);

                return Json(response);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _facilityService.Delete(id);
                return Json(response);
            }
            catch
            {
                return View();
            }
        }
    }
}
