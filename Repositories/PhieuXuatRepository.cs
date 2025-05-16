using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Data;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories
{
    public class PhieuXuatRepository : IPhieuXuatService
    {
        private readonly DataContext _context;

        public PhieuXuatRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task<long> GetTotalPhieuXuatByCurrentMonthYear(int month, int year)
        {
            // Xác định ngày đầu và cuối của tháng/year
            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            // Đếm tất cả phiếu xuất có NgayLapPhieu trong khoảng thời gian này
            return await _context.DsPhieuXuat
                .Where(p => p.NgayLapPhieu >= startDate && p.NgayLapPhieu <= endDate)
                .SumAsync(p => p.TongTriGia);
        }

        public async Task<IEnumerable<PhieuXuat>> GetPhieuXuatPage(int offset, int size = 20)
        {
            return await _context.DsPhieuXuat
                .Include(m => m.DaiLy)
                .Skip(offset * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> GetTotalPages(int size = 20)
        {
            int leftover = await _context.DsPhieuXuat.CountAsync() % size;
            int totalPages = await _context.DsPhieuXuat.CountAsync() / size;
            if (leftover > 0)
            {
                totalPages++;
            }
            return totalPages;
        }

        public async Task AddPhieuXuat(PhieuXuat phieuXuat)
        {
            _context.DsPhieuXuat.Add(phieuXuat);
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<int, long>> GetTotalValueByDaiLyAsync(int month, int year)
        {
            // Lấy tổng TongTriGia của mỗi đại lý trong tháng/năm, không giới hạn số lượng
            var list = await _context.DsPhieuXuat
                .AsNoTracking()
                .Where(px => px.NgayLapPhieu.Month == month
                          && px.NgayLapPhieu.Year == year)
                .GroupBy(px => px.MaDaiLy)
                .Select(g => new
                {
                    MaDaiLy = g.Key,
                    TotalValue = g.Sum(px => px.TongTriGia)
                })
                .ToListAsync();

            // Chuyển sang Dictionary<MaDaiLy, TotalValue>
            return list.ToDictionary(x => x.MaDaiLy, x => x.TotalValue);
        }

        public async Task DeletePhieuXuat(int id)
        {
            var phieuXuat = await _context.DsPhieuXuat.FindAsync(id);
            if (phieuXuat != null)
            {
                _context.DsPhieuXuat.Remove(phieuXuat);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PhieuXuat>> GetAllPhieuXuat()
        {
            return await _context.DsPhieuXuat
                .Include(p => p.DaiLy)
                .Include(p => p.DsChiTietPhieuXuat)
                    .ThenInclude(c => c.MatHang)
                        .ThenInclude(m => m.DonViTinh)
                .ToListAsync();
        }

        public async Task<PhieuXuat> GetPhieuXuatById(int id)
        {
            PhieuXuat? phieuXuat = await _context.DsPhieuXuat
                .Include(p => p.DaiLy)
                .Include(p => p.DsChiTietPhieuXuat)
                    .ThenInclude(c => c.MatHang)
                        .ThenInclude(m => m.DonViTinh)
                .FirstOrDefaultAsync(p => p.MaPhieuXuat == id);
            return phieuXuat ?? throw new Exception("PhieuXuat not found!");
        }

        public async Task UpdatePhieuXuat(PhieuXuat phieuXuat)
        {
            _context.Entry(phieuXuat).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PhieuXuat>> GetPhieuXuatByDaiLyId(int maDaiLy)
        {
            return await _context.DsPhieuXuat
                .Include(p => p.DaiLy)
                .Include(p => p.DsChiTietPhieuXuat)
                    .ThenInclude(c => c.MatHang)
                        .ThenInclude(m => m.DonViTinh)
                .Where(p => p.MaDaiLy == maDaiLy)
                .ToListAsync();
        }

        public async Task<IEnumerable<PhieuXuat>> GetPhieuXuatByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.DsPhieuXuat
                .Include(p => p.DaiLy)
                .Include(p => p.DsChiTietPhieuXuat)
                    .ThenInclude(c => c.MatHang)
                        .ThenInclude(m => m.DonViTinh)
                .Where(p => p.NgayLapPhieu >= startDate && p.NgayLapPhieu <= endDate)
                .ToListAsync();
        }

        public async Task<int> GenerateAvailableId()
        {
            int maxId = await _context.DsPhieuXuat.MaxAsync(d => d.MaPhieuXuat);
            return maxId + 1;
        }
    }
}