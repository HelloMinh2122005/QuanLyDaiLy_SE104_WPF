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
using GemBox.Spreadsheet;

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
            ExportToPDFCommand = new RelayCommand(ExportToPDF);
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
        public ICommand ExportToPDFCommand { get; }
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

        [Obsolete]
        private void ExportToPDF()
        {
            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            if (BaoCaoCongNoList == null || BaoCaoCongNoList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = Path.GetFileName(pdfFilePath),
                InitialDirectory = folderPath
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                pdfFilePath = saveFileDialog.FileName;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (var memoryStream = new MemoryStream())
                {
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("Báo Cáo Công Nợ");

                        ws.Cells["A1"].Value = $"Báo Cáo Công Nợ {SelectedMonth} - {SelectedYear}";
                        ws.Cells["A1:E1"].Merge = true;
                        ws.Cells["A1"].Style.Font.Size = 16;
                        ws.Cells["A1"].Style.Font.Bold = true;
                        ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        string nguoiLap = "Nguyễn Văn A";
                        string ngayLap = DateTime.Now.ToString("dd/MM/yyyy");

                        ws.Cells["A2"].Value = "Người lập:";
                        ws.Cells["B2"].Value = nguoiLap;
                        ws.Cells["B2"].Style.Font.Italic = true;

                        ws.Cells["A3"].Value = "Ngày lập:";
                        ws.Cells["B3"].Value = ngayLap;
                        ws.Cells["B3"].Style.Font.Italic = true;

                        // Tiêu đề bảng
                        ws.Cells["A5"].Value = "STT";
                        ws.Cells["B5"].Value = "Tên Đại Lý";
                        ws.Cells["C5"].Value = "Nợ Đầu Tháng";
                        ws.Cells["D5"].Value = "Giá Trị Giao Dịch";
                        ws.Cells["E5"].Value = "Nợ Cuối Tháng";

                        ws.Cells["A5:E5"].Style.Font.Bold = true;
                        ws.Cells["A5:E5"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells["A5:E5"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSalmon);

                        int row = 6;

                        foreach (var item in BaoCaoCongNoList)
                        {
                            ws.Cells[row, 1].Value = item.STT;
                            ws.Cells[row, 2].Value = item.TenDaiLy;
                            ws.Cells[row, 3].Value = item.NoDauThang;
                            ws.Cells[row, 4].Value = item.GiaTriGiaoDich;
                            ws.Cells[row, 5].Value = item.NoCuoiThang;

                            // Format tiền
                            ws.Cells[row, 3].Style.Numberformat.Format = "#,##0\" VNĐ\"";
                            ws.Cells[row, 4].Style.Numberformat.Format = "#,##0\" VNĐ\"";
                            ws.Cells[row, 5].Style.Numberformat.Format = "#,##0\" VNĐ\"";


                            row++;
                        }

                        // AutoFit và định dạng bảng
                        ws.Cells[ws.Dimension.Address].AutoFitColumns();

                        var borderRange = ws.Cells[$"A5:E{row - 1}"];
                        borderRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        borderRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        borderRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        borderRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        borderRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        borderRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        ws.PrinterSettings.HorizontalCentered = true;
                        ws.PrinterSettings.PaperSize = ePaperSize.A4;
                        ws.PrinterSettings.FitToPage = true;
                        ws.PrinterSettings.FitToWidth = 1;
                        ws.PrinterSettings.FitToHeight = 0;

                        ws.PrinterSettings.LeftMargin = 0.5M;
                        ws.PrinterSettings.RightMargin = 0.5M;
                        ws.PrinterSettings.TopMargin = 0.5M;
                        ws.PrinterSettings.BottomMargin = 0.5M;

                        ws.PrinterSettings.RepeatRows = new ExcelAddress("$5:$5");

                        package.SaveAs(memoryStream);
                    }

                    memoryStream.Position = 0;

                    GemBox.Spreadsheet.ExcelFile.Load(memoryStream).Save(pdfFilePath, GemBox.Spreadsheet.SaveOptions.PdfDefault);

                    MessageBox.Show("Xuất PDF thành công:\n" + pdfFilePath, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = pdfFilePath,
                            UseShellExecute = true
                        });
                    }
                    catch { }
                }
            }
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
