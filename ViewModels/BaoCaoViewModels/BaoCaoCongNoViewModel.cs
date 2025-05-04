using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using QuanLyDaiLy.Views.BaoCaoViews;
using QuanLyDaiLy.Services;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuanLyDaiLy.ViewModels.BaoCaoViewModels
{
    public class BaoCaoCongNo
    {
        public int STT { get; set; }
        public string TenDaiLy { get; set; }
        public decimal NoDauThang { get; set; }
        public decimal NoCuoiThang { get; set; }
        public decimal GiaTriGiaoDich { get; set; }
    }
    public class BaoCaoCongNoViewModel : INotifyPropertyChanged
    {
        private readonly IDaiLyService _daiLyService;
        private readonly IPhieuXuatService _phieuXuatService;
        private readonly IPhieuThuService _phieuThuService;
        public BaoCaoCongNoViewModel(
            string selectedMonth,
            int selectedYear,
            IDaiLyService daiLyService,
            IPhieuXuatService phieuXuatService,
            IPhieuThuService phieuThuService
        )
        {
            _daiLyService = daiLyService;
            _phieuXuatService = phieuXuatService;
            _phieuThuService = phieuThuService;

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

            SelectedMonth = selectedMonth;
            SelectedYear = selectedYear;

            _ = LoadData();
            LapBaoCaoCommand = new RelayCommand(ExportToPDF);
            CloseCommand = new RelayCommand(CloseWindow);
        }
        public event EventHandler? DataChanged;
        public ObservableCollection<string> MonthOptions { get; set; }
        public ObservableCollection<int> YearOptions { get; set; }

        private string _selectedMonth = "Tháng 1";
        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    _ = LoadData();
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedYear = DateTime.Now.Year;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    _ = LoadData();
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<BaoCaoCongNo> _baoCaoCongNoList = new();
        public ObservableCollection<BaoCaoCongNo> BaoCaoCongNoList
        {
            get => _baoCaoCongNoList;
            set
            {
                _baoCaoCongNoList = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand LapBaoCaoCommand { get; }
        private async Task LoadData()
        {
            BaoCaoCongNoList.Clear();

            var allDaiLy = await _daiLyService.GetAllDaiLy();

            int month = int.Parse(SelectedMonth.Replace("Tháng ", ""));
            int year = SelectedYear;

            DateTime dauThang = new DateTime(year, month, 1);
            DateTime cuoiThang = dauThang.AddMonths(1).AddDays(-1); // cuối tháng đang xét

            foreach (var daiLy in allDaiLy)
            {
                int maDaiLy = daiLy.MaDaiLy;

                // Lấy phiếu xuất và phiếu thu của đại lý
                var phieuXuat = await _phieuXuatService.GetPhieuXuatByDaiLyId(maDaiLy);
                var phieuThu = await _phieuThuService.GetPhieuThuByDaiLyId(maDaiLy);

                // Tổng giá trị phiếu xuất trước tháng hiện tại (nợ đầu)
                decimal tongPhieuXuatTruoc = phieuXuat
                    .Where(p => p.NgayLapPhieu < dauThang)
                    .Sum(p => p.TongTriGia);

                // Tổng giá trị phiếu thu trước tháng hiện tại (nợ đầu)
                decimal tongPhieuThuTruoc = phieuThu
                    .Where(p => p.NgayThuTien < dauThang)
                    .Sum(p => p.SoTienThu);

                // Nợ đầu tháng
                decimal noDauThang = tongPhieuXuatTruoc - tongPhieuThuTruoc;

                // Giá trị giao dịch = tổng phiếu xuất trong tháng
                decimal tongPhieuXuatTrongThang = phieuXuat
                    .Where(p => p.NgayLapPhieu.Month == month && p.NgayLapPhieu.Year == year)
                    .Sum(p => p.TongTriGia);

                // Tổng thu trong tháng
                decimal tongPhieuThuTrongThang = phieuThu
                    .Where(p => p.NgayThuTien.Month == month && p.NgayThuTien.Year == year)
                    .Sum(p => p.SoTienThu);

                // Nợ cuối tháng = Nợ đầu - tổng phiếu thu trong tháng
                decimal noCuoiThang = noDauThang - tongPhieuThuTrongThang;

                // Thêm vào danh sách báo cáo
                BaoCaoCongNoList.Add(new BaoCaoCongNo
                {
                    STT = BaoCaoCongNoList.Count + 1,
                    TenDaiLy = daiLy.TenDaiLy,
                    NoDauThang = noDauThang,
                    GiaTriGiaoDich = tongPhieuXuatTrongThang,
                    NoCuoiThang = noCuoiThang
                });
            }
        }
        private void ExportToPDF()
        {
            if (BaoCaoCongNoList == null || BaoCaoCongNoList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string baseFileName = $"BaoCaoCongNo_{SelectedMonth.Replace(" ", "")}_{SelectedYear}";
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
                                .Text($"Báo Cáo Công Nợ {SelectedMonth} - {SelectedYear}")
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

                                // Thông tin người lập và ngày lập
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
                                            columns.RelativeColumn(); // Nợ Đầu Tháng
                                            columns.RelativeColumn(); // Giá Trị Giao Dịch
                                            columns.RelativeColumn(); // Nợ Cuối Tháng
                                        });

                                        // Tiêu đề cột (Thêm màu nền xanh nhạt và căn giữa)
                                        table.Header(header =>
                                        {
                                            header.Cell().Element(CellStyle).AlignCenter().Text("STT").Bold();
                                            header.Cell().Element(CellStyle).AlignCenter().Text("Tên Đại Lý").Bold();
                                            header.Cell().Element(CellStyle).AlignCenter().Text("Nợ Đầu Tháng").Bold();
                                            header.Cell().Element(CellStyle).AlignCenter().Text("Giá Trị Giao Dịch").Bold();
                                            header.Cell().Element(CellStyle).AlignCenter().Text("Nợ Cuối Tháng").Bold();
                                        });

                                        // Dữ liệu (Căn giữa các ô)
                                        foreach (var item in BaoCaoCongNoList)
                                        {
                                            table.Cell().Element(CellStyle).AlignCenter().Text(item.STT.ToString());
                                            table.Cell().Element(CellStyle).AlignCenter().Text(item.TenDaiLy ?? "");
                                            table.Cell().Element(CellStyle).AlignCenter().Text(item.NoDauThang.ToString("N0") + " VNĐ");
                                            table.Cell().Element(CellStyle).AlignCenter().Text(item.GiaTriGiaoDich.ToString("N0") + " VNĐ");
                                            table.Cell().Element(CellStyle).AlignCenter().Text(item.NoCuoiThang.ToString("N0") + " VNĐ");
                                        }
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
        }

        // Style cho mỗi cell trong bảng
        QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2)
                .Padding(5);
        }


        private void CloseWindow()
        {
            Application.Current.Windows.OfType<BaoCaoCongNoWindow>().FirstOrDefault()?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
