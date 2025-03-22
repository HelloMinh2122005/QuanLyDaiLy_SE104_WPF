﻿using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Data;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories
{
    public class MatHangRepository : IMatHangService
    {
        private readonly DataContext _context;

        public MatHangRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task AddMatHang(MatHang matHang)
        {
            _context.DsMatHang.Add(matHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatHang(long id)
        {
            var matHang = await _context.DsMatHang.FindAsync(id);
            if (matHang != null)
            {
                _context.DsMatHang.Remove(matHang);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MatHang>> GetAllMatHang()
        {
            return await _context.DsMatHang
                .Include(m => m.DonViTinh)
                .ToListAsync();
        }

        public async Task<MatHang> GetMatHangById(long id)
        {
            MatHang? matHang = await _context.DsMatHang
                .Include(m => m.DonViTinh)
                .FirstOrDefaultAsync(m => m.MaMatHang == id);
            return matHang ?? throw new Exception("MatHang not found!");
        }

        public async Task<MatHang> GetMatHangByTenMatHang(string tenMatHang)
        {
            MatHang? matHang = await _context.DsMatHang
                .Include(m => m.DonViTinh)
                .FirstOrDefaultAsync(m => m.TenMatHang == tenMatHang);
            return matHang ?? throw new Exception("MatHang not found!");
        }

        public async Task UpdateMatHang(MatHang matHang)
        {
            _context.Entry(matHang).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}