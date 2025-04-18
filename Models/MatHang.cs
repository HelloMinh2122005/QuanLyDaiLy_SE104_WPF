using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class MatHang
    {
        [Key]
        public int MaMatHang { get; set; } = 0;

        [Required(ErrorMessage = "Tên mặt hàng không được để trống")]
        [StringLength(100, ErrorMessage = "Tên mặt hàng không được vượt quá 100 ký tự")]
        public string TenMatHang { get; set; } = "";
        public int MaDonViTinh { get; set; } = 0;
        public int SoLuongTon { get; set; } = 0;

        // Navigation properties
        public DonViTinh DonViTinh { get; set; } = null!;
        public ICollection<ChiTietPhieuXuat> DsChiTietPhieuXuat { get; set; } = [];
    }
}