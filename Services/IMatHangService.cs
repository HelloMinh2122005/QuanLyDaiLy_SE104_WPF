using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IMatHangService
    {
        Task<MatHang> GetMatHangById(int id);
        Task<IEnumerable<MatHang>> GetAllMatHang();
        Task<IEnumerable<MatHang>> GetMatHangPage(int offset, int size);
        Task AddMatHang(MatHang matHang);
        Task UpdateMatHang(MatHang matHang);
        Task DeleteMatHang(int id);
        Task<MatHang> GetMatHangByTenMatHang(string tenMatHang);
        Task<int> GenerateAvailableId();
    }
}