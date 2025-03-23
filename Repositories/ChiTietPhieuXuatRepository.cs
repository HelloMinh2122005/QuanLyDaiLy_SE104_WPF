using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Data;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories
{
    public class ChiTietPhieuXuatRepository : IChiTietPhieuXuatService
    {
        private readonly DataContext _context;

        public ChiTietPhieuXuatRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task AddChiTietPhieuXuat(ChiTietPhieuXuat chiTietPhieuXuat)
        {
            _context.DsChiTietPhieuXuat.Add(chiTietPhieuXuat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChiTietPhieuXuat(int id)
        {
            var chiTietPhieuXuat = await _context.DsChiTietPhieuXuat.FindAsync(id);
            if (chiTietPhieuXuat != null)
            {
                _context.DsChiTietPhieuXuat.Remove(chiTietPhieuXuat);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ChiTietPhieuXuat>> GetAllChiTietPhieuXuat()
        {
            return await _context.DsChiTietPhieuXuat
                .Include(c => c.PhieuXuat)
                .Include(c => c.MatHang)
                    .ThenInclude(m => m.DonViTinh)
                .ToListAsync();
        }

        public async Task<ChiTietPhieuXuat> GetChiTietPhieuXuatById(int id)
        {
            ChiTietPhieuXuat? chiTietPhieuXuat = await _context.DsChiTietPhieuXuat
                .Include(c => c.PhieuXuat)
                .Include(c => c.MatHang)
                    .ThenInclude(m => m.DonViTinh)
                .FirstOrDefaultAsync(c => c.MaChiTietPhieuXuat == id);
            return chiTietPhieuXuat ?? throw new Exception("ChiTietPhieuXuat not found!");
        }

        public async Task<IEnumerable<ChiTietPhieuXuat>> GetChiTietPhieuXuatByPhieuXuatId(int maPhieuXuat)
        {
            return await _context.DsChiTietPhieuXuat
                .Include(c => c.PhieuXuat)
                .Include(c => c.MatHang)
                    .ThenInclude(m => m.DonViTinh)
                .Where(c => c.MaPhieuXuat == maPhieuXuat)
                .ToListAsync();
        }

        public async Task UpdateChiTietPhieuXuat(ChiTietPhieuXuat chiTietPhieuXuat)
        {
            _context.Entry(chiTietPhieuXuat).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}