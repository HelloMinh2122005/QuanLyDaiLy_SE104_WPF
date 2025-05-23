﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuanLyDaiLy.Models.dto
{
    public partial class BaoCaoCongNo : ObservableObject
    {
        public int STT { get; set; }
        public string TenDaiLy { get; set; }
        public decimal NoDauThang { get; set; }
        public decimal NoCuoiThang { get; set; }
        public decimal GiaTriGiaoDich { get; set; }
    }
}
