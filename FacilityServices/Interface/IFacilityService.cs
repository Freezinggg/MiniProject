using FacilityServices.Model;
using System.Runtime.CompilerServices;

namespace FacilityServices.Interface
{
    public interface IFacilityService
    {
        public Task<List<Facility>> GetAll();
        public Task<Facility?> GetById(int id);
        public Task<bool> Delete(int id);
        public Task<bool> Update(int id, Facility updatedFacility);
        public Task<Facility> Save(Facility facility);
    }
}
