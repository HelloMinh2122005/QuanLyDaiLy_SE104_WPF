using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Data;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories
{
    public class PhieuThuRepository : IPhieuThuService
    {
        private readonly DataContext _context;

        public PhieuThuRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task AddPhieuThu(PhieuThu phieuThu)
        {
            _context.DsPhieuThu.Add(phieuThu);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePhieuThu(int id)
        {
            var phieuThu = await _context.DsPhieuThu.FindAsync(id);
            if (phieuThu != null)
            {
                _context.DsPhieuThu.Remove(phieuThu);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PhieuThu>> GetAllPhieuThu()
        {
            return await _context.DsPhieuThu
                .Include(p => p.DaiLy)
                .ToListAsync();
        }

        public async Task<PhieuThu> GetPhieuThuById(int id)
        {
            PhieuThu? phieuThu = await _context.DsPhieuThu
                .Include(p => p.DaiLy)
                .FirstOrDefaultAsync(p => p.MaPhieuThu == id);
            return phieuThu ?? throw new Exception("PhieuThu not found!");
        }

        public async Task UpdatePhieuThu(PhieuThu phieuThu)
        {
            _context.Entry(phieuThu).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        async public Task<IEnumerable<PhieuThu>> GetPhieuThuByDaiLyId(int maDaiLy)
        {
            return await _context.DsPhieuThu
                .Include(p => p.DaiLy)
                .Where(p => p.MaDaiLy == maDaiLy)
                .ToListAsync();
        }

        public async Task<IEnumerable<PhieuThu>> GetPhieuThuByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.DsPhieuThu
                .Include(p => p.DaiLy)
                .Where(p => p.NgayThuTien >= startDate && p.NgayThuTien <= endDate)
                .ToListAsync();
        }
    }
}