﻿using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Services
{
    public interface IPhieuXuatService
    {
        Task<PhieuXuat> GetPhieuXuatById(int id);
        Task<IEnumerable<PhieuXuat>> GetAllPhieuXuat();
        Task AddPhieuXuat(PhieuXuat phieuXuat);
        Task UpdatePhieuXuat(PhieuXuat phieuXuat);
        Task DeletePhieuXuat(int id);
        Task<IEnumerable<PhieuXuat>> GetPhieuXuatByDaiLyId(int maDaiLy);
        Task<IEnumerable<PhieuXuat>> GetPhieuXuatByDateRange(DateTime startDate, DateTime endDate);
        Task<int> GenerateAvailableId();

        Task<Dictionary<int, long>> GetTotalValueByDaiLyAsync(int month, int year);
        Task<long> GetTotalPhieuXuatByCurrentMonthYear(int month, int year);

    }
}