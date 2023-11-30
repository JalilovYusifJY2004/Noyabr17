using _16noyabr.DAL;
using Microsoft.EntityFrameworkCore;

namespace _16noyabr.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public  async Task<Dictionary<string,string>> GetSettings()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            return settings;
                }
    }
}
