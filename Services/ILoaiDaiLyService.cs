using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface ILoaiDaiLyService
    {
        Task<LoaiDaiLy> GetLoaiDaiLyById(int id);
        Task<IEnumerable<LoaiDaiLy>> GetAllLoaiDaiLy();
        Task<IEnumerable<LoaiDaiLy>> GetLoaiDaiLyPage(int offset, int size = 12);
        Task<int> GetTotalPages(int size = 12);
        Task AddLoaiDaiLy(LoaiDaiLy loaiDaiLy);
        Task UpdateLoaiDaiLy(LoaiDaiLy loaiDaiLy);
        Task DeleteLoaiDaiLy(int id);
        Task<int> GenerateAvailableId();
    }
}