using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IDaiLyService
    {
        Task<DaiLy> GetDaiLyById(long id);
        Task<IEnumerable<DaiLy>> GetAllDaiLy();
        Task AddDaiLy(DaiLy daiLy);
        Task UpdateDaiLy(DaiLy daiLy);
        Task DeleteDaiLy(long id);
        Task<DaiLy> GetDaiLyByTenDaiLy(string tenDaiLy);
    }
}