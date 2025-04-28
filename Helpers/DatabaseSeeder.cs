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
                new ThamSo { Id = 1, SoLuongDaiLyToiDa = 4, QuyDinhSoLuongDaiLyToiDa = true, QuyDinhTienThuTienNo = true }
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
                new Quan { MaQuan = 20, TenQuan = "Huyện Hóc Môn" }
            );
        }

        private static void SeedLoaiDaiLy(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoaiDaiLy>().HasData(
                new LoaiDaiLy { MaLoaiDaiLy = 1, TenLoaiDaiLy = "Đại lý loại 1", NoToiDa = 20000 },
                new LoaiDaiLy { MaLoaiDaiLy = 2, TenLoaiDaiLy = "Đại lý loại 2", NoToiDa = 50 }
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
                    TienNo = 5000000L
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
                    SoLuongTon = 100
                },
                new
                {
                    MaMatHang = 2,
                    TenMatHang = "Nước ngọt",
                    MaDonViTinh = 5,
                    SoLuongTon = 200
                },
                new
                {
                    MaMatHang = 3,
                    TenMatHang = "Mì tôm",
                    MaDonViTinh = 3,
                    SoLuongTon = 150
                },
                // 4-23: Additional mat hang
                new
                {
                    MaMatHang = 4,
                    TenMatHang = "Đường",
                    MaDonViTinh = 1,
                    SoLuongTon = 80
                },
                new
                {
                    MaMatHang = 5,
                    TenMatHang = "Muối",
                    MaDonViTinh = 1,
                    SoLuongTon = 120
                },
                new
                {
                    MaMatHang = 6,
                    TenMatHang = "Bột giặt",
                    MaDonViTinh = 3,
                    SoLuongTon = 75
                },
                new
                {
                    MaMatHang = 7,
                    TenMatHang = "Nước mắm",
                    MaDonViTinh = 5,
                    SoLuongTon = 95
                },
                new
                {
                    MaMatHang = 8,
                    TenMatHang = "Dầu ăn",
                    MaDonViTinh = 5,
                    SoLuongTon = 110
                },
                new
                {
                    MaMatHang = 9,
                    TenMatHang = "Sữa tươi",
                    MaDonViTinh = 5,
                    SoLuongTon = 180
                },
                new
                {
                    MaMatHang = 10,
                    TenMatHang = "Bánh kẹo",
                    MaDonViTinh = 2,
                    SoLuongTon = 250
                },
                new
                {
                    MaMatHang = 11,
                    TenMatHang = "Nước rửa chén",
                    MaDonViTinh = 5,
                    SoLuongTon = 60
                },
                new
                {
                    MaMatHang = 12,
                    TenMatHang = "Bia",
                    MaDonViTinh = 3,
                    SoLuongTon = 85
                },
                new
                {
                    MaMatHang = 13,
                    TenMatHang = "Nước suối",
                    MaDonViTinh = 5,
                    SoLuongTon = 300
                },
                new
                {
                    MaMatHang = 14,
                    TenMatHang = "Bột ngọt",
                    MaDonViTinh = 1,
                    SoLuongTon = 40
                },
                new
                {
                    MaMatHang = 15,
                    TenMatHang = "Giấy vệ sinh",
                    MaDonViTinh = 3,
                    SoLuongTon = 120
                },
                new
                {
                    MaMatHang = 16,
                    TenMatHang = "Khăn giấy",
                    MaDonViTinh = 2,
                    SoLuongTon = 90
                },
                new
                {
                    MaMatHang = 17,
                    TenMatHang = "Nước tương",
                    MaDonViTinh = 5,
                    SoLuongTon = 70
                },
                new
                {
                    MaMatHang = 18,
                    TenMatHang = "Tương ớt",
                    MaDonViTinh = 5,
                    SoLuongTon = 65
                },
                new
                {
                    MaMatHang = 19,
                    TenMatHang = "Bột canh",
                    MaDonViTinh = 1,
                    SoLuongTon = 55
                },
                new
                {
                    MaMatHang = 20,
                    TenMatHang = "Dầu gội",
                    MaDonViTinh = 5,
                    SoLuongTon = 80
                },
                new
                {
                    MaMatHang = 21,
                    TenMatHang = "Sữa tắm",
                    MaDonViTinh = 5,
                    SoLuongTon = 75
                },
                new
                {
                    MaMatHang = 22,
                    TenMatHang = "Kem đánh răng",
                    MaDonViTinh = 2,
                    SoLuongTon = 100
                },
                new
                {
                    MaMatHang = 23,
                    TenMatHang = "Cafe",
                    MaDonViTinh = 1,
                    SoLuongTon = 60
                }
            );
        }

        private static void SeedPhieuXuat(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2023, 1, 1);

            modelBuilder.Entity<PhieuXuat>().HasData(
                new PhieuXuat
                {
                    MaPhieuXuat = 1,
                    MaDaiLy = 1,
                    NgayLapPhieu = seedDate.AddDays(5),
                    TongTriGia = 2500000
                },
                new PhieuXuat
                {
                    MaPhieuXuat = 2,
                    MaDaiLy = 2,
                    NgayLapPhieu = seedDate.AddDays(10),
                    TongTriGia = 1800000
                },
                new PhieuXuat
                {
                    MaPhieuXuat = 3,
                    MaDaiLy = 3,
                    NgayLapPhieu = seedDate.AddDays(15),
                    TongTriGia = 3200000
                },
                new PhieuXuat
                {
                    MaPhieuXuat = 4,
                    MaDaiLy = 4,
                    NgayLapPhieu = seedDate.AddDays(20),
                    TongTriGia = 1500000
                },
                new PhieuXuat
                {
                    MaPhieuXuat = 5,
                    MaDaiLy = 5,
                    NgayLapPhieu = seedDate.AddDays(25),
                    TongTriGia = 2200000
                },
                new PhieuXuat
                {
                    MaPhieuXuat = 6,
                    MaDaiLy = 1,
                    NgayLapPhieu = seedDate.AddDays(35),
                    TongTriGia = 1700000
                },
                new PhieuXuat
                {
                    MaPhieuXuat = 7,
                    MaDaiLy = 2,
                    NgayLapPhieu = seedDate.AddDays(40),
                    TongTriGia = 2800000
                },
                new PhieuXuat
                {
                    MaPhieuXuat = 8,
                    MaDaiLy = 6,
                    NgayLapPhieu = seedDate.AddDays(45),
                    TongTriGia = 3500000
                },
                new PhieuXuat
                {
                    MaPhieuXuat = 9,
                    MaDaiLy = 7,
                    NgayLapPhieu = seedDate.AddDays(50),
                    TongTriGia = 1900000
                },
                new PhieuXuat
                {
                    MaPhieuXuat = 10,
                    MaDaiLy = 8,
                    NgayLapPhieu = seedDate.AddDays(55),
                    TongTriGia = 2700000
                }
            );
        }

        private static void SeedChiTietPhieuXuat(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietPhieuXuat>().HasData(
                // Phiếu xuất 1
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 1,
                    MaPhieuXuat = 1,
                    MaMatHang = 1,
                    SoLuongXuat = 10,
                    DonGia = 120000,
                    ThanhTien = 1200000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 2,
                    MaPhieuXuat = 1,
                    MaMatHang = 3,
                    SoLuongXuat = 5,
                    DonGia = 260000,
                    ThanhTien = 1300000
                },

                // Phiếu xuất 2
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 3,
                    MaPhieuXuat = 2,
                    MaMatHang = 2,
                    SoLuongXuat = 20,
                    DonGia = 30000,
                    ThanhTien = 600000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 4,
                    MaPhieuXuat = 2,
                    MaMatHang = 4,
                    SoLuongXuat = 15,
                    DonGia = 80000,
                    ThanhTien = 1200000
                },

                // Phiếu xuất 3
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 5,
                    MaPhieuXuat = 3,
                    MaMatHang = 5,
                    SoLuongXuat = 8,
                    DonGia = 50000,
                    ThanhTien = 400000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 6,
                    MaPhieuXuat = 3,
                    MaMatHang = 6,
                    SoLuongXuat = 10,
                    DonGia = 180000,
                    ThanhTien = 1800000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 7,
                    MaPhieuXuat = 3,
                    MaMatHang = 7,
                    SoLuongXuat = 20,
                    DonGia = 50000,
                    ThanhTien = 1000000
                },

                // Phiếu xuất 4
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 8,
                    MaPhieuXuat = 4,
                    MaMatHang = 8,
                    SoLuongXuat = 12,
                    DonGia = 75000,
                    ThanhTien = 900000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 9,
                    MaPhieuXuat = 4,
                    MaMatHang = 9,
                    SoLuongXuat = 10,
                    DonGia = 60000,
                    ThanhTien = 600000
                },

                // Phiếu xuất 5
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 10,
                    MaPhieuXuat = 5,
                    MaMatHang = 10,
                    SoLuongXuat = 25,
                    DonGia = 28000,
                    ThanhTien = 700000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 11,
                    MaPhieuXuat = 5,
                    MaMatHang = 11,
                    SoLuongXuat = 15,
                    DonGia = 100000,
                    ThanhTien = 1500000
                },

                // Phiếu xuất 6
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 12,
                    MaPhieuXuat = 6,
                    MaMatHang = 12,
                    SoLuongXuat = 10,
                    DonGia = 170000,
                    ThanhTien = 1700000
                },

                // Phiếu xuất 7
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 13,
                    MaPhieuXuat = 7,
                    MaMatHang = 13,
                    SoLuongXuat = 30,
                    DonGia = 20000,
                    ThanhTien = 600000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 14,
                    MaPhieuXuat = 7,
                    MaMatHang = 14,
                    SoLuongXuat = 10,
                    DonGia = 40000,
                    ThanhTien = 400000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 15,
                    MaPhieuXuat = 7,
                    MaMatHang = 15,
                    SoLuongXuat = 12,
                    DonGia = 150000,
                    ThanhTien = 1800000
                },

                // Phiếu xuất 8
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 16,
                    MaPhieuXuat = 8,
                    MaMatHang = 16,
                    SoLuongXuat = 20,
                    DonGia = 45000,
                    ThanhTien = 900000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 17,
                    MaPhieuXuat = 8,
                    MaMatHang = 17,
                    SoLuongXuat = 25,
                    DonGia = 40000,
                    ThanhTien = 1000000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 18,
                    MaPhieuXuat = 8,
                    MaMatHang = 18,
                    SoLuongXuat = 40,
                    DonGia = 40000,
                    ThanhTien = 1600000
                },

                // Phiếu xuất 9
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 19,
                    MaPhieuXuat = 9,
                    MaMatHang = 19,
                    SoLuongXuat = 15,
                    DonGia = 30000,
                    ThanhTien = 450000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 20,
                    MaPhieuXuat = 9,
                    MaMatHang = 20,
                    SoLuongXuat = 10,
                    DonGia = 85000,
                    ThanhTien = 850000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 21,
                    MaPhieuXuat = 9,
                    MaMatHang = 21,
                    SoLuongXuat = 6,
                    DonGia = 100000,
                    ThanhTien = 600000
                },

                // Phiếu xuất 10
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 22,
                    MaPhieuXuat = 10,
                    MaMatHang = 22,
                    SoLuongXuat = 18,
                    DonGia = 50000,
                    ThanhTien = 900000
                },
                new ChiTietPhieuXuat
                {
                    MaChiTietPhieuXuat = 23,
                    MaPhieuXuat = 10,
                    MaMatHang = 23,
                    SoLuongXuat = 15,
                    DonGia = 120000,
                    ThanhTien = 1800000
                }
            );
        }

        private static void SeedPhieuThu(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2023, 1, 1);

            modelBuilder.Entity<PhieuThu>().HasData(
                new PhieuThu
                {
                    MaPhieuThu = 1,
                    MaDaiLy = 1,
                    NgayThuTien = seedDate.AddDays(15),
                    SoTienThu = 2000000
                },
                new PhieuThu
                {
                    MaPhieuThu = 2,
                    MaDaiLy = 2,
                    NgayThuTien = seedDate.AddDays(20),
                    SoTienThu = 1500000
                },
                new PhieuThu
                {
                    MaPhieuThu = 3,
                    MaDaiLy = 3,
                    NgayThuTien = seedDate.AddDays(25),
                    SoTienThu = 3000000
                },
                new PhieuThu
                {
                    MaPhieuThu = 4,
                    MaDaiLy = 4,
                    NgayThuTien = seedDate.AddDays(30),
                    SoTienThu = 1000000
                },
                new PhieuThu
                {
                    MaPhieuThu = 5,
                    MaDaiLy = 5,
                    NgayThuTien = seedDate.AddDays(35),
                    SoTienThu = 2500000
                }
            );
        }
    }
}