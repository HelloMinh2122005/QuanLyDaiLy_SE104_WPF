using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class DaiLy
    {
        [Key]
        public int MaDaiLy { get; set; } = 0;
        public string TenDaiLy { get; set; } = "";
        public int MaLoaiDaiLy { get; set; } = 0;
        public int MaQuan { get; set; } = 0;
        public string DiaChi { get; set; } = "";
        public string DienThoai { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime NgayTiepNhan { get; set; } = DateTime.Now;
        public long TienNo { get; set; } = 0;

        // Navigation properties
        public LoaiDaiLy LoaiDaiLy { get; set; } = new();
        public Quan Quan { get; set; } = new();
        public ICollection<PhieuThu> DsPhieuThu { get; set; } = [];
        public ICollection<PhieuXuat> DsPhieuXuat { get; set; } = [];
    }
}