using QuanLyDaiLy.Views.BaoCaoViews;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using QuanLyDaiLy.Commands;
using System.Collections.ObjectModel;
using QuanLyDaiLy.Services;
using Microsoft.Win32;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuanLyDaiLy.ViewModels.BaoCaoViewModels
{
    public class BaoCaoDoanhSo
    {
        public int STT { get; set; }
        public string TenDaiLy { get; set; }
        public int SoLuongPhieuXuat { get; set; }
        public decimal TongGiaTriGiaoDich { get; set; }
        public double TiLe { get; set; }
    }
    public class BaoCaoDoanhSoViewModel : INotifyPropertyChanged
    {
        private readonly IDaiLyService _daiLyService;
        private readonly IPhieuXuatService _phieuXuatService;

        public BaoCaoDoanhSoViewModel(
            string selectedMonth,
            int selectedYear,
            IDaiLyService daiLyService,
            IPhieuXuatService phieuXuatService
        )
        {
            _daiLyService = daiLyService;
            _phieuXuatService = phieuXuatService;

            MonthOptions = 
            [
                "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4",
                "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
                "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
            ];

            int currentYear = DateTime.Now.Year;
            YearOptions = new ObservableCollection<int>();
            for (int i = currentYear - 4; i <= currentYear; i++)
            {
                YearOptions.Add(i);
            }

            SelectedMonth = selectedMonth;
            SelectedYear = selectedYear;

            _ = LoadData();
            ExportToPDFCommand = new RelayCommand(ExportToPDF);
            CloseCommand = new RelayCommand(CloseWindow);
        }

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
                    OnPropertyChanged();
                    _ = LoadData();
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
                    OnPropertyChanged();
                    _ = LoadData();
                }
            }
        }

        private ObservableCollection<BaoCaoDoanhSo> _baoCaoDoanhSoList = new();
        public ObservableCollection<BaoCaoDoanhSo> BaoCaoDoanhSoList
        {
            get => _baoCaoDoanhSoList;
            set
            {
                _baoCaoDoanhSoList = value;
                OnPropertyChanged();
            }
        }
        private decimal _totalDoanhSo;
        public decimal TotalDoanhSo
        {
            get => _totalDoanhSo;
            set
            {
                if (_totalDoanhSo != value)
                {
                    _totalDoanhSo = value;
                    OnPropertyChanged();
                }
            }
        }

        private async Task LoadData()
        {
            BaoCaoDoanhSoList.Clear();

            // Lấy dữ liệu từ dịch vụ song song
            var allPhieuXuatTask = _phieuXuatService.GetAllPhieuXuat();
            var allDaiLyTask = _daiLyService.GetAllDaiLy();

            var allPhieuXuat = await allPhieuXuatTask;
            var allDaiLy = await allDaiLyTask;

            int month = int.Parse(SelectedMonth.Replace("Tháng ", ""));
            int year = SelectedYear;

            // Lọc phiếu xuất trong tháng và năm đã chọn
            var phieuTrongThang = allPhieuXuat
                .Where(p => p.NgayLapPhieu.Month == month && p.NgayLapPhieu.Year == year)
                .ToList();

            decimal tongTatCa = 0;
            int stt = 1;

            foreach (var daiLy in allDaiLy)
            {
                // Lấy phiếu xuất tương ứng với đại lý này trong tháng
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
                    item.TiLe = Math.Round((double)(item.TongGiaTriGiaoDich / tongTatCa)*100, 2);
                }
            }
        }





        public ICommand ExportToPDFCommand { get; }
        public ICommand CloseCommand { get; }
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







        public event EventHandler? DataChanged;
        private void CloseWindow()
        {
            Application.Current.Windows.OfType<BaoCaoDoanhSoWindow>().FirstOrDefault()?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
