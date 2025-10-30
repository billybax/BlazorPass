using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR; 
using BlazorPass.Hubs; 

namespace BlazorPass.Services
{
    public class PasswordEntryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<TableUpdateHub> _hubContext;

		public PasswordEntryService(ApplicationDbContext context, IHubContext<TableUpdateHub> hubContext) 
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<List<LocPass>> GetPasswordEntriesAsync()
        {
            return await _context.LocPasses.ToListAsync();
        }

        public async Task<LocPass> GetPasswordEntryByIdAsync(int id)
        {
            return await _context.LocPasses.FindAsync(id);
        }

        public async Task AddPasswordEntryAsync(LocPass entry)
        {
            _context.LocPasses.Add(entry);
            await _context.SaveChangesAsync();
            // Отправляем уведомление после добавления
            await _hubContext.Clients.All.SendAsync("RefreshTable");
        }

        public async Task UpdatePasswordEntryAsync(LocPass entry)
        {
            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            // Отправляем уведомление после обновления
            await _hubContext.Clients.All.SendAsync("RefreshTable");
        }

        public async Task DeletePasswordEntryAsync(int id)
        {
            var entry = await _context.LocPasses.FindAsync(id);
            if (entry != null)
            {
                _context.LocPasses.Remove(entry);
                await _context.SaveChangesAsync();
                // Отправляем уведомление после удаления
                await _hubContext.Clients.All.SendAsync("RefreshTable");
            }
        }
    }
}