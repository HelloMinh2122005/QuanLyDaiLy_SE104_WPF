using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Data;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories
{
    public class QuanRepository : IQuanService
    {
        private readonly DataContext _context;

        public QuanRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task AddQuan(Quan quan)
        {
            _context.DsQuan.Add(quan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuan(int id)
        {
            var quan = await _context.DsQuan.FindAsync(id);
            if (quan != null)
            {
                _context.DsQuan.Remove(quan);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Quan>> GetAllQuan()
        {
            return await _context.DsQuan
                .Include(q => q.DsDaiLy)
                .ToListAsync();
        }

        public async Task<Quan> GetQuanById(int id)
        {
            Quan? quan = await _context.DsQuan
                            .Include(q => q.DsDaiLy)
                            .FirstOrDefaultAsync(q => q.MaQuan == id);
            return quan ?? throw new Exception("Quan not found!");
        }

        public async Task<Quan> GetQuanByTenQuan(string tenQuan)
        {
            Quan? quan = await _context.DsQuan.Include(q => q.DsDaiLy).FirstOrDefaultAsync(q => q.TenQuan == tenQuan);
            return quan ?? throw new Exception("Quan not found!");
        }

        public async Task UpdateQuan(Quan quan)
        {
            _context.Entry(quan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> GenerateAvailableId()
        {
            var quan = await _context.DsQuan.OrderByDescending(q => q.MaQuan).FirstOrDefaultAsync();
            return quan?.MaQuan + 1 ?? 1;
        }
    }
}