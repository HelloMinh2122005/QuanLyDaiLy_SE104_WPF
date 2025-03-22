using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class ThamSo
    {
        [Key]
        public long Id { get; set; } = 0;
        public int SoLuongDaiLyToiDa { get; set; } = 0;
        public bool QuyDinhTienThuTienNo { get; set; } = true;
    }
}
