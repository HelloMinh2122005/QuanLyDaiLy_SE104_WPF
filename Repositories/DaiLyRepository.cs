using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Data;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories
{
    public class DaiLyRepository : IDaiLyService
    {
        private readonly DataContext _context;

        public DaiLyRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task AddDaiLy(DaiLy daiLy)
        {
            _context.DsDaiLy.Add(daiLy);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalDaiLyUpToMonthYear(int month, int year)
        {
            // Xác định ngày cuối của tháng/year
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            // Đếm tất cả đại lý có NgayTiepNhan <= endDate
            return await _context.DsDaiLy
                .CountAsync(d => d.NgayTiepNhan <= endDate);
        }


        public async Task<List<DaiLy>> GetDaiLysByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.DsDaiLy
                    .AsNoTracking()
                    .Where(d => ids.Contains(d.MaDaiLy))
                    .ToListAsync();
        }
        public async Task DeleteDaiLy(int id)
        {
            var daiLy = await _context.DsDaiLy.FindAsync(id);
            if (daiLy != null)
            {
                _context.DsDaiLy.Remove(daiLy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DaiLy>> GetAllDaiLy()
        {
            return await _context.DsDaiLy
                .Include(d => d.LoaiDaiLy)
                .Include(d => d.Quan)
                .ToListAsync();
        }

        public async Task<DaiLy> GetDaiLyById(int id)
        {
            DaiLy? existingDaiLy = await _context.DsDaiLy
                                            .Include(d => d.LoaiDaiLy)
                                            .Include(d => d.Quan)
                                            .FirstOrDefaultAsync(d => d.MaDaiLy == id);
            return existingDaiLy ?? throw new Exception("DaiLy not found!");
        }

        public async Task<DaiLy> GetDaiLyByTenDaiLy(string tenDaiLy)
        {
            DaiLy? daiLy = await _context.DsDaiLy.FirstAsync(d => d.TenDaiLy == tenDaiLy);
            return daiLy ?? throw new Exception("DaiLy not found!");
        }

        public async Task UpdateDaiLy(DaiLy daiLy)
        {
            _context.Entry(daiLy).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> GenerateAvailableId()
        {
            int maxId = await _context.DsDaiLy.MaxAsync(d => d.MaDaiLy);
            return maxId + 1;
        }
    }
}