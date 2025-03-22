using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IPhieuThuService
    {
        Task<PhieuThu> GetPhieuThuById(long id);
        Task<IEnumerable<PhieuThu>> GetAllPhieuThu();
        Task AddPhieuThu(PhieuThu phieuThu);
        Task UpdatePhieuThu(PhieuThu phieuThu);
        Task DeletePhieuThu(long id);
        Task<IEnumerable<PhieuThu>> GetPhieuThuByDaiLyId(long maDaiLy);
        Task<IEnumerable<PhieuThu>> GetPhieuThuByDateRange(DateTime startDate, DateTime endDate);
    }
}