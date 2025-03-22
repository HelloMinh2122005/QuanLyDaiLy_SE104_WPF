using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IChiTietPhieuXuatService
    {
        Task<ChiTietPhieuXuat> GetChiTietPhieuXuatById(long id);
        Task<IEnumerable<ChiTietPhieuXuat>> GetAllChiTietPhieuXuat();
        Task AddChiTietPhieuXuat(ChiTietPhieuXuat chiTietPhieuXuat);
        Task UpdateChiTietPhieuXuat(ChiTietPhieuXuat chiTietPhieuXuat);
        Task DeleteChiTietPhieuXuat(long id);
        Task<IEnumerable<ChiTietPhieuXuat>> GetChiTietPhieuXuatByPhieuXuatId(long maPhieuXuat);
    }
}