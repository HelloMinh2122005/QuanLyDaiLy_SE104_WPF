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

        public async Task<long> GetTotalPhieuThuByYear(int year)
        {
            // Xác định ngày bắt đầu và ngày kết thúc của năm
            var startDate = new DateTime(year, 1, 1); // Ngày 1 tháng 1 của năm
            var endDate = new DateTime(year, 12, 31); // Ngày 31 tháng 12 của năm

            // Đếm tất cả phiếu thu có NgayThuTien trong khoảng thời gian này và tính tổng giá trị
            return await _context.DsPhieuThu
                .Where(p => p.NgayThuTien >= startDate && p.NgayThuTien <= endDate)
                .SumAsync(p => p.SoTienThu);
        }
        public async Task<IEnumerable<PhieuThu>> GetPhieuThuByCurrentYearAndLastYear(int currentYear, int lastYear)
        {
            return await _context.DsPhieuThu
                    .Include(p => p.DaiLy)
                    .Where(p => new[] { currentYear, lastYear }.Contains(p.NgayThuTien.Year))
                    .ToListAsync();
        }

        public async Task<long> GetTotalPhieuThuUpToMonthYear(int month, int year)
        {
            // Xác định ngày đầu và ngày cuối của tháng/year
            var startDate = new DateTime(year, month, 1);  // Ngày đầu tháng
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));  // Ngày cuối tháng

            // Tính tổng phiếu thu có NgayThuTien trong khoảng từ startDate đến endDate
            return await _context.DsPhieuThu
                .Where(p => p.NgayThuTien >= startDate && p.NgayThuTien <= endDate)
                .SumAsync(p => p.SoTienThu);
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

        public async Task<long> GetToltalPhieuThuBySingleDate(DateTime date)
        {
            // Lấy danh sách các phiếu thu cho ngày cụ thể
            var phieuThus = await _context.DsPhieuThu
                .Where(p => p.NgayThuTien.Year == date.Year && p.NgayThuTien.Month == date.Month && p.NgayThuTien.Day == date.Day)
                .ToListAsync();  // Lấy tất cả phiếu thu trong ngày

            // Nếu không có phiếu thu, trả về 0
            if (phieuThus == null || !phieuThus.Any())
            {
                return 0;
            }

            // Nếu có phiếu thu, tính tổng số tiền thu
            return phieuThus.Sum(p => p.SoTienThu);
        }

        public async Task<IEnumerable<PhieuThu>> GetPhieuThuByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.DsPhieuThu
                .Include(p => p.DaiLy)
                .Where(p => p.NgayThuTien >= startDate && p.NgayThuTien <= endDate)
                .ToListAsync();
        }

        public async Task<int> GenerateAvailableId()
        {
            int maxId = await _context.DsPhieuThu.MaxAsync(d => d.MaPhieuThu);
            return maxId + 1;
        }
    }
}