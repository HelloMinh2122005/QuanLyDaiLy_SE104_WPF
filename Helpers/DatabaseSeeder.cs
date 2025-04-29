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
                    TienNo = 3000000L
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
                    TienNo = 7000000L
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
                    TienNo = 2000000L
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
                    TienNo = 8500000L
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
                    SoLuongTon = 9000
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
                new PhieuXuat { MaPhieuXuat = 30, MaDaiLy = 1, NgayLapPhieu = seedDate.AddMonths(58), TongTriGia = 500000 }

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
                new ChiTietPhieuXuat { MaChiTietPhieuXuat = 90, MaPhieuXuat = 30, MaMatHang = 1, SoLuongXuat = 10, DonGia = 50000, ThanhTien = 500000 }

            );
        }

        private static void SeedPhieuThu(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2023, 1, 1);

            modelBuilder.Entity<PhieuThu>().HasData(
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
                new PhieuThu { MaPhieuThu = 15, MaDaiLy = 1, NgayThuTien = seedDate.AddMonths(60), SoTienThu = 1700000 }
            );
        }
    }
}