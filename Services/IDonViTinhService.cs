using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IDonViTinhService
    {
        Task<DonViTinh> GetDonViTinhById(long id);
        Task<IEnumerable<DonViTinh>> GetAllDonViTinh();
        Task AddDonViTinh(DonViTinh donViTinh);
        Task UpdateDonViTinh(DonViTinh donViTinh);
        Task DeleteDonViTinh(long id);
    }
}