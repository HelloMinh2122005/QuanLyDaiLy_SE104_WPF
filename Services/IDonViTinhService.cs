using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IDonViTinhService
    {
        Task<DonViTinh> GetDonViTinhById(int id);
        Task<IEnumerable<DonViTinh>> GetAllDonViTinh();
        Task<IEnumerable<DonViTinh>> GetDonViTinhPage(int offset, int size = 12);
        Task<int> GetTotalPages(int size = 12);
        Task AddDonViTinh(DonViTinh donViTinh);
        Task UpdateDonViTinh(DonViTinh donViTinh);
        Task DeleteDonViTinh(int id);
        Task<int> GenerateAvailableId();
    }
}