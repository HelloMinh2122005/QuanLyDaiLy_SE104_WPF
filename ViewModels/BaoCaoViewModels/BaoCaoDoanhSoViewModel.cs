using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models.dto;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.BaoCaoViews;
using QuanLyDaiLy.Views.MatHangViews;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuanLyDaiLy.ViewModels.BaoCaoViewModels
{
    public partial class BaoCaoDoanhSoViewModel : ObservableObject, IRecipient<SelectedDateMessage>
    {
        private readonly IDaiLyService _daiLyService;
        private readonly IPhieuXuatService _phieuXuatService;
        private string _selectedMonth;
        private int _selectedYear;

        public BaoCaoDoanhSoViewModel(
            IDaiLyService daiLyService,
            IPhieuXuatService phieuXuatService
        )
        {
            _daiLyService = daiLyService;
            _phieuXuatService = phieuXuatService;
            WeakReferenceMessenger.Default.RegisterAll(this);

            MonthOptions = new ObservableCollection<string>
            {
                "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4",
                "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
                "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
            };

            int currentYear = DateTime.Now.Year;
            YearOptions = new ObservableCollection<int>();
            for (int i = currentYear - 4; i <= currentYear; i++)
            {
                YearOptions.Add(i);
            }

            SelectedMonth = MonthOptions.First();
            SelectedYear = YearOptions.Last();

            _ = LoadData();
        }

        public ObservableCollection<string> MonthOptions { get; set; }
        public ObservableCollection<int> YearOptions { get; set; }

        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    SetProperty(ref _selectedMonth, value);
                    _ = LoadData();
                }
            }
        }

        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    SetProperty(ref _selectedYear, value);
                    _ = LoadData();
                }
            }
        }

        private ObservableCollection<BaoCaoDoanhSo> _baoCaoDoanhSoList = new();
        public ObservableCollection<BaoCaoDoanhSo> BaoCaoDoanhSoList
        {
            get => _baoCaoDoanhSoList;
            set => SetProperty(ref _baoCaoDoanhSoList, value);
        }

        private decimal _totalDoanhSo;
        public decimal TotalDoanhSo
        {
            get => _totalDoanhSo;
            set => SetProperty(ref _totalDoanhSo, value);
        }

        private async Task LoadData()
        {
            BaoCaoDoanhSoList.Clear();

            var allPhieuXuatTask = _phieuXuatService.GetAllPhieuXuat();
            var allDaiLyTask = _daiLyService.GetAllDaiLy();

            var allPhieuXuat = await allPhieuXuatTask;
            var allDaiLy = await allDaiLyTask;

            int month = int.Parse(SelectedMonth.Replace("Tháng ", ""));
            int year = SelectedYear;

            var phieuTrongThang = allPhieuXuat
                .Where(p => p.NgayLapPhieu.Month == month && p.NgayLapPhieu.Year == year)
                .ToList();

            decimal tongTatCa = 0;
            int stt = 1;

            foreach (var daiLy in allDaiLy)
            {
                var phieuCuaDaiLy = phieuTrongThang
                    .Where(p => p.MaDaiLy == daiLy.MaDaiLy)
                    .ToList();

                decimal tongGiaTri = phieuCuaDaiLy.Sum(p => p.TongTriGia);

                BaoCaoDoanhSoList.Add(new BaoCaoDoanhSo
                {
                    STT = stt++,
                    TenDaiLy = daiLy.TenDaiLy,
                    SoLuongPhieuXuat = phieuCuaDaiLy.Count(),
                    TongGiaTriGiaoDich = tongGiaTri,
                    TiLe = 0.0
                });

                tongTatCa += tongGiaTri;
            }

            TotalDoanhSo = tongTatCa;

            if (tongTatCa > 0)
            {
                foreach (var item in BaoCaoDoanhSoList)
                {
                    item.TiLe = Math.Round((double)(item.TongGiaTriGiaoDich / tongTatCa) * 100, 2);
                }
            }
        }

        [RelayCommand]
        private void Close()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<BaoCaoDoanhSoWindow>().FirstOrDefault()?.Close();
        }
        [RelayCommand]
        private void ExportToPDF()
        {
            if (BaoCaoDoanhSoList == null || BaoCaoDoanhSoList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string baseFileName = $"BaoCaoDoanhSo_{SelectedMonth.Replace(" ", "")}_{SelectedYear}";
            string extension = ".pdf";
            string pdfFilePath = Path.Combine(folderPath, baseFileName + extension);

            int count = 1;
            while (File.Exists(pdfFilePath))
            {
                pdfFilePath = Path.Combine(folderPath, $"{baseFileName} ({count}){extension}");
                count++;
            }

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = Path.GetFileName(pdfFilePath),
                InitialDirectory = folderPath
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                pdfFilePath = saveFileDialog.FileName;

                var document = QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);

                        // Header
                        page.Header().Element(header =>
                        {
                            header
                                .PaddingBottom(10)
                                .Text($"Báo Cáo Doanh Số {SelectedMonth}-{SelectedYear}")
                                .FontSize(16)
                                .Bold()
                                .AlignCenter();
                        });

                        // Nội dung
                        page.Content().Element(content =>
                        {
                            content.PaddingVertical(10).Column(column =>
                            {
                                column.Spacing(5);

                                // Thông tin người lập
                                column.Item().Text($"Người lập: Nguyễn Văn A").Italic();
                                column.Item().Text($"Ngày lập: {DateTime.Now:dd/MM/yyyy}").Italic();

                                // Bảng dữ liệu
                                column.Item().Element(tableContainer =>
                                {
                                    tableContainer.Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(); // STT
                                            columns.RelativeColumn(); // Tên Đại Lý
                                            columns.RelativeColumn(); // Số Lượng Phiếu
                                            columns.RelativeColumn(); // Tổng GTGD
                                            columns.RelativeColumn(); // Tỉ lệ
                                        });

                                        // Tiêu đề cột (Thêm màu nền xanh nhạt và căn giữa)
                                        table.Header(header =>
                                        {
                                            header.Cell().Element(CellStyle).AlignCenter().Text("STT").Bold();
                                            header.Cell().Element(CellStyle).AlignCenter().Text("Tên Đại Lý").Bold();
                                            header.Cell().Element(CellStyle).AlignCenter().Text("Số Lượng Phiếu Xuất").Bold();
                                            header.Cell().Element(CellStyle).AlignCenter().Text("Tổng Giá Trị Giao Dịch").Bold();
                                            header.Cell().Element(CellStyle).AlignCenter().Text("Tỉ Lệ").Bold();
                                        });

                                        // Dữ liệu (Căn giữa các ô)
                                        foreach (var item in BaoCaoDoanhSoList)
                                        {
                                            table.Cell().Element(CellStyle).AlignCenter().Text(item.STT.ToString());
                                            table.Cell().Element(CellStyle).AlignCenter().Text(item.TenDaiLy ?? "");
                                            table.Cell().Element(CellStyle).AlignCenter().Text(item.SoLuongPhieuXuat.ToString());
                                            table.Cell().Element(CellStyle).AlignCenter().Text(item.TongGiaTriGiaoDich.ToString("N0") + " VNĐ");
                                            table.Cell().Element(CellStyle).AlignCenter().Text(item.TiLe.ToString("N2") + " %");
                                        }

                                        // Tổng doanh số
                                        table.Cell().ColumnSpan(3).Element(CellStyle).Text("Tổng Doanh Số Của Tất Cả Đại Lý").Bold().AlignCenter();
                                        table.Cell().ColumnSpan(2).Element(CellStyle).Text(TotalDoanhSo.ToString("N0") + " VNĐ").Bold().AlignCenter();
                                    });
                                });
                            });
                        });
                    });
                });

                try
                {
                    document.GeneratePdf(pdfFilePath);
                    MessageBox.Show($"Xuất PDF thành công:\n{pdfFilePath}", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = pdfFilePath,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo PDF: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // Style cho mỗi cell trong bảng
            QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container)
            {
                return container
                    .Border(1)
                    .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2)
                    .Padding(5);
            }
        }

        public void Receive(SelectedDateMessage message)
        {
            (int month, int year) = message.Value;
            _selectedMonth = $"Tháng {month}";
            _selectedYear = year;
            _ = LoadData();
        }
    }
}
