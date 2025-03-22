using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IQuanService
    {
        Task<Quan> GetQuanById(long id);
        Task<IEnumerable<Quan>> GetAllQuan();
        Task AddQuan(Quan quan);
        Task UpdateQuan(Quan quan);
        Task DeleteQuan(long id);
    }
}