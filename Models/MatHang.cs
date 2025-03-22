using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class MatHang
    {
        [Key]
        public long MaMatHang { get; set; } = 0;
        public string TenMatHang { get; set; } = "";
        public long MaDonViTinh { get; set; } = 0;
        public int SoLuongTon { get; set; } = 0;

        // Navigation properties
        public DonViTinh DonViTinh { get; set; } = new();
        public ICollection<ChiTietPhieuXuat> DsChiTietPhieuXuat { get; set; } = [];
    }
}