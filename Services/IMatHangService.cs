using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IMatHangService
    {
        Task<MatHang> GetMatHangById(long id);
        Task<IEnumerable<MatHang>> GetAllMatHang();
        Task AddMatHang(MatHang matHang);
        Task UpdateMatHang(MatHang matHang);
        Task DeleteMatHang(long id);
        Task<MatHang> GetMatHangByTenMatHang(string tenMatHang);
    }
}