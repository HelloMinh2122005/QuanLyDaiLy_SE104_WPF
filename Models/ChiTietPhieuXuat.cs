using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class ChiTietPhieuXuat
    {
        [Key]
        public long MaChiTietPhieuXuat { get; set; } = 0;
        public long MaPhieuXuat { get; set; } = 0;
        public long MaMatHang { get; set; } = 0;
        public int SoLuongXuat { get; set; } = 0;
        public long DonGia { get; set; } = 0;
        public long ThanhTien { get; set; } = 0;

        // Navigation properties
        public PhieuXuat PhieuXuat { get; set; } = new();
        public MatHang MatHang { get; set; } = new();
    }
}