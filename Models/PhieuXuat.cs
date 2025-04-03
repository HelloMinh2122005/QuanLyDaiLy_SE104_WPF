using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class PhieuXuat
    {
        [Key]
        public int MaPhieuXuat { get; set; } = 0;
        public int MaDaiLy { get; set; } = 0;
        public DateTime NgayLapPhieu { get; set; } = DateTime.Now;
        public long TongTriGia { get; set; } = 0;

        // Navigation properties
        public DaiLy DaiLy { get; set; } = null!;
        public ICollection<ChiTietPhieuXuat> DsChiTietPhieuXuat { get; set; } = [];
    }
}