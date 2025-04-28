using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class ChiTietPhieuXuat
    {
        [Key]
        public int MaChiTietPhieuXuat { get; set; } = 0;
        public int MaPhieuXuat { get; set; } = 0;
        public int MaMatHang { get; set; } = 0;
        public int SoLuongXuat { get; set; } = 0;
        public long DonGia { get; set; } = 0;
        public long ThanhTien { get; set; } = 0;


        // Navigation properties
        public PhieuXuat PhieuXuat { get; set; } = null!;
        public MatHang MatHang { get; set; } = null!;
    }
}