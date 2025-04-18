using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Helpers;
using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<DaiLy> DsDaiLy { get; set; } = null!;
    public DbSet<LoaiDaiLy> DsLoaiDaiLy { get; set; } = null!;
    public DbSet<Quan> DsQuan { get; set; } = null!;
    public DbSet<PhieuThu> DsPhieuThu { get; set; } = null!;
    public DbSet<PhieuXuat> DsPhieuXuat { get; set; } = null!;
    public DbSet<ChiTietPhieuXuat> DsChiTietPhieuXuat { get; set; } = null!;
    public DbSet<MatHang> DsMatHang { get; set; } = null!;
    public DbSet<DonViTinh> DsDonViTinh { get; set; } = null!;
    public DbSet<ThamSo> DsThamSo { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // DaiLy (1:n) <- (1:1) LoaiDaiLy
        modelBuilder.Entity<DaiLy>()
            .HasOne(d => d.LoaiDaiLy)
            .WithMany(l => l.DsDaiLy)
            .HasForeignKey(d => d.MaLoaiDaiLy)
            .OnDelete(DeleteBehavior.Cascade);

        // DaiLy (1:n) <- (1:1) Quan
        modelBuilder.Entity<DaiLy>()
            .HasOne(d => d.Quan)
            .WithMany(q => q.DsDaiLy)
            .HasForeignKey(d => d.MaQuan)
            .OnDelete(DeleteBehavior.Cascade);

        // PhieuThu (1:n) <- (1:1) DaiLy
        modelBuilder.Entity<PhieuThu>()
            .HasOne(p => p.DaiLy)
            .WithMany(d => d.DsPhieuThu)
            .HasForeignKey(p => p.MaDaiLy)
            .OnDelete(DeleteBehavior.Cascade);

        // PhieuXuat (1:n) <- (1:1) DaiLy
        modelBuilder.Entity<PhieuXuat>()
            .HasOne(p => p.DaiLy)
            .WithMany(d => d.DsPhieuXuat)
            .HasForeignKey(p => p.MaDaiLy)
            .OnDelete(DeleteBehavior.Cascade);

        // ChiTietPhieuXuat (1:n) <- (1:1) PhieuXuat
        modelBuilder.Entity<ChiTietPhieuXuat>()
            .HasOne(c => c.PhieuXuat)
            .WithMany(p => p.DsChiTietPhieuXuat)
            .HasForeignKey(c => c.MaPhieuXuat)
            .OnDelete(DeleteBehavior.Cascade);

        // ChiTietPhieuXuat (1:n) <- (1:1) MatHang
        modelBuilder.Entity<ChiTietPhieuXuat>()
            .HasOne(c => c.MatHang)
            .WithMany(m => m.DsChiTietPhieuXuat)
            .HasForeignKey(c => c.MaMatHang)
            .OnDelete(DeleteBehavior.Cascade);

        // MatHang (1:n) <- (1:1) DonViTinh
        modelBuilder.Entity<MatHang>()
            .HasOne(m => m.DonViTinh)
            .WithMany(d => d.DsMatHang)
            .HasForeignKey(m => m.MaDonViTinh)
            .OnDelete(DeleteBehavior.Cascade);

        DatabaseSeeder.Seed(modelBuilder);
    }
}
