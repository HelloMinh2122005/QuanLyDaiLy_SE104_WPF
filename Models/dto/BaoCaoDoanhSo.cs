using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDaiLy.Models.dto
{
    public class BaoCaoDoanhSo
    {
        public int STT { get; set; }
        public string TenDaiLy { get; set; }
        public int SoLuongPhieuXuat { get; set; }
        public decimal TongGiaTriGiaoDich { get; set; }
        public double TiLe { get; set; }
    }

}
