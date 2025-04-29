using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class ThamSo
    {
        [Key]
        public int Id { get; set; } = 0;
        public int SoLuongDaiLyToiDa { get; set; } = 0;
        public bool QuyDinhSoLuongDaiLyToiDa { get; set; } = true;
        public bool QuyDinhTienThuTienNo { get; set; } = true;
    }
}
