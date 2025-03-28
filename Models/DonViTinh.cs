using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class DonViTinh
    {
        [Key]
        public int MaDonViTinh { get; set; } = 0;

        [StringLength(100, ErrorMessage = "Tên đơn vị không được vượt quá 100 ký tự")]
        public string TenDonViTinh { get; set; } = "";

        // Navigation property
        public ICollection<MatHang> DsMatHang { get; set; } = [];
    }
}