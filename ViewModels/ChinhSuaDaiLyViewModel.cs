using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace QuanLyDaiLy.ViewModels
{
    public class ChinhSuaDaiLyViewModel : INotifyPropertyChanged
    {
        private readonly IDaiLyService daiLyService;
        private readonly IQuanService quanService;
        private readonly ILoaiDaiLyService loaiDaiLyService;
        private readonly int _daiLyId;

        public ChinhSuaDaiLyViewModel(
            IDaiLyService daiLyService,
            IQuanService quanService,
            ILoaiDaiLyService loaiDaiLyService,
            int dailyId
        ) {
            this.daiLyService = daiLyService;
            this.quanService = quanService;
            this.loaiDaiLyService = loaiDaiLyService;
            _daiLyId = dailyId;

            CloseWindowCommand = new RelayCommand(CloseWindow);
            CapNhatDaiLyCommand = new RelayCommand(async () => await CapNhatDaiLy());

            _ = LoadDataAsync();
        }

        public event EventHandler? DataChanged;

        // Properties for binding
        private string _maDaiLy = string.Empty;
        public string MaDaiLy
        {
            get => _maDaiLy;
            set
            {
                _maDaiLy = value;
                OnPropertyChanged();
            }
        }

        private string _tenDaiLy = string.Empty;
        public string TenDaiLy
        {
            get => _tenDaiLy;
            set
            {
                _tenDaiLy = value;
                OnPropertyChanged();
            }
        }

        private string _soDienThoai = string.Empty;
        public string SoDienThoai
        {
            get => _soDienThoai;
            set
            {
                _soDienThoai = value;
                OnPropertyChanged();
            }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayTiepNhan;
        public DateTime NgayTiepNhan
        {
            get => _ngayTiepNhan;
            set
            {
                _ngayTiepNhan = value;
                OnPropertyChanged();
            }
        }

        private string _diaChi = string.Empty;
        public string DiaChi
        {
            get => _diaChi;
            set
            {
                _diaChi = value;
                OnPropertyChanged();
            }
        }

        private LoaiDaiLy _selectedLoaiDaiLy = new();
        public LoaiDaiLy SelectedLoaiDaiLy
        {
            get => _selectedLoaiDaiLy;
            set
            {
                _selectedLoaiDaiLy = value;
                OnPropertyChanged();
            }
        }

        private Quan _selectedQuan = new();
        public Quan SelectedQuan
        {
            get => _selectedQuan;
            set
            {
                _selectedQuan = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<LoaiDaiLy> loaiDaiLies = [];
        public ObservableCollection<LoaiDaiLy> LoaiDaiLies
        {
            get => loaiDaiLies;
            set
            {
                loaiDaiLies = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Quan> quans = [];
        public ObservableCollection<Quan> Quans
        {
            get => quans;
            set
            {
                quans = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand CloseWindowCommand { get; }
        public ICommand CapNhatDaiLyCommand { get; }

        private async Task LoadDataAsync()
        {
            var listLoaiDaiLy = await loaiDaiLyService.GetAllLoaiDaiLy();
            var listQuan = await quanService.GetAllQuan();

            LoaiDaiLies.Clear();
            Quans.Clear();
            LoaiDaiLies = [.. listLoaiDaiLy];
            Quans = [.. listQuan];

            // Load the DaiLy data
            var daiLy = await daiLyService.GetDaiLyById(_daiLyId);
            if (daiLy != null)
            {
                MaDaiLy = daiLy.MaDaiLy.ToString();
                TenDaiLy = daiLy.TenDaiLy;
                SoDienThoai = daiLy.DienThoai;
                Email = daiLy.Email;
                DiaChi = daiLy.DiaChi;
                NgayTiepNhan = daiLy.NgayTiepNhan;

                // Set selected values
                SelectedLoaiDaiLy = LoaiDaiLies.FirstOrDefault(l => l.MaLoaiDaiLy == daiLy.MaLoaiDaiLy) ?? new LoaiDaiLy();
                SelectedQuan = Quans.FirstOrDefault(q => q.MaQuan == daiLy.MaQuan) ?? new Quan();
            }
        }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<ChinhSuaDaiLyWindow>().FirstOrDefault()?.Close();
        }

        private async Task CapNhatDaiLy()
        {
            if (string.IsNullOrWhiteSpace(TenDaiLy))
            {
                MessageBox.Show("Tên đại lý không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SelectedLoaiDaiLy == null || SelectedQuan == null)
            {
                MessageBox.Show("Vui lòng chọn loại đại lý và quận!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                DaiLy existingDaiLy = await daiLyService.GetDaiLyById(_daiLyId);
                if (existingDaiLy == null)
                {
                    MessageBox.Show("Dai Ly not found");
                    return;
                }
                else
                {
                    try
                    {
                        existingDaiLy.TenDaiLy = TenDaiLy;
                        existingDaiLy.DienThoai = SoDienThoai;
                        existingDaiLy.Email = Email;
                        existingDaiLy.DiaChi = DiaChi;
                        existingDaiLy.NgayTiepNhan = NgayTiepNhan;
                        existingDaiLy.MaLoaiDaiLy = SelectedLoaiDaiLy.MaLoaiDaiLy;
                        existingDaiLy.MaQuan = SelectedQuan.MaQuan;
                        existingDaiLy.LoaiDaiLy = SelectedLoaiDaiLy;
                        existingDaiLy.Quan = SelectedQuan;

                        await daiLyService.UpdateDaiLy(existingDaiLy);
                        MessageBox.Show("Cập nhật đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Cập nhật đại lý không thành công", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SHIT", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}