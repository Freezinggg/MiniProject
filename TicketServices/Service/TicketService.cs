using Microsoft.EntityFrameworkCore;
using TicketServices.Model;

namespace TicketServices.Service
{
    public interface ITicketService
    {
        public Task<List<Ticket>> GetAll();
        public Task<Ticket?> GetById(int id);
        //public Task<bool> Delete(int id);
        //public Task<bool> Update(int id, Ticket updatedTicket);
        public Task<Ticket> Book(Ticket ticket);
        public Task<List<Ticket>?> GetBookingCountByDayAndFacility(int facility);
    }

    public class TicketService : ITicketService
    {
        private readonly TicketDBContext _context;

        public TicketService(TicketDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            Ticket? ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Ticket>> GetAll()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<List<Ticket>?> GetBookingCountByDayAndFacility(int facility)
        {
            DateTime dtStart = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day,
                hour: 0, minute: 0, second: 0);

            DateTime dtEnd = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day,
                hour: 23, minute: 59, second: 59);


            var result = await _context.Tickets.Where(x => dtStart <= x.TicketValidFrom && x.TicketValidFrom <= dtEnd).ToListAsync();

            return result;
        }

        public async Task<Ticket?> GetById(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<Ticket> Book(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }
    }
}
