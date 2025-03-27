using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IMatHangService
    {
        Task<MatHang> GetMatHangById(int id);
        Task<IEnumerable<MatHang>> GetAllMatHang();
        Task AddMatHang(MatHang matHang);
        Task UpdateMatHang(MatHang matHang);
        Task DeleteMatHang(int id);
        Task<MatHang> GetMatHangByTenMatHang(string tenMatHang);
        Task<int> GenerateAvailableId();
    }
}