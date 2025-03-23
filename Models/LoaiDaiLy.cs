using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class LoaiDaiLy
    {
        [Key]
        public int MaLoaiDaiLy { get; set; } = 0;
        public string TenLoaiDaiLy { get; set; } = "";
        public int NoToiDa { get; set; } = 0;

        // Navigation property
        public ICollection<DaiLy> DsDaiLy { get; set; } = [];
    }
}
