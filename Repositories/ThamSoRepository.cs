using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Data;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.Repositories
{
    public class ThamSoRepository : IThamSoService
    {
        private readonly DataContext _context;

        public ThamSoRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task<ThamSo> GetThamSo()
        {
            ThamSo? thamSo = await _context.DsThamSo.FirstOrDefaultAsync();
            return thamSo ?? throw new Exception("ThamSo not found!");
        }

        public async Task UpdateThamSo(ThamSo thamSo)
        {
            _context.Entry(thamSo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task InitializeThamSo(ThamSo thamSo)
        {
            if (!await _context.DsThamSo.AnyAsync())
            {
                _context.DsThamSo.Add(thamSo);
                await _context.SaveChangesAsync();
            }
        }
    }
}