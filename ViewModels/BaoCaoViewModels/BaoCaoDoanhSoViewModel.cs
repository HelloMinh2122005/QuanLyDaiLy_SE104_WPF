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
using OfficeOpenXml;
using GemBox.Spreadsheet;

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

            // Chờ kết quả từ cả hai dịch vụ
            var allPhieuXuat = await allPhieuXuatTask;
            var allDaiLy = await allDaiLyTask;

            int month = int.Parse(SelectedMonth.Replace("Tháng ", ""));
            int year = SelectedYear;

            // Lọc phiếu xuất trong tháng và năm đã chọn và nhóm theo mã đại lý
            var phieuTrongThang = allPhieuXuat
                .Where(p => p.NgayLapPhieu.Month == month && p.NgayLapPhieu.Year == year)
                .GroupBy(p => p.MaDaiLy)
                .ToDictionary(g => g.Key, g => g.ToList());

            decimal tongTatCa = 0;

            // Duyệt qua tất cả đại lý và tính toán báo cáo
            foreach (var daiLy in allDaiLy)
            {
                if (!phieuTrongThang.ContainsKey(daiLy.MaDaiLy))
                    continue;

                var phieuCuaDaiLy = phieuTrongThang[daiLy.MaDaiLy];

                // Tính tổng giá trị giao dịch của đại lý
                decimal tongGiaTri = phieuCuaDaiLy.Sum(p => p.TongTriGia);

                BaoCaoDoanhSoList.Add(new BaoCaoDoanhSo
                {
                    STT = BaoCaoDoanhSoList.Count + 1,
                    TenDaiLy = daiLy.TenDaiLy,
                    SoLuongPhieuXuat = phieuCuaDaiLy.Count,
                    TongGiaTriGiaoDich = tongGiaTri,
                    TiLe = 0 // sẽ tính tiếp ở dưới
                });

                tongTatCa += tongGiaTri;
            }

            // Cập nhật tổng doanh số của tất cả các đại lý
            TotalDoanhSo = tongTatCa;

            // Tính tỉ lệ cho mỗi đại lý nếu tổng giá trị giao dịch lớn hơn 0
            if (tongTatCa > 0)
            {
                foreach (var item in BaoCaoDoanhSoList)
                {
                    item.TiLe = Math.Round((double)(item.TongGiaTriGiaoDich / tongTatCa) * 100, 2);
                }
            }
        }

        public ICommand ExportToPDFCommand { get; }
        public ICommand CloseCommand { get; }

        [Obsolete]
        private void ExportToPDF()
        {
            // Thiết lập license cho GemBox.Spreadsheet
            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            if (BaoCaoDoanhSoList == null || BaoCaoDoanhSoList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
                        var ws = package.Workbook.Worksheets.Add("Báo Cáo");

                        ws.Cells["A1"].Value = $"Báo Cáo Doanh Số {SelectedMonth} - {SelectedYear}";
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

                        ws.Cells["A5"].Value = "STT";
                        ws.Cells["B5"].Value = "Tên Đại Lý";
                        ws.Cells["C5"].Value = "Số Lượng Phiếu Xuất";
                        ws.Cells["D5"].Value = "Tổng Giá Trị Giao Dịch (VNĐ)";
                        ws.Cells["E5"].Value = "Tỉ Lệ (%)";

                        ws.Cells["A5:E5"].Style.Font.Bold = true;
                        ws.Cells["A5:E5"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells["A5:E5"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        int startRow = 6;
                        int row = startRow;

                        foreach (var item in BaoCaoDoanhSoList)
                        {
                            ws.Cells[row, 1].Value = item.STT;
                            ws.Cells[row, 2].Value = item.TenDaiLy;
                            ws.Cells[row, 3].Value = item.SoLuongPhieuXuat;
                            ws.Cells[row, 4].Value = item.TongGiaTriGiaoDich;
                            ws.Cells[row, 5].Value = item.TiLe;

                            // Format số có cách 3 chữ số ở cột giá trị
                            ws.Cells[row, 4].Style.Numberformat.Format = "#,##0";

                            row++;
                        }

                        ws.Cells[row, 1, row, 3].Merge = true;
                        ws.Cells[row, 1].Value = "Tổng Doanh Số Của Tất Cả Đại Lý";
                        ws.Cells[row, 1].Style.Font.Bold = true;

                        ws.Cells[row, 4].Value = TotalDoanhSo;
                        ws.Cells[row, 4].Style.Font.Bold = true;
                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0";

                        ws.Cells[row, 4, row, 5].Merge = true;

                        row += 2;

                        ws.Cells[ws.Dimension.Address].AutoFitColumns();

                        var borderRange = ws.Cells[$"A5:E{row}"];
                        borderRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        borderRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        borderRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        borderRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        borderRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        borderRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        ws.Cells["A2:B3"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                        ws.Cells["A2:B3"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                        ws.Cells["A2:B3"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                        ws.Cells["A2:B3"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;

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
