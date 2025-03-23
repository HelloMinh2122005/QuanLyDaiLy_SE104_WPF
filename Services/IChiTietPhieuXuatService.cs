using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IChiTietPhieuXuatService
    {
        Task<ChiTietPhieuXuat> GetChiTietPhieuXuatById(int id);
        Task<IEnumerable<ChiTietPhieuXuat>> GetAllChiTietPhieuXuat();
        Task AddChiTietPhieuXuat(ChiTietPhieuXuat chiTietPhieuXuat);
        Task UpdateChiTietPhieuXuat(ChiTietPhieuXuat chiTietPhieuXuat);
        Task DeleteChiTietPhieuXuat(int id);
        Task<IEnumerable<ChiTietPhieuXuat>> GetChiTietPhieuXuatByPhieuXuatId(int maPhieuXuat);
    }
}