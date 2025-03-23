using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface ILoaiDaiLyService
    {
        Task<LoaiDaiLy> GetLoaiDaiLyById(int id);
        Task<IEnumerable<LoaiDaiLy>> GetAllLoaiDaiLy();
        Task AddLoaiDaiLy(LoaiDaiLy loaiDaiLy);
        Task UpdateLoaiDaiLy(LoaiDaiLy loaiDaiLy);
        Task DeleteLoaiDaiLy(int id);
    }
}