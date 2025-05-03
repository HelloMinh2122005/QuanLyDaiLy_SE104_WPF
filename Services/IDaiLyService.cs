using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IDaiLyService
    {
        Task<DaiLy> GetDaiLyById(int id);
        Task<IEnumerable<DaiLy>> GetAllDaiLy();
        Task AddDaiLy(DaiLy daiLy);
        Task UpdateDaiLy(DaiLy daiLy);
        Task DeleteDaiLy(int id);
        Task<DaiLy> GetDaiLyByTenDaiLy(string tenDaiLy);
        Task<int> GenerateAvailableId();
        Task<List<DaiLy>> GetDaiLysByIdsAsync(IEnumerable<int> ids);
        Task<int> GetTotalDaiLyUpToMonthYear(int month, int year);
        Task<DateTime?> GetEarliestDaiLyDateAsync();
    }
}