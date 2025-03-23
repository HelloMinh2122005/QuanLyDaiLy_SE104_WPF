using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IDonViTinhService
    {
        Task<DonViTinh> GetDonViTinhById(int id);
        Task<IEnumerable<DonViTinh>> GetAllDonViTinh();
        Task AddDonViTinh(DonViTinh donViTinh);
        Task UpdateDonViTinh(DonViTinh donViTinh);
        Task DeleteDonViTinh(int id);
    }
}