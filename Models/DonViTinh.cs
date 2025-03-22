using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class DonViTinh
    {
        [Key]
        public long MaDonViTinh { get; set; } = 0;
        public string TenDonViTinh { get; set; } = "";

        // Navigation property
        public ICollection<MatHang> DsMatHang { get; set; } = [];
    }
}