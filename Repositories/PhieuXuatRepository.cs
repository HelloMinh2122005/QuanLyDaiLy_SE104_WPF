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

        public async Task<long> GetTotalPhieuXuatByYear(int year)
        {
            // Xác định ngày bắt đầu và ngày kết thúc của năm
            var startDate = new DateTime(year, 1, 1); // Ngày 1 tháng 1 của năm
            var endDate = new DateTime(year, 12, 31); // Ngày 31 tháng 12 của năm
            // Đếm tất cả phiếu xuất có NgayLapPhieu trong khoảng thời gian này và tính tổng giá trị
            return await _context.DsPhieuXuat
                .Where(p => p.NgayLapPhieu >= startDate && p.NgayLapPhieu <= endDate)
                .SumAsync(p => p.TongTriGia);
        }

        public async Task<long> GetToltalPhieuXuatBySingleDate(DateTime date)
        {
            // Lấy danh sách các phiếu thu cho ngày cụ thể
            var phieuThus = await _context.DsPhieuXuat
                .Where(p => p.NgayLapPhieu.Year == date.Year && p.NgayLapPhieu.Month == date.Month && p.NgayLapPhieu.Day == date.Day)
                .ToListAsync();  // Lấy tất cả phiếu thu trong ngày

            // Nếu không có phiếu thu, trả về 0
            if (phieuThus == null || !phieuThus.Any())
            {
                return 0;
            }

            // Nếu có phiếu thu, tính tổng số tiền thu
            return phieuThus.Sum(p => p.TongTriGia);
        }
        public async Task<IEnumerable<PhieuXuat>> GetPhieuXuatByCurrentYearAndLastYear(int currentYear, int lastYear)
        {
            return await _context.DsPhieuXuat
                .Include(p => p.DaiLy)
                .Where(p => new[] { currentYear, lastYear }.Contains(p.NgayLapPhieu.Year))
                .ToListAsync();

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