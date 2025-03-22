using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class PhieuXuat
    {
        [Key]
        public long MaPhieuXuat { get; set; } = 0;
        public long MaDaiLy { get; set; } = 0;
        public DateTime NgayLapPhieu { get; set; } = DateTime.Now;
        public long TongTriGia { get; set; } = 0;

        // Navigation properties
        public DaiLy DaiLy { get; set; } = new();
        public ICollection<ChiTietPhieuXuat> DsChiTietPhieuXuat { get; set; } = [];
    }
}