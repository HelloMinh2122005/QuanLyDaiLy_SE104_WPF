using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Data;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories
{
    public class LoaiDaiLyRepository : ILoaiDaiLyService
    {
        private readonly DataContext _context;

        public LoaiDaiLyRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task AddLoaiDaiLy(LoaiDaiLy loaiDaiLy)
        {
            _context.DsLoaiDaiLy.Add(loaiDaiLy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLoaiDaiLy(long id)
        {
            var loaiDaiLy = await _context.DsLoaiDaiLy.FindAsync(id);
            if (loaiDaiLy != null)
            {
                _context.DsLoaiDaiLy.Remove(loaiDaiLy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<LoaiDaiLy> GetLoaiDaiLyById(long id)
        {
            LoaiDaiLy? loaiDaiLy = await _context.DsLoaiDaiLy.FindAsync(id);
            return loaiDaiLy ?? throw new Exception("LoaiDaiLy not found!");
        }

        public async Task<LoaiDaiLy> GetLoaiDaiLyByTenLoaiDaiLy(string tenLoaiDaiLy)
        {
            LoaiDaiLy? loaiDaiLy = await _context.DsLoaiDaiLy.FirstOrDefaultAsync(l => l.TenLoaiDaiLy == tenLoaiDaiLy);
            return loaiDaiLy ?? throw new Exception("LoaiDaiLy not found!");
        }

        public async Task UpdateLoaiDaiLy(LoaiDaiLy loaiDaiLy)
        {
            _context.Entry(loaiDaiLy).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        async Task<IEnumerable<LoaiDaiLy>> ILoaiDaiLyService.GetAllLoaiDaiLy()
        {
            return await _context.DsLoaiDaiLy
                .Include(l => l.DsDaiLy)
                .ToListAsync();
        }
    }
}