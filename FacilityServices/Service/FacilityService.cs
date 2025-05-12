using FacilityServices.Interface;
using FacilityServices.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FacilityServices.Service
{
    public class FacilityService : IFacilityService
    {
        private readonly FacilityDbContext _context;

        public FacilityService(FacilityDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            Facility? facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return false;

            _context.Facilities.Remove(facility);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Facility>> GetAll()
        {
            return await _context.Facilities.ToListAsync();
        }

        public async Task<Facility?> GetById(int id)
        {
            return await _context.Facilities.FindAsync(id);
        }

        public async Task<bool> Update(int id, Facility updatedFacility)
        {
            Facility? facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return false;

            facility.FacilityName = updatedFacility.FacilityName;
            if (!string.IsNullOrEmpty(updatedFacility.FacilityImage)) facility.FacilityImage = updatedFacility.FacilityImage;
            facility.FacilityMaxCap = updatedFacility.FacilityMaxCap;
            facility.IsOpen = updatedFacility.IsOpen;
            facility.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Facility> Save(Facility facility)
        {
            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();
            return facility;
        }
    }
}
