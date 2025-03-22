using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyDaiLy.Models
{
    public class Quan
    {
        [Key]
        public long MaQuan { get; set; } = 0;
        public string TenQuan { get; set; } = "";

        // Navigation property
        public ICollection<DaiLy> DsDaiLy { get; set; } = [];
    }
}