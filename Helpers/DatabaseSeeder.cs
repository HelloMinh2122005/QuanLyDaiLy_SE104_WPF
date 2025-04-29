using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Helpers
{
    public static class DatabaseSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedThamSo(modelBuilder);
            SeedDonViTinh(modelBuilder);
            SeedQuan(modelBuilder);
            SeedLoaiDaiLy(modelBuilder);
            SeedDaiLy(modelBuilder);
            SeedMatHang(modelBuilder);
            SeedPhieuXuat(modelBuilder);  
            SeedChiTietPhieuXuat(modelBuilder);
            SeedPhieuThu(modelBuilder);
        }

        private static void SeedThamSo(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ThamSo>().HasData(
                new ThamSo { Id = 1, SoLuongDaiLyToiDa = 4, QuyDinhTienThuTienNo = true }
            );
        }

        private static void SeedDonViTinh(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonViTinh>().HasData(
                new DonViTinh { MaDonViTinh = 1, TenDonViTinh = "Kg" },
                new DonViTinh { MaDonViTinh = 2, TenDonViTinh = "Cái" },
                new DonViTinh { MaDonViTinh = 3, TenDonViTinh = "Thùng" },
                new DonViTinh { MaDonViTinh = 4, TenDonViTinh = "Lít" },
                new DonViTinh { MaDonViTinh = 5, TenDonViTinh = "Chai" }
            );
        }

        private static void SeedQuan(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quan>().HasData(
                new Quan { MaQuan = 1, TenQuan = "Quận 1" },
                new Quan { MaQuan = 2, TenQuan = "Quận 2" },
                new Quan { MaQuan = 3, TenQuan = "Quận 3" },
                new Quan { MaQuan = 4, TenQuan = "Quận 4" },
                new Quan { MaQuan = 5, TenQuan = "Quận 5" },
                new Quan { MaQuan = 6, TenQuan = "Quận 6" },
                new Quan { MaQuan = 7, TenQuan = "Quận 7" },
                new Quan { MaQuan = 8, TenQuan = "Quận 8" },
                new Quan { MaQuan = 9, TenQuan = "Quận 9" },
                new Quan { MaQuan = 10, TenQuan = "Quận 10" },
                new Quan { MaQuan = 11, TenQuan = "Quận 11" },
                new Quan { MaQuan = 12, TenQuan = "Quận 12" },
                new Quan { MaQuan = 13, TenQuan = "Quận Bình Thạnh" },
                new Quan { MaQuan = 14, TenQuan = "Quận Tân Bình" },
                new Quan { MaQuan = 15, TenQuan = "Quận Tân Phú" },
                new Quan { MaQuan = 16, TenQuan = "Quận Phú Nhuận" },
                new Quan { MaQuan = 17, TenQuan = "Quận Gò Vấp" },
                new Quan { MaQuan = 18, TenQuan = "Quận Thủ Đức" },
                new Quan { MaQuan = 19, TenQuan = "Huyện Củ Chi" },
                new Quan { MaQuan = 20, TenQuan = "Huyện Hóc Môn" },
                // Hà Nội
                new Quan { MaQuan = 21, TenQuan = "Quận Ba Đình" },
                new Quan { MaQuan = 22, TenQuan = "Quận Hoàn Kiếm" },
                new Quan { MaQuan = 23, TenQuan = "Quận Hai Bà Trưng" },
                new Quan { MaQuan = 24, TenQuan = "Quận Đống Đa" },
                new Quan { MaQuan = 25, TenQuan = "Quận Cầu Giấy" },
                new Quan { MaQuan = 26, TenQuan = "Quận Tây Hồ" },
                new Quan { MaQuan = 27, TenQuan = "Quận Long Biên" },
                new Quan { MaQuan = 28, TenQuan = "Quận Bắc Từ Liêm" },
                new Quan { MaQuan = 29, TenQuan = "Quận Nam Từ Liêm" },
                new Quan { MaQuan = 30, TenQuan = "Quận Hà Đông" },
                // Hải Phòng
                new Quan { MaQuan = 31, TenQuan = "Quận Hồng Bàng" },
                new Quan { MaQuan = 32, TenQuan = "Quận Ngô Quyền" },
                new Quan { MaQuan = 33, TenQuan = "Quận Lê Chân" },
                new Quan { MaQuan = 34, TenQuan = "Quận Kiến An" },
                new Quan { MaQuan = 35, TenQuan = "Quận Dương Kinh" },
                // Đà Nẵng
                new Quan { MaQuan = 36, TenQuan = "Quận Hải Châu" },
                new Quan { MaQuan = 37, TenQuan = "Quận Liên Chiểu" },
                new Quan { MaQuan = 38, TenQuan = "Quận Thanh Khê" },
                new Quan { MaQuan = 39, TenQuan = "Quận Sơn Trà" },
                new Quan { MaQuan = 40, TenQuan = "Quận Ngũ Hành Sơn" },
                // Cần Thơ
                new Quan { MaQuan = 41, TenQuan = "Quận Ninh Kiều" },
                new Quan { MaQuan = 42, TenQuan = "Quận Bình Thủy" },
                new Quan { MaQuan = 43, TenQuan = "Quận Cái Răng" },
                new Quan { MaQuan = 44, TenQuan = "Quận Ô Môn" },
                new Quan { MaQuan = 45, TenQuan = "Quận Thốt Nốt" },
                // Bổ sung miền Đông & ĐBSCL
                new Quan { MaQuan = 46, TenQuan = "Quận Gò Công" },
                new Quan { MaQuan = 47, TenQuan = "Quận Tân Uyên" },
                new Quan { MaQuan = 48, TenQuan = "Quận Trảng Bàng" },
                new Quan { MaQuan = 49, TenQuan = "Huyện Châu Thành" },
                new Quan { MaQuan = 50, TenQuan = "Huyện Tân Hiệp" }
            );
        }

        private static void SeedLoaiDaiLy(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoaiDaiLy>().HasData(
                new LoaiDaiLy { MaLoaiDaiLy = 1, TenLoaiDaiLy = "Đại lý loại 1", NoToiDa = 60_000 },
                new LoaiDaiLy { MaLoaiDaiLy = 2, TenLoaiDaiLy = "Đại lý loại 2", NoToiDa = 80_000 },
                new LoaiDaiLy { MaLoaiDaiLy = 3, TenLoaiDaiLy = "Đại lý loại 3", NoToiDa = 100_000 },
                new LoaiDaiLy { MaLoaiDaiLy = 4, TenLoaiDaiLy = "Đại lý loại 4", NoToiDa = 120_000 },
                new LoaiDaiLy { MaLoaiDaiLy = 5, TenLoaiDaiLy = "Đại lý loại 5", NoToiDa = 135_000 },
                new LoaiDaiLy { MaLoaiDaiLy = 6, TenLoaiDaiLy = "Đại lý loại 6", NoToiDa = 150_000 },
                new LoaiDaiLy { MaLoaiDaiLy = 7, TenLoaiDaiLy = "Đại lý loại 7", NoToiDa = 165_000 },
                new LoaiDaiLy { MaLoaiDaiLy = 8, TenLoaiDaiLy = "Đại lý loại 8", NoToiDa = 180_000 },
                new LoaiDaiLy { MaLoaiDaiLy = 9, TenLoaiDaiLy = "Đại lý loại 9", NoToiDa = 190_000 },
                new LoaiDaiLy { MaLoaiDaiLy = 10, TenLoaiDaiLy = "Đại lý loại 10", NoToiDa = 200_000 }
            );
        }

        private static void SeedDaiLy(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2023, 1, 1);
            modelBuilder.Entity<DaiLy>().HasData(
                // 1-4: Original dai ly
                new
                {
                    MaDaiLy = 1,
                    TenDaiLy = "Đại lý Minh Phát",
                    MaLoaiDaiLy = 1,
                    MaQuan = 1,
                    DiaChi = "12 Nguyễn Huệ",
                    DienThoai = "0901234567",
                    Email = "MinhPhat@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-3),
                    TienNo = 15000000L
                },
                new
                {
                    MaDaiLy = 2,
                    TenDaiLy = "Đại lý Hoàng Gia",
                    MaLoaiDaiLy = 2,
                    MaQuan = 2,
                    DiaChi = "45 Lê Lợi",
                    DienThoai = "0912345678",
                    Email = "HoangGia@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-2),
                    TienNo = 20000000L
                },
                new
                {
                    MaDaiLy = 3,
                    TenDaiLy = "Đại lý Thịnh Vượng",
                    MaLoaiDaiLy = 1,
                    MaQuan = 3,
                    DiaChi = "78 Nguyễn Trãi",
                    DienThoai = "0923456789",
                    Email = "ThinhVuong@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-4),
                    TienNo = 17000000L
                },
                new
                {
                    MaDaiLy = 4,
                    TenDaiLy = "Đại lý Thành Công",
                    MaLoaiDaiLy = 2,
                    MaQuan = 4,
                    DiaChi = "32 Lý Tự Trọng",
                    DienThoai = "0934567890",
                    Email = "ThanhCong@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-1),
                    TienNo = 11000000L
                },
                // 5-14: Additional dai ly
                new
                {
                    MaDaiLy = 5,
                    TenDaiLy = "Đại lý Tân Tiến",
                    MaLoaiDaiLy = 1,
                    MaQuan = 5,
                    DiaChi = "56 Trần Hưng Đạo",
                    DienThoai = "0945678901",
                    Email = "TanTien@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-5),
                    TienNo = 21000000L
                },
                new
                {
                    MaDaiLy = 6,
                    TenDaiLy = "Đại lý Phát Đạt",
                    MaLoaiDaiLy = 2,
                    MaQuan = 6,
                    DiaChi = "123 Nguyễn Văn Cừ",
                    DienThoai = "0956789012",
                    Email = "PhatDat@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-6),
                    TienNo = 4200000L
                },
                new
                {
                    MaDaiLy = 7,
                    TenDaiLy = "Đại lý Kim Cương",
                    MaLoaiDaiLy = 1,
                    MaQuan = 7,
                    DiaChi = "87 Nguyễn Thị Minh Khai",
                    DienThoai = "0967890123",
                    Email = "KimCuong@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-7),
                    TienNo = 12000000L
                },
                new
                {
                    MaDaiLy = 8,
                    TenDaiLy = "Đại lý Hoàng Long",
                    MaLoaiDaiLy = 1,
                    MaQuan = 8,
                    DiaChi = "245 Hai Bà Trưng",
                    DienThoai = "0978901234",
                    Email = "HoangLong@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-2),
                    TienNo = 3500000L
                },
                new
                {
                    MaDaiLy = 9,
                    TenDaiLy = "Đại lý Bình Minh",
                    MaLoaiDaiLy = 2,
                    MaQuan = 9,
                    DiaChi = "67 Phan Đình Phùng",
                    DienThoai = "0989012345",
                    Email = "BinhMinh@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-3),
                    TienNo = 1800000L
                },
                new
                {
                    MaDaiLy = 10,
                    TenDaiLy = "Đại lý Trường Thịnh",
                    MaLoaiDaiLy = 2,
                    MaQuan = 10,
                    DiaChi = "34 Lê Thánh Tôn",
                    DienThoai = "0990123456",
                    Email = "TruongThinh@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-8),
                    TienNo = 6700000L
                },
                new
                {
                    MaDaiLy = 11,
                    TenDaiLy = "Đại lý Tấn Phát",
                    MaLoaiDaiLy = 1,
                    MaQuan = 11,
                    DiaChi = "189 Điện Biên Phủ",
                    DienThoai = "0901234567",
                    Email = "TanPhat@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-4),
                    TienNo = 15000000L
                },
                new
                {
                    MaDaiLy = 12,
                    TenDaiLy = "Đại lý Quang Minh",
                    MaLoaiDaiLy = 1,
                    MaQuan = 12,
                    DiaChi = "76 Võ Văn Tần",
                    DienThoai = "0912345678",
                    Email = "QuangMinh@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-9),
                    TienNo = 4300000L
                },
                new
                {
                    MaDaiLy = 13,
                    TenDaiLy = "Đại lý Hưng Thịnh",
                    MaLoaiDaiLy = 2,
                    MaQuan = 13,
                    DiaChi = "54 Cách Mạng Tháng 8",
                    DienThoai = "0923456789",
                    Email = "HungThinh@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-5),
                    TienNo = 2100000L
                },
                new
                {
                    MaDaiLy = 14,
                    TenDaiLy = "Đại lý Phú Quý",
                    MaLoaiDaiLy = 2,
                    MaQuan = 14,
                    DiaChi = "112 Nguyễn Du",
                    DienThoai = "0934567890",
                    Email = "PhuQuy@gmail.com",
                    NgayTiepNhan = seedDate.AddMonths(-10),
                    TienNo = 9800000L
                }
            );
        }

        private static void SeedMatHang(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatHang>().HasData(
                // 1-3: Original mat hang
                new
                {
                    MaMatHang = 1,
                    TenMatHang = "Gạo",
                    MaDonViTinh = 1,
                    SoLuongTon = 7500
                },
                new
                {
                    MaMatHang = 2,
                    TenMatHang = "Nước ngọt",
                    MaDonViTinh = 5,
                    SoLuongTon = 8500
                },
                new
                {
                    MaMatHang = 3,
                    TenMatHang = "Mì tôm",
                    MaDonViTinh = 3,
                    SoLuongTon = 5500
                },
                // 4-23: Additional mat hang
                new
                {
                    MaMatHang = 4,
                    TenMatHang = "Đường",
                    MaDonViTinh = 1,
                    SoLuongTon = 8500
                },
                new
                {
                    MaMatHang = 5,
                    TenMatHang = "Muối",
                    MaDonViTinh = 1,
                    SoLuongTon = 5500
                },
                new
                {
                    MaMatHang = 6,
                    TenMatHang = "Bột giặt",
                    MaDonViTinh = 3,
                    SoLuongTon = 9500
                },
                new
                {
                    MaMatHang = 7,
                    TenMatHang = "Nước mắm",
                    MaDonViTinh = 5,
                    SoLuongTon = 80000
                },
                new
                {
                    MaMatHang = 8,
                    TenMatHang = "Dầu ăn",
                    MaDonViTinh = 5,
                    SoLuongTon = 8000
                },
                new
                {
                    MaMatHang = 9,
                    TenMatHang = "Sữa tươi",
                    MaDonViTinh = 5,
                    SoLuongTon = 7009
                },
                new
                {
                    MaMatHang = 10,
                    TenMatHang = "Bánh kẹo",
                    MaDonViTinh = 2,
                    SoLuongTon = 6000
                },
                new
                {
                    MaMatHang = 11,
                    TenMatHang = "Nước rửa chén",
                    MaDonViTinh = 5,
                    SoLuongTon =9000
                },
                new
                {
                    MaMatHang = 12,
                    TenMatHang = "Bia",
                    MaDonViTinh = 3,
                    SoLuongTon = 6000
                },
                new
                {
                    MaMatHang = 13,
                    TenMatHang = "Nước suối",
                    MaDonViTinh = 5,
                    SoLuongTon = 7000
                },
                new
                {
                    MaMatHang = 14,
                    TenMatHang = "Bột ngọt",
                    MaDonViTinh = 1,
                    SoLuongTon = 9888
                },
                new
                {
                    MaMatHang = 15,
                    TenMatHang = "Giấy vệ sinh",
                    MaDonViTinh = 3,
                    SoLuongTon = 7600
                },
                new
                {
                    MaMatHang = 16,
                    TenMatHang = "Khăn giấy",
                    MaDonViTinh = 2,
                    SoLuongTon = 8500
                },
                new
                {
                    MaMatHang = 17,
                    TenMatHang = "Nước tương",
                    MaDonViTinh = 5,
                    SoLuongTon = 10000
                },
                new
                {
                    MaMatHang = 18,
                    TenMatHang = "Tương ớt",
                    MaDonViTinh = 5,
                    SoLuongTon = 9000
                },
                new
                {
                    MaMatHang = 19,
                    TenMatHang = "Bột canh",
                    MaDonViTinh = 1,
                    SoLuongTon = 8000
                },
                new
                {
                    MaMatHang = 20,
                    TenMatHang = "Dầu gội",
                    MaDonViTinh = 5,
                    SoLuongTon = 5000
                },
                new
                {
                    MaMatHang = 21,
                    TenMatHang = "Sữa tắm",
                    MaDonViTinh = 5,
                    SoLuongTon = 6000
                },
                new
                {
                    MaMatHang = 22,
                    TenMatHang = "Kem đánh răng",
                    MaDonViTinh = 2,
                    SoLuongTon = 9086
                },
                new
                {
                    MaMatHang = 23,
                    TenMatHang = "Cafe",
                    MaDonViTinh = 1,
                    SoLuongTon = 8005
                },
                // 24-100 : thêm 77 mặt hàng mới
                new
                {
                    MaMatHang = 24,
                    TenMatHang = "Khoai tây",
                    MaDonViTinh = 1,
                    SoLuongTon = 9569
                },
                new
                {
                    MaMatHang = 25,
                    TenMatHang = "Cà rốt",
                    MaDonViTinh = 1,
                    SoLuongTon = 8238
                },
                new
                {
                    MaMatHang = 26,
                    TenMatHang = "Hành tây",
                    MaDonViTinh = 1,
                    SoLuongTon = 7173
                },
                new
                {
                    MaMatHang = 27,
                    TenMatHang = "Ớt chuông",
                    MaDonViTinh = 1,
                    SoLuongTon = 9473
                },
                new
                {
                    MaMatHang = 28,
                    TenMatHang = "Bắp cải",
                    MaDonViTinh = 1,
                    SoLuongTon = 5886
                },
                new
                {
                    MaMatHang = 29,
                    TenMatHang = "Rau muống",
                    MaDonViTinh = 1,
                    SoLuongTon = 8324
                },
                new
                {
                    MaMatHang = 30,
                    TenMatHang = "Măng tây",
                    MaDonViTinh = 1,
                    SoLuongTon = 9391
                },
                new
                {
                    MaMatHang = 31,
                    TenMatHang = "Bông cải xanh",
                    MaDonViTinh = 1,
                    SoLuongTon = 7706
                },
                new
                {
                    MaMatHang = 32,
                    TenMatHang = "Dưa chuột",
                    MaDonViTinh = 1,
                    SoLuongTon = 6424
                },
                new
                {
                    MaMatHang = 33,
                    TenMatHang = "Bí đỏ",
                    MaDonViTinh = 1,
                    SoLuongTon = 7356
                },
                new
                {
                    MaMatHang = 34,
                    TenMatHang = "Cải bó xôi",
                    MaDonViTinh = 1,
                    SoLuongTon = 9912
                },
                new
                {
                    MaMatHang = 35,
                    TenMatHang = "Gừng",
                    MaDonViTinh = 1,
                    SoLuongTon = 5982
                },
                new
                {
                    MaMatHang = 36,
                    TenMatHang = "Tỏi",
                    MaDonViTinh = 1,
                    SoLuongTon = 8970
                },
                new
                {
                    MaMatHang = 37,
                    TenMatHang = "Chanh",
                    MaDonViTinh = 1,
                    SoLuongTon = 7001
                },
                new
                {
                    MaMatHang = 38,
                    TenMatHang = "Dứa",
                    MaDonViTinh = 1,
                    SoLuongTon = 6311
                },
                new
                {
                    MaMatHang = 39,
                    TenMatHang = "Xoài",
                    MaDonViTinh = 1,
                    SoLuongTon = 7998
                },
                new
                {
                    MaMatHang = 40,
                    TenMatHang = "Dưa hấu",
                    MaDonViTinh = 1,
                    SoLuongTon = 6632
                },
                new
                {
                    MaMatHang = 41,
                    TenMatHang = "Đu đủ",
                    MaDonViTinh = 1,
                    SoLuongTon = 7329
                },
                new
                {
                    MaMatHang = 42,
                    TenMatHang = "Dâu tây",
                    MaDonViTinh = 1,
                    SoLuongTon = 6798
                },
                new
                {
                    MaMatHang = 43,
                    TenMatHang = "Việt quất",
                    MaDonViTinh = 1,
                    SoLuongTon = 9622
                },
                new
                {
                    MaMatHang = 44,
                    TenMatHang = "Thịt bò",
                    MaDonViTinh = 1,
                    SoLuongTon = 8092
                },
                new
                {
                    MaMatHang = 45,
                    TenMatHang = "Thịt heo",
                    MaDonViTinh = 1,
                    SoLuongTon = 9508
                },
                new
                {
                    MaMatHang = 46,
                    TenMatHang = "Thịt gà",
                    MaDonViTinh = 1,
                    SoLuongTon = 6439
                },
                new
                {
                    MaMatHang = 47,
                    TenMatHang = "Cá hồi",
                    MaDonViTinh = 1,
                    SoLuongTon = 8422
                },
                new
                {
                    MaMatHang = 48,
                    TenMatHang = "Cá ngừ",
                    MaDonViTinh = 1,
                    SoLuongTon = 6570
                },
                new
                {
                    MaMatHang = 49,
                    TenMatHang = "Tôm",
                    MaDonViTinh = 1,
                    SoLuongTon = 7901
                },
                new
                {
                    MaMatHang = 50,
                    TenMatHang = "Cua",
                    MaDonViTinh = 1,
                    SoLuongTon = 7350
                },
                new
                {
                    MaMatHang = 51,
                    TenMatHang = "Mực",
                    MaDonViTinh = 1,
                    SoLuongTon = 6155
                },
                new
                {
                    MaMatHang = 52,
                    TenMatHang = "Mật ong",
                    MaDonViTinh = 5,
                    SoLuongTon = 6738
                },
                new
                {
                    MaMatHang = 53,
                    TenMatHang = "Nước khoáng",
                    MaDonViTinh = 5,
                    SoLuongTon = 5954
                },
                new
                {
                    MaMatHang = 54,
                    TenMatHang = "Nước tăng lực",
                    MaDonViTinh = 5,
                    SoLuongTon = 7564
                },
                new
                {
                    MaMatHang = 55,
                    TenMatHang = "Nước dừa",
                    MaDonViTinh = 5,
                    SoLuongTon = 8077
                },
                new
                {
                    MaMatHang = 56,
                    TenMatHang = "Nước cam",
                    MaDonViTinh = 4,
                    SoLuongTon = 5269
                },
                new
                {
                    MaMatHang = 57,
                    TenMatHang = "Trà sữa",
                    MaDonViTinh = 5,
                    SoLuongTon = 7489
                },
                new
                {
                    MaMatHang = 58,
                    TenMatHang = "Trà đào",
                    MaDonViTinh = 5,
                    SoLuongTon = 5492
                },
                new
                {
                    MaMatHang = 59,
                    TenMatHang = "Trà chanh",
                    MaDonViTinh = 5,
                    SoLuongTon = 5928
                },
                new
                {
                    MaMatHang = 60,
                    TenMatHang = "Nước ép cà rốt",
                    MaDonViTinh = 4,
                    SoLuongTon = 9939
                },
                new
                {
                    MaMatHang = 61,
                    TenMatHang = "Nước ép dưa hấu",
                    MaDonViTinh = 4,
                    SoLuongTon = 8384
                },
                new
                {
                    MaMatHang = 62,
                    TenMatHang = "Bia không cồn",
                    MaDonViTinh = 5,
                    SoLuongTon = 9562
                },
                new
                {
                    MaMatHang = 63,
                    TenMatHang = "Bia lon",
                    MaDonViTinh = 5,
                    SoLuongTon = 9384
                },
                new
                {
                    MaMatHang = 64,
                    TenMatHang = "Bia chai lớn",
                    MaDonViTinh = 5,
                    SoLuongTon = 8224
                },
                new
                {
                    MaMatHang = 65,
                    TenMatHang = "Rượu vang đỏ",
                    MaDonViTinh = 5,
                    SoLuongTon = 9839
                },
                new
                {
                    MaMatHang = 66,
                    TenMatHang = "Rượu vang trắng",
                    MaDonViTinh = 5,
                    SoLuongTon = 5136
                },
                new
                {
                    MaMatHang = 67,
                    TenMatHang = "Rượu gạo",
                    MaDonViTinh = 5,
                    SoLuongTon = 9810
                },
                new
                {
                    MaMatHang = 68,
                    TenMatHang = "Rượu trắng",
                    MaDonViTinh = 5,
                    SoLuongTon = 6367
                },
                new
                {
                    MaMatHang = 69,
                    TenMatHang = "Rượu nếp",
                    MaDonViTinh = 5,
                    SoLuongTon = 7126
                },
                new
                {
                    MaMatHang = 70,
                    TenMatHang = "Dầu ô-liu",
                    MaDonViTinh = 5,
                    SoLuongTon = 9546
                },
                new
                {
                    MaMatHang = 71,
                    TenMatHang = "Dầu mè",
                    MaDonViTinh = 5,
                    SoLuongTon = 8149
                },
                new
                {
                    MaMatHang = 72,
                    TenMatHang = "Dầu hướng dương",
                    MaDonViTinh = 5,
                    SoLuongTon = 9092
                },
                new
                {
                    MaMatHang = 73,
                    TenMatHang = "Giấm táo",
                    MaDonViTinh = 5,
                    SoLuongTon = 7324
                },
                new
                {
                    MaMatHang = 74,
                    TenMatHang = "Giấm gạo",
                    MaDonViTinh = 5,
                    SoLuongTon = 8977
                },
                new
                {
                    MaMatHang = 75,
                    TenMatHang = "Cà chua",
                    MaDonViTinh = 1,
                    SoLuongTon = 6463
                },
                new
                {
                    MaMatHang = 76,
                    TenMatHang = "Ớt cay",
                    MaDonViTinh = 1,
                    SoLuongTon = 8854
                },
                new
                {
                    MaMatHang = 77,
                    TenMatHang = "Hạt điều",
                    MaDonViTinh = 1,
                    SoLuongTon = 9781
                },
                new
                {
                    MaMatHang = 78,
                    TenMatHang = "Hạt bí",
                    MaDonViTinh = 1,
                    SoLuongTon = 9938
                },
                new
                {
                    MaMatHang = 79,
                    TenMatHang = "Hạt dẻ",
                    MaDonViTinh = 1,
                    SoLuongTon = 5281
                },
                new
                {
                    MaMatHang = 80,
                    TenMatHang = "Ngô ngọt",
                    MaDonViTinh = 1,
                    SoLuongTon = 8512
                },
                new
                {
                    MaMatHang = 81,
                    TenMatHang = "Bột sắn",
                    MaDonViTinh = 1,
                    SoLuongTon = 5986
                },
                new
                {
                    MaMatHang = 82,
                    TenMatHang = "Tương đậu nành",
                    MaDonViTinh = 5,
                    SoLuongTon = 5275
                },
                new
                {
                    MaMatHang = 83,
                    TenMatHang = "Nước súc miệng",
                    MaDonViTinh = 5,
                    SoLuongTon = 6450
                },
                new
                {
                    MaMatHang = 84,
                    TenMatHang = "Kem dưỡng da",
                    MaDonViTinh = 5,
                    SoLuongTon = 8428
                },
                new
                {
                    MaMatHang = 85,
                    TenMatHang = "Bánh đa",
                    MaDonViTinh = 2,
                    SoLuongTon = 6025
                },
                new
                {
                    MaMatHang = 86,
                    TenMatHang = "Bánh phở",
                    MaDonViTinh = 2,
                    SoLuongTon = 6714
                },
                new
                {
                    MaMatHang = 87,
                    TenMatHang = "Bắp rang",
                    MaDonViTinh = 2,
                    SoLuongTon = 9699
                },
                new
                {
                    MaMatHang = 88,
                    TenMatHang = "Bột chiên xù",
                    MaDonViTinh = 1,
                    SoLuongTon = 9867
                },
                new
                {
                    MaMatHang = 89,
                    TenMatHang = "Sốt BBQ",
                    MaDonViTinh = 5,
                    SoLuongTon = 6308
                },
                new
                {
                    MaMatHang = 90,
                    TenMatHang = "Tỏi băm",
                    MaDonViTinh = 1,
                    SoLuongTon = 8923
                },
                new
                {
                    MaMatHang = 91,
                    TenMatHang = "Hành phi",
                    MaDonViTinh = 1,
                    SoLuongTon = 6811
                },
                new
                {
                    MaMatHang = 92,
                    TenMatHang = "Muối tiêu chanh",
                    MaDonViTinh = 1,
                    SoLuongTon = 5264
                },
                new
                {
                    MaMatHang = 93,
                    TenMatHang = "Gia vị nấu phở",
                    MaDonViTinh = 1,
                    SoLuongTon = 9104
                },
                new
                {
                    MaMatHang = 94,
                    TenMatHang = "Dầu đậu nành",
                    MaDonViTinh = 5,
                    SoLuongTon = 7958
                },
                new
                {
                    MaMatHang = 95,
                    TenMatHang = "Xúc xích Đức",
                    MaDonViTinh = 2,
                    SoLuongTon = 6935
                },
                new
                {
                    MaMatHang = 96,
                    TenMatHang = "Thịt xông khói",
                    MaDonViTinh = 2,
                    SoLuongTon = 7513
                },
                new
                {
                    MaMatHang = 97,
                    TenMatHang = "Phô mai que",
                    MaDonViTinh = 2,
                    SoLuongTon = 9917
                },
                new
                {
                    MaMatHang = 98,
                    TenMatHang = "Bánh quy mặn",
                    MaDonViTinh = 2,
                    SoLuongTon = 9349
                },
                new
                {
                    MaMatHang = 99,
                    TenMatHang = "Bánh quy ngọt",
                    MaDonViTinh = 2,
                    SoLuongTon = 6794
                },
                new
                {
                    MaMatHang = 100,
                    TenMatHang = "Bánh pía",
                    MaDonViTinh = 2,
                    SoLuongTon = 7160
                }

            );
        }

        private static void SeedPhieuXuat(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2023, 1, 1);

            modelBuilder.Entity<PhieuXuat>().HasData(
                //Đại lý 1
                new PhieuXuat { MaPhieuXuat = 1, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(0), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 2, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(2), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 3, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(4), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 4, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(6), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 5, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(8), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 6, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(10), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 7, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(12), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 8, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(14), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 9, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(16), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 10, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(18), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 11, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(20), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 12, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(22), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 13, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(24), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 14, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(26), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 15, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(28), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 16, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(30), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 17, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(32), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 18, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(34), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 19, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(36), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 20, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(38), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 21, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(40), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 22, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(42), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 23, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(44), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 24, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(46), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 25, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(48), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 26, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(50), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 27, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(52), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 28, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(54), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 29, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(56), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 30, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(58), TongTriGia = 500000 },
                // ------------------------ PHIẾU XUẤT – ĐẠI LÝ 2 ------------------------
                new PhieuXuat { MaPhieuXuat = 31, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(1), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 32, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(3), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 33, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(5), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 34, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(7), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 35, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(9), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 36, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(11), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 37, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(13), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 38, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(15), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 39, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(17), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 40, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(19), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 41, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(21), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 42, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(23), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 43, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(25), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 44, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(27), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 45, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(29), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 46, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(31), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 47, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(33), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 48, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(35), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 49, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(37), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 50, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(39), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 51, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(41), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 52, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(43), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 53, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(45), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 54, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(47), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 55, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(49), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 56, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(51), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 57, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(53), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 58, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(55), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 59, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(57), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 60, MaDaiLy = 2, NgayLapPhieu = seedDate.AddMonths(59), TongTriGia = 500000 },
                // ------------------------ PHIẾU XUẤT – ĐẠI LÝ 3 ------------------------
                new PhieuXuat { MaPhieuXuat = 61, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(2), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 62, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(4), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 63, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(6), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 64, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(8), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 65, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(10), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 66, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(12), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 67, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(14), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 68, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(16), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 69, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(18), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 70, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(20), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 71, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(22), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 72, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(24), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 73, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(26), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 74, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(28), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 75, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(30), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 76, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(32), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 77, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(34), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 78, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(36), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 79, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(38), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 80, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(40), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 81, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(42), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 82, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(44), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 83, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(46), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 84, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(48), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 85, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(50), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 86, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(52), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 87, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(54), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 88, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(56), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 89, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(58), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 90, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(60), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 91, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(62), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 92, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(64), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 93, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(66), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 94, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(68), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 95, MaDaiLy = 3, NgayLapPhieu = seedDate.AddMonths(70), TongTriGia = 500000 },
                // ------------------------ PHIẾU XUẤT – ĐẠI LÝ 4 ------------------------
                new PhieuXuat { MaPhieuXuat = 96, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(2), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 97, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(4), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 98, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(6), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 99, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(8), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 100, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(10), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 101, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(12), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 102, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(14), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 103, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(16), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 104, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(18), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 105, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(20), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 106, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(22), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 107, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(24), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 108, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(26), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 109, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(28), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 110, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(30), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 111, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(32), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 112, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(34), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 113, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(36), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 114, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(38), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 115, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(40), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 116, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(42), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 117, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(44), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 118, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(46), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 119, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(48), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 120, MaDaiLy = 4, NgayLapPhieu = seedDate.AddMonths(50), TongTriGia = 500000 },
                // ------------------------ PHIẾU XUẤT – ĐẠI LÝ 5 ------------------------
                new PhieuXuat { MaPhieuXuat = 121, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(2), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 122, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(4), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 123, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(6), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 124, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(8), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 125, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(10), TongTriGia = 500000 },
                
                new PhieuXuat { MaPhieuXuat = 126, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(12), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 127, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(14), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 128, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(16), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 129, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(18), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 130, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(20), TongTriGia = 500000 },
                
                // … (lặp lại chu kỳ 5 giá trị trên đến PX 150 – cách 2 tháng)
                new PhieuXuat { MaPhieuXuat = 131, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(22), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 132, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(24), TongTriGia = 2000000 },
                new PhieuXuat { MaPhieuXuat = 133, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(26), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 134, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(28), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 135, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(30), TongTriGia = 500000 },
                
                // … tiếp tục đến:
                new PhieuXuat { MaPhieuXuat = 146, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(46), TongTriGia = 1500000 },
                new PhieuXuat { MaPhieuXuat = 147, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(48), TongTriGia = 1000000 },
                new PhieuXuat { MaPhieuXuat = 148, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(50), TongTriGia = 500000 },
                new PhieuXuat { MaPhieuXuat = 149, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(52), TongTriGia = 2500000 },
                new PhieuXuat { MaPhieuXuat = 150, MaDaiLy = 5, NgayLapPhieu = seedDate.AddMonths(54), TongTriGia = 2000000 }
            );
        }

        private static void SeedChiTietPhieuXuat(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietPhieuXuat>().HasData(
                // --- Chi tiết Phiếu Xuất 1 (tổng 2 500 000 – 5 CT) ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 1, MaPhieuXuat = 1, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 2, MaPhieuXuat = 1, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 3, MaPhieuXuat = 1, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 4, MaPhieuXuat = 1, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 5, MaPhieuXuat = 1, MaMatHang = 5, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 2 (tổng 2 000 000 – 4 CT) ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 6, MaPhieuXuat = 2, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 7, MaPhieuXuat = 2, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 8, MaPhieuXuat = 2, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 9, MaPhieuXuat = 2, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 3 (tổng 1 500 000 – 3 CT) ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 10, MaPhieuXuat = 3, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 11, MaPhieuXuat = 3, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 12, MaPhieuXuat = 3, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 4 (tổng 1 000 000 – 2 CT) ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 13, MaPhieuXuat = 4, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 14, MaPhieuXuat = 4, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 5 (tổng 500 000 – 1 CT) ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 15, MaPhieuXuat = 5, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 6 (tổng 2 500 000 – 5 CT) ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 16, MaPhieuXuat = 6, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 17, MaPhieuXuat = 6, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 18, MaPhieuXuat = 6, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 19, MaPhieuXuat = 6, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 20, MaPhieuXuat = 6, MaMatHang = 5, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 7 (tổng 2 000 000 – 4 CT) ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 21, MaPhieuXuat = 7, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 22, MaPhieuXuat = 7, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 23, MaPhieuXuat = 7, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 24, MaPhieuXuat = 7, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 8 (tổng 1 500 000 – 3 CT) ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 25, MaPhieuXuat = 8, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 26, MaPhieuXuat = 8, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 27, MaPhieuXuat = 8, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 9 (tổng 1 000 000 – 2 CT) ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 28, MaPhieuXuat = 9, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 29, MaPhieuXuat = 9, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 10 (tổng 500 000 – 1 CT) ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 30, MaPhieuXuat = 10, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 11 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 31, MaPhieuXuat = 11, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 32, MaPhieuXuat = 11, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 33, MaPhieuXuat = 11, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 34, MaPhieuXuat = 11, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 35, MaPhieuXuat = 11, MaMatHang = 5, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 12 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 36, MaPhieuXuat = 12, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 37, MaPhieuXuat = 12, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 38, MaPhieuXuat = 12, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 39, MaPhieuXuat = 12, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 13 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 40, MaPhieuXuat = 13, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 41, MaPhieuXuat = 13, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 42, MaPhieuXuat = 13, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 14 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 43, MaPhieuXuat = 14, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 44, MaPhieuXuat = 14, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 15 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 45, MaPhieuXuat = 15, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 16 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 46, MaPhieuXuat = 16, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 47, MaPhieuXuat = 16, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 48, MaPhieuXuat = 16, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 49, MaPhieuXuat = 16, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 50, MaPhieuXuat = 16, MaMatHang = 5, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 17 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 51, MaPhieuXuat = 17, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 52, MaPhieuXuat = 17, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 53, MaPhieuXuat = 17, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 54, MaPhieuXuat = 17, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 18 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 55, MaPhieuXuat = 18, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 56, MaPhieuXuat = 18, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 57, MaPhieuXuat = 18, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 19 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 58, MaPhieuXuat = 19, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 59, MaPhieuXuat = 19, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 20 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 60, MaPhieuXuat = 20, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 21 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 61, MaPhieuXuat = 21, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 62, MaPhieuXuat = 21, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 63, MaPhieuXuat = 21, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 64, MaPhieuXuat = 21, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 65, MaPhieuXuat = 21, MaMatHang = 5, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 22 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 66, MaPhieuXuat = 22, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 67, MaPhieuXuat = 22, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 68, MaPhieuXuat = 22, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 69, MaPhieuXuat = 22, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 23 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 70, MaPhieuXuat = 23, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 71, MaPhieuXuat = 23, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 72, MaPhieuXuat = 23, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 24 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 73, MaPhieuXuat = 24, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 74, MaPhieuXuat = 24, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 25 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 75, MaPhieuXuat = 25, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 26 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 76, MaPhieuXuat = 26, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 77, MaPhieuXuat = 26, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 78, MaPhieuXuat = 26, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 79, MaPhieuXuat = 26, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 80, MaPhieuXuat = 26, MaMatHang = 5, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 27 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 81, MaPhieuXuat = 27, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 82, MaPhieuXuat = 27, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 83, MaPhieuXuat = 27, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 84, MaPhieuXuat = 27, MaMatHang = 4, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 28 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 85, MaPhieuXuat = 28, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 86, MaPhieuXuat = 28, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 87, MaPhieuXuat = 28, MaMatHang = 3, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 29 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 88, MaPhieuXuat = 29, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 89, MaPhieuXuat = 29, MaMatHang = 2, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // --- Chi tiết Phiếu Xuất 30 ---
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 90, MaPhieuXuat = 30, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },

                // -------- CT PX 31 (Tong 2,500,000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 91, MaPhieuXuat = 31, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 92, MaPhieuXuat = 31, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 93, MaPhieuXuat = 31, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 94, MaPhieuXuat = 31, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 95, MaPhieuXuat = 31, MaMatHang = 10, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                // -------- CT PX 32 (Tong 2,000,000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 96, MaPhieuXuat = 32, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 97, MaPhieuXuat = 32, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 98, MaPhieuXuat = 32, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 99, MaPhieuXuat = 32, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                // -------- CT PX 33 (Tong 1,500,000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 100, MaPhieuXuat = 33, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 101, MaPhieuXuat = 33, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 102, MaPhieuXuat = 33, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                // -------- CT PX 34 (Tong 1,000,000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 103, MaPhieuXuat = 34, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 104, MaPhieuXuat = 34, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                // -------- CT PX 35 (Tong 500,000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 105, MaPhieuXuat = 35, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                // -------- CT PX 36 (Tong 2,500,000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 106, MaPhieuXuat = 36, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 107, MaPhieuXuat = 36, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 108, MaPhieuXuat = 36, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 109, MaPhieuXuat = 36, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 110, MaPhieuXuat = 36, MaMatHang = 10, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                // -------- CT PX 37 (Tong 2,000,000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 111, MaPhieuXuat = 37, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 112, MaPhieuXuat = 37, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 113, MaPhieuXuat = 37, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 114, MaPhieuXuat = 37, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                // -------- CT PX 38 (Tong 1,000,000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 115, MaPhieuXuat = 38, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 116, MaPhieuXuat = 38, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                // -------- CT PX 39 (Tong 500,000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 117, MaPhieuXuat = 39, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                // -------- CT PX 40 (Tong 500,000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 118, MaPhieuXuat = 40, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                // -------- CT PX 41 (Tong 2,500,000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 119, MaPhieuXuat = 41, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 120, MaPhieuXuat = 41, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 121, MaPhieuXuat = 41, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 122, MaPhieuXuat = 41, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 123, MaPhieuXuat = 41, MaMatHang = 10, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                // -------- CT PX 42 (Tong 2,000,000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 124, MaPhieuXuat = 42, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 125, MaPhieuXuat = 42, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 126, MaPhieuXuat = 42, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 127, MaPhieuXuat = 42, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                // -------- CT PX 43 (Tong 1,500,000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 128, MaPhieuXuat = 43, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 129, MaPhieuXuat = 43, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 130, MaPhieuXuat = 43, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                // -------- CT PX 44 (Tong 1,000,000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 131, MaPhieuXuat = 44, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 132, MaPhieuXuat = 44, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                // -------- CT PX 45 (Tong 500,000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 133, MaPhieuXuat = 45, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                // -------- CT PX 46 (Tong 2,500,000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 134, MaPhieuXuat = 46, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 135, MaPhieuXuat = 46, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 136, MaPhieuXuat = 46, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 137, MaPhieuXuat = 46, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 138, MaPhieuXuat = 46, MaMatHang = 10, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                // -------- CT PX 47 (Tong 2,000,000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 139, MaPhieuXuat = 47, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 140, MaPhieuXuat = 47, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 141, MaPhieuXuat = 47, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 142, MaPhieuXuat = 47, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                // -------- CT PX 48 (Tong 1,000,000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 143, MaPhieuXuat = 48, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 144, MaPhieuXuat = 48, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                // -------- CT PX 49 (Tong 500,000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 145, MaPhieuXuat = 49, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                // -------- CT PX 50 (Tong 500,000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 146, MaPhieuXuat = 50, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                // -------- CT PX 51 (Tong 2,500,000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 147, MaPhieuXuat = 51, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 148, MaPhieuXuat = 51, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 149, MaPhieuXuat = 51, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 150, MaPhieuXuat = 51, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 151, MaPhieuXuat = 51, MaMatHang = 10, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                // -------- CT PX 52 (Tong 2,000,000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 152, MaPhieuXuat = 52, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 153, MaPhieuXuat = 52, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 154, MaPhieuXuat = 52, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 155, MaPhieuXuat = 52, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                // -------- CT PX 53 (Tong 1,500,000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 156, MaPhieuXuat = 53, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 157, MaPhieuXuat = 53, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 158, MaPhieuXuat = 53, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                // -------- CT PX 54 (Tong 1,000,000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 159, MaPhieuXuat = 54, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 160, MaPhieuXuat = 54, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                // -------- CT PX 55 (Tong 500,000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 161, MaPhieuXuat = 55, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                // -------- CT PX 56 (Tong 2,500,000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 162, MaPhieuXuat = 56, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 163, MaPhieuXuat = 56, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 164, MaPhieuXuat = 56, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 165, MaPhieuXuat = 56, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 166, MaPhieuXuat = 56, MaMatHang = 10, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                // -------- CT PX 57 (Tong 2,000,000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 167, MaPhieuXuat = 57, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 168, MaPhieuXuat = 57, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 169, MaPhieuXuat = 57, MaMatHang = 8, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 170, MaPhieuXuat = 57, MaMatHang = 9, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                // -------- CT PX 58 (Tong 1,000,000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 171, MaPhieuXuat = 58, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 172, MaPhieuXuat = 58, MaMatHang = 7, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                // -------- CT PX 59 (Tong 500,000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 173, MaPhieuXuat = 59, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                // -------- CT PX 60 (Tong 500,000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 174, MaPhieuXuat = 60, MaMatHang = 6, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                // ===================== CHI TIẾT PHIẾU XUẤT – ĐẠI LÝ 3 (PX 61 → 95) =====================
                // Mặt hàng 11-15, mỗi CT = 500 000 ₫, ID 175-279
                // ---------------------------------------------------------------------------------------
                
                // -------- CT PX 61 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 175, MaPhieuXuat = 61, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 176, MaPhieuXuat = 61, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 177, MaPhieuXuat = 61, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 178, MaPhieuXuat = 61, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 179, MaPhieuXuat = 61, MaMatHang = 15, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 62 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 180, MaPhieuXuat = 62, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 181, MaPhieuXuat = 62, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 182, MaPhieuXuat = 62, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 183, MaPhieuXuat = 62, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 63 (Tổng 1 500 000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 184, MaPhieuXuat = 63, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 185, MaPhieuXuat = 63, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 186, MaPhieuXuat = 63, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // -------- CT PX 64 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 187, MaPhieuXuat = 64, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 188, MaPhieuXuat = 64, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 65 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 189, MaPhieuXuat = 65, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 66 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 190, MaPhieuXuat = 66, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 191, MaPhieuXuat = 66, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 192, MaPhieuXuat = 66, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 193, MaPhieuXuat = 66, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 194, MaPhieuXuat = 66, MaMatHang = 15, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 67 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 195, MaPhieuXuat = 67, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 196, MaPhieuXuat = 67, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 197, MaPhieuXuat = 67, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 198, MaPhieuXuat = 67, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 68 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 199, MaPhieuXuat = 68, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 200, MaPhieuXuat = 68, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 69 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 201, MaPhieuXuat = 69, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 70 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 202, MaPhieuXuat = 70, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 71 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 203, MaPhieuXuat = 71, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 204, MaPhieuXuat = 71, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 205, MaPhieuXuat = 71, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 206, MaPhieuXuat = 71, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 207, MaPhieuXuat = 71, MaMatHang = 15, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 72 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 208, MaPhieuXuat = 72, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 209, MaPhieuXuat = 72, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 210, MaPhieuXuat = 72, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 211, MaPhieuXuat = 72, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 73 (Tổng 1 500 000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 212, MaPhieuXuat = 73, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 213, MaPhieuXuat = 73, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 214, MaPhieuXuat = 73, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // -------- CT PX 74 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 215, MaPhieuXuat = 74, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 216, MaPhieuXuat = 74, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 75 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 217, MaPhieuXuat = 75, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 76 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 218, MaPhieuXuat = 76, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 219, MaPhieuXuat = 76, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 220, MaPhieuXuat = 76, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 221, MaPhieuXuat = 76, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 222, MaPhieuXuat = 76, MaMatHang = 15, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 77 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 223, MaPhieuXuat = 77, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 224, MaPhieuXuat = 77, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 225, MaPhieuXuat = 77, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 226, MaPhieuXuat = 77, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 78 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 227, MaPhieuXuat = 78, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 228, MaPhieuXuat = 78, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 79 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 229, MaPhieuXuat = 79, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 80 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 230, MaPhieuXuat = 80, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 81 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 231, MaPhieuXuat = 81, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 232, MaPhieuXuat = 81, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 233, MaPhieuXuat = 81, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 234, MaPhieuXuat = 81, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 235, MaPhieuXuat = 81, MaMatHang = 15, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 82 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 236, MaPhieuXuat = 82, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 237, MaPhieuXuat = 82, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 238, MaPhieuXuat = 82, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 239, MaPhieuXuat = 82, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 83 (Tổng 1 500 000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 240, MaPhieuXuat = 83, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 241, MaPhieuXuat = 83, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 242, MaPhieuXuat = 83, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // -------- CT PX 84 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 243, MaPhieuXuat = 84, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 244, MaPhieuXuat = 84, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 85 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 245, MaPhieuXuat = 85, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 86 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 246, MaPhieuXuat = 86, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 247, MaPhieuXuat = 86, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 248, MaPhieuXuat = 86, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 249, MaPhieuXuat = 86, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 250, MaPhieuXuat = 86, MaMatHang = 15, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 87 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 251, MaPhieuXuat = 87, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 252, MaPhieuXuat = 87, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 253, MaPhieuXuat = 87, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 254, MaPhieuXuat = 87, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 88 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 255, MaPhieuXuat = 88, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 256, MaPhieuXuat = 88, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 89 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 257, MaPhieuXuat = 89, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 90 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 258, MaPhieuXuat = 90, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 91 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 259, MaPhieuXuat = 91, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 260, MaPhieuXuat = 91, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 261, MaPhieuXuat = 91, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 262, MaPhieuXuat = 91, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 263, MaPhieuXuat = 91, MaMatHang = 15, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 92 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 264, MaPhieuXuat = 92, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 265, MaPhieuXuat = 92, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 266, MaPhieuXuat = 92, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 267, MaPhieuXuat = 92, MaMatHang = 14, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 93 (Tổng 1 500 000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 268, MaPhieuXuat = 93, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 269, MaPhieuXuat = 93, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 270, MaPhieuXuat = 93, MaMatHang = 13, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // -------- CT PX 94 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 271, MaPhieuXuat = 94, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 272, MaPhieuXuat = 94, MaMatHang = 12, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 95 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 273, MaPhieuXuat = 95, MaMatHang = 11, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                // ================== CHI TIẾT PHIẾU XUẤT – ĐẠI LÝ 4 (PX 101 → 120) ==================
                // Mặt hàng 16 → 20 - mỗi CT = 500 000 ₫.  ID liên tục 295 → 354
                // ------------------------------------------------------------------------------------
                
                // -------- CT PX 101 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 295, MaPhieuXuat = 101, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 296, MaPhieuXuat = 101, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 297, MaPhieuXuat = 101, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 298, MaPhieuXuat = 101, MaMatHang = 19, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 299, MaPhieuXuat = 101, MaMatHang = 20, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 102 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 300, MaPhieuXuat = 102, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 301, MaPhieuXuat = 102, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 302, MaPhieuXuat = 102, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 303, MaPhieuXuat = 102, MaMatHang = 19, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 103 (Tổng 1 500 000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 304, MaPhieuXuat = 103, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 305, MaPhieuXuat = 103, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 306, MaPhieuXuat = 103, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // -------- CT PX 104 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 307, MaPhieuXuat = 104, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 308, MaPhieuXuat = 104, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 105 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 309, MaPhieuXuat = 105, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 106 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 310, MaPhieuXuat = 106, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 311, MaPhieuXuat = 106, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 312, MaPhieuXuat = 106, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 313, MaPhieuXuat = 106, MaMatHang = 19, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 314, MaPhieuXuat = 106, MaMatHang = 20, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 107 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 315, MaPhieuXuat = 107, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 316, MaPhieuXuat = 107, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 317, MaPhieuXuat = 107, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 318, MaPhieuXuat = 107, MaMatHang = 19, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 108 (Tổng 1 500 000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 319, MaPhieuXuat = 108, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 320, MaPhieuXuat = 108, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 321, MaPhieuXuat = 108, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // -------- CT PX 109 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 322, MaPhieuXuat = 109, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 323, MaPhieuXuat = 109, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 110 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 324, MaPhieuXuat = 110, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 111 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 325, MaPhieuXuat = 111, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 326, MaPhieuXuat = 111, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 327, MaPhieuXuat = 111, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 328, MaPhieuXuat = 111, MaMatHang = 19, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 329, MaPhieuXuat = 111, MaMatHang = 20, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 112 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 330, MaPhieuXuat = 112, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 331, MaPhieuXuat = 112, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 332, MaPhieuXuat = 112, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 333, MaPhieuXuat = 112, MaMatHang = 19, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 113 (Tổng 1 500 000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 334, MaPhieuXuat = 113, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 335, MaPhieuXuat = 113, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 336, MaPhieuXuat = 113, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // -------- CT PX 114 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 337, MaPhieuXuat = 114, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 338, MaPhieuXuat = 114, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 115 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 339, MaPhieuXuat = 115, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 116 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 340, MaPhieuXuat = 116, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 341, MaPhieuXuat = 116, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 342, MaPhieuXuat = 116, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 343, MaPhieuXuat = 116, MaMatHang = 19, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 344, MaPhieuXuat = 116, MaMatHang = 20, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 117 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 345, MaPhieuXuat = 117, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 346, MaPhieuXuat = 117, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 347, MaPhieuXuat = 117, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 348, MaPhieuXuat = 117, MaMatHang = 19, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 118 (Tổng 1 500 000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 349, MaPhieuXuat = 118, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 350, MaPhieuXuat = 118, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 351, MaPhieuXuat = 118, MaMatHang = 18, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // -------- CT PX 119 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 352, MaPhieuXuat = 119, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 353, MaPhieuXuat = 119, MaMatHang = 17, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 120 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 354, MaPhieuXuat = 120, MaMatHang = 16, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },

                // ================= CHI TIẾT PHIẾU XUẤT – ĐẠI LÝ 5 (PX 121 → 150) =================
                // Mặt hàng 21 → 25 – mỗi chi tiết 500 000 ₫.  ID liên tục 355 → 504.
                // ---------------------------------------------------------------------------------

                // -------- CT PX 121 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 355, MaPhieuXuat = 121, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 356, MaPhieuXuat = 121, MaMatHang = 22, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 357, MaPhieuXuat = 121, MaMatHang = 23, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 358, MaPhieuXuat = 121, MaMatHang = 24, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 359, MaPhieuXuat = 121, MaMatHang = 25, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 122 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 360, MaPhieuXuat = 122, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 361, MaPhieuXuat = 122, MaMatHang = 22, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 362, MaPhieuXuat = 122, MaMatHang = 23, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 363, MaPhieuXuat = 122, MaMatHang = 24, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 123 (Tổng 1 500 000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 364, MaPhieuXuat = 123, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 365, MaPhieuXuat = 123, MaMatHang = 22, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 366, MaPhieuXuat = 123, MaMatHang = 23, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // -------- CT PX 124 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 367, MaPhieuXuat = 124, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 368, MaPhieuXuat = 124, MaMatHang = 22, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 125 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 369, MaPhieuXuat = 125, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 126 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 370, MaPhieuXuat = 126, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 371, MaPhieuXuat = 126, MaMatHang = 22, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 372, MaPhieuXuat = 126, MaMatHang = 23, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 373, MaPhieuXuat = 126, MaMatHang = 24, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 374, MaPhieuXuat = 126, MaMatHang = 25, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 },
                
                // -------- CT PX 127 (Tổng 2 000 000 – 4 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 375, MaPhieuXuat = 127, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 376, MaPhieuXuat = 127, MaMatHang = 22, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 377, MaPhieuXuat = 127, MaMatHang = 23, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 378, MaPhieuXuat = 127, MaMatHang = 24, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                
                // -------- CT PX 128 (Tổng 1 500 000 – 3 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 379, MaPhieuXuat = 128, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 380, MaPhieuXuat = 128, MaMatHang = 22, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 381, MaPhieuXuat = 128, MaMatHang = 23, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                
                // -------- CT PX 129 (Tổng 1 000 000 – 2 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 382, MaPhieuXuat = 129, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 383, MaPhieuXuat = 129, MaMatHang = 22, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                
                // -------- CT PX 130 (Tổng 500 000 – 1 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 384, MaPhieuXuat = 130, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                
                // -------- CT PX 131 (Tổng 2 500 000 – 5 CT) --------
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 385, MaPhieuXuat = 131, MaMatHang = 21, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 386, MaPhieuXuat = 131, MaMatHang = 22, SoLuongXuat = 20, DonGia = 25000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 387, MaPhieuXuat = 131, MaMatHang = 23, SoLuongXuat = 5, DonGia = 100000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 388, MaPhieuXuat = 131, MaMatHang = 24, SoLuongXuat = 25, DonGia = 20000, ThanhTien = 500000 },
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 389, MaPhieuXuat = 131, MaMatHang = 25, SoLuongXuat = 50, DonGia = 10000, ThanhTien = 500000 }
               
            );
        }

        private static void SeedPhieuThu(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2023, 1, 1);

            modelBuilder.Entity<PhieuThu>().HasData(
                // ------------------------ PHIẾU THU – ĐẠI LÝ 1 ------------------------
                new PhieuThu { MaPhieuThu = 1, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(4), SoTienThu = 2000000 },
                new PhieuThu { MaPhieuThu = 2, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(8), SoTienThu = 1500000 },
                new PhieuThu { MaPhieuThu = 3, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(12), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 4, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(16), SoTienThu = 1400000 },
                new PhieuThu { MaPhieuThu = 5, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(20), SoTienThu = 2200000 },
                new PhieuThu { MaPhieuThu = 6, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(24), SoTienThu = 1900000 },
                new PhieuThu { MaPhieuThu = 7, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(28), SoTienThu = 1300000 },
                new PhieuThu { MaPhieuThu = 8, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(32), SoTienThu = 2100000 },
                new PhieuThu { MaPhieuThu = 9, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(36), SoTienThu = 1700000 },
                new PhieuThu { MaPhieuThu = 10, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(40), SoTienThu = 1600000 },
                new PhieuThu { MaPhieuThu = 11, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(44), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 12, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(48), SoTienThu = 1900000 },
                new PhieuThu { MaPhieuThu = 13, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(52), SoTienThu = 2000000 },
                new PhieuThu { MaPhieuThu = 14, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(56), SoTienThu = 2100000 },
                new PhieuThu { MaPhieuThu = 15, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(60), SoTienThu = 1700000 },
                // ------------------------ PHIẾU THU – ĐẠI LÝ 2 ------------------------
                new PhieuThu { MaPhieuThu = 16, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(5), SoTienThu = 1600000 },
                new PhieuThu { MaPhieuThu = 17, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(9), SoTienThu = 1400000 },
                new PhieuThu { MaPhieuThu = 18, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(13), SoTienThu = 1500000 },
                new PhieuThu { MaPhieuThu = 19, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(17), SoTienThu = 1300000 },
                new PhieuThu { MaPhieuThu = 20, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(21), SoTienThu = 1700000 },
                new PhieuThu { MaPhieuThu = 21, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(25), SoTienThu = 1200000 },
                new PhieuThu { MaPhieuThu = 22, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(29), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 23, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(33), SoTienThu = 1500000 },
                new PhieuThu { MaPhieuThu = 24, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(37), SoTienThu = 1400000 },
                new PhieuThu { MaPhieuThu = 25, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(41), SoTienThu = 1000000 },
                new PhieuThu { MaPhieuThu = 26, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(45), SoTienThu = 1300000 },
                new PhieuThu { MaPhieuThu = 27, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(49), SoTienThu = 1700000 },
                new PhieuThu { MaPhieuThu = 28, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(53), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 29, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(57), SoTienThu = 1200000 },
                new PhieuThu { MaPhieuThu = 30, MaDaiLy = 2, NgayThuTien = seedDate.AddMonths(61), SoTienThu = 1600000 },
                // ------------------------ PHIẾU THU – ĐẠI LÝ 3 ------------------------
                new PhieuThu { MaPhieuThu = 31, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(6), SoTienThu = 2000000 },
                new PhieuThu { MaPhieuThu = 32, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(10), SoTienThu = 1500000 },
                new PhieuThu { MaPhieuThu = 33, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(14), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 34, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(18), SoTienThu = 1600000 },
                new PhieuThu { MaPhieuThu = 35, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(22), SoTienThu = 1900000 },
                
                new PhieuThu { MaPhieuThu = 36, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(26), SoTienThu = 1700000 },
                new PhieuThu { MaPhieuThu = 37, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(30), SoTienThu = 2200000 },
                new PhieuThu { MaPhieuThu = 38, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(34), SoTienThu = 1500000 },
                new PhieuThu { MaPhieuThu = 39, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(38), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 40, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(42), SoTienThu = 1700000 },
                
                new PhieuThu { MaPhieuThu = 41, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(46), SoTienThu = 1600000 },
                new PhieuThu { MaPhieuThu = 42, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(50), SoTienThu = 2000000 },
                new PhieuThu { MaPhieuThu = 43, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(54), SoTienThu = 1900000 },
                new PhieuThu { MaPhieuThu = 44, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(58), SoTienThu = 1600000 },
                new PhieuThu { MaPhieuThu = 45, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(62), SoTienThu = 1400000 },
                
                new PhieuThu { MaPhieuThu = 46, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(66), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 47, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(70), SoTienThu = 1700000 },
                new PhieuThu { MaPhieuThu = 48, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(74), SoTienThu = 2000000 },
                new PhieuThu { MaPhieuThu = 49, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(78), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 50, MaDaiLy = 3, NgayThuTien = seedDate.AddMonths(82), SoTienThu = 2000000 },
                                // ------------------------ PHIẾU THU – ĐẠI LÝ 4 ------------------------
                new PhieuThu { MaPhieuThu = 51, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(4), SoTienThu = 2000000 },
                new PhieuThu { MaPhieuThu = 52, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(8), SoTienThu = 1600000 },
                new PhieuThu { MaPhieuThu = 53, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(12), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 54, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(16), SoTienThu = 1700000 },
                new PhieuThu { MaPhieuThu = 55, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(20), SoTienThu = 1900000 },
                
                new PhieuThu { MaPhieuThu = 56, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(24), SoTienThu = 1500000 },
                new PhieuThu { MaPhieuThu = 57, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(28), SoTienThu = 2200000 },
                new PhieuThu { MaPhieuThu = 58, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(32), SoTienThu = 1400000 },
                new PhieuThu { MaPhieuThu = 59, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(36), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 60, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(40), SoTienThu = 1600000 },
                
                new PhieuThu { MaPhieuThu = 61, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(44), SoTienThu = 1700000 },
                new PhieuThu { MaPhieuThu = 62, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(48), SoTienThu = 1900000 },
                new PhieuThu { MaPhieuThu = 63, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(52), SoTienThu = 2000000 },
                new PhieuThu { MaPhieuThu = 64, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(56), SoTienThu = 2100000 },
                new PhieuThu { MaPhieuThu = 65, MaDaiLy = 4, NgayThuTien = seedDate.AddMonths(60), SoTienThu = 1300000 },
                // ------------------------ PHIẾU THU – ĐẠI LÝ 5 ------------------------
                new PhieuThu { MaPhieuThu = 82, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(4), SoTienThu = 2000000 },
                new PhieuThu { MaPhieuThu = 83, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(8), SoTienThu = 1600000 },
                new PhieuThu { MaPhieuThu = 84, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(12), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 85, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(16), SoTienThu = 1700000 },
                new PhieuThu { MaPhieuThu = 86, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(20), SoTienThu = 1900000 },
                
                new PhieuThu { MaPhieuThu = 87, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(24), SoTienThu = 2000000 },
                new PhieuThu { MaPhieuThu = 88, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(28), SoTienThu = 1600000 },
                new PhieuThu { MaPhieuThu = 89, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(32), SoTienThu = 1800000 },
                new PhieuThu { MaPhieuThu = 90, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(36), SoTienThu = 1700000 },
                new PhieuThu { MaPhieuThu = 91, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(40), SoTienThu = 1900000 },
                
                new PhieuThu { MaPhieuThu = 92, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(44), SoTienThu = 1500000 },
                new PhieuThu { MaPhieuThu = 93, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(48), SoTienThu = 1500000 },
                new PhieuThu { MaPhieuThu = 94, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(52), SoTienThu = 1500000 },
                new PhieuThu { MaPhieuThu = 95, MaDaiLy = 5, NgayThuTien = seedDate.AddMonths(56), SoTienThu = 1500000 }



            );
        }
    }
}