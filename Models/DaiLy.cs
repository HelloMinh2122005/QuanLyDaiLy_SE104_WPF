using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class DaiLy
    {
        [Key]
        public long MaDaiLy { get; set; } = 0;
        public string TenDaiLy { get; set; } = "";
        public long MaLoaiDaiLy { get; set; } = 0;
        public long MaQuan { get; set; } = 0;
        public string DiaChi { get; set; } = "";
        public string DienThoai { get; set; } = "";
        public DateTime NgayTiepNhan { get; set; } = DateTime.Now;
        public long TienNo { get; set; } = 0;

        // Navigation properties
        public LoaiDaiLy LoaiDaiLy { get; set; } = new();
        public Quan Quan { get; set; } = new();
        public ICollection<PhieuThu> DsPhieuThu { get; set; } = [];
        public ICollection<PhieuXuat> DsPhieuXuat { get; set; } = [];
    }
}