using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IThamSoService
    {
        Task<ThamSo> GetThamSo();
        Task UpdateThamSo(ThamSo thamSo);
    }
}