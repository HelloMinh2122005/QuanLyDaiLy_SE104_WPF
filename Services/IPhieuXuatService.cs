using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IPhieuXuatService
    {
        Task<PhieuXuat> GetPhieuXuatById(long id);
        Task<IEnumerable<PhieuXuat>> GetAllPhieuXuat();
        Task AddPhieuXuat(PhieuXuat phieuXuat);
        Task UpdatePhieuXuat(PhieuXuat phieuXuat);
        Task DeletePhieuXuat(long id);
        Task<IEnumerable<PhieuXuat>> GetPhieuXuatByDaiLyId(long maDaiLy);
        Task<IEnumerable<PhieuXuat>> GetPhieuXuatByDateRange(DateTime startDate, DateTime endDate);
    }
}