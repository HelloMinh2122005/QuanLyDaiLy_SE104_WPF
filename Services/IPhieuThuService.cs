using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IPhieuThuService
    {
        Task<PhieuThu> GetPhieuThuById(int id);
        Task<IEnumerable<PhieuThu>> GetAllPhieuThu();
        Task AddPhieuThu(PhieuThu phieuThu);
        Task UpdatePhieuThu(PhieuThu phieuThu);
        Task DeletePhieuThu(int id);
        Task<IEnumerable<PhieuThu>> GetPhieuThuByDaiLyId(int maDaiLy);
        Task<IEnumerable<PhieuThu>> GetPhieuThuByDateRange(DateTime startDate, DateTime endDate);
        Task<int> GenerateAvailableId();
        Task<IEnumerable<PhieuThu>> GetPhieuThuByCurrentYearAndLastYear(int currentYear,int lastYear);
        Task<long> GetTotalPhieuThuUpToMonthYear(int month, int year);
        Task<long> GetTotalPhieuThuByYear(int year);
        Task<long> GetToltalPhieuThuBySingleDate(DateTime date);
    }
}