using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.QuanViews;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace QuanLyDaiLy.ViewModels.QuanViewModels
{
    public class ThemQuanViewModel : INotifyPropertyChanged
    {
        private readonly IQuanService _quanService;

        public ThemQuanViewModel(IQuanService quanService)
        {
            _quanService = quanService;

            // Initialize commands
            CloseWindowCommand = new RelayCommand(CloseWindow);
            TiepNhanQuanCommand = new RelayCommand(async () => await TiepNhanQuan());
            QuanMoiCommand = new RelayCommand(QuanMoi);
        }

        // Event to notify parent view when data changes
        public event EventHandler? DataChanged;

        // Properties for binding
        private string _maQuan = string.Empty;
        public string MaQuan
        {
            get => _maQuan;
            set
            {
                _maQuan = value;
                OnPropertyChanged();
            }
        }

        private string _tenQuan = string.Empty;
        public string TenQuan
        {
            get => _tenQuan;
            set
            {
                _tenQuan = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand CloseWindowCommand { get; }
        public ICommand TiepNhanQuanCommand { get; }
        public ICommand QuanMoiCommand { get; }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<ThemQuanWindow>().FirstOrDefault()?.Close();
        }

        private async Task TiepNhanQuan()
        {
            if (string.IsNullOrWhiteSpace(TenQuan))
            {
                MessageBox.Show("Tên quận không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                MaQuan = (await _quanService.GenerateAvailableId()).ToString();

                Quan quan = new()
                {
                    MaQuan = int.Parse(MaQuan),
                    TenQuan = TenQuan
                };

                await _quanService.AddQuan(quan);
                MessageBox.Show("Thêm quận thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thêm quận không thành công", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void QuanMoi()
        {
            MaQuan = string.Empty;
            TenQuan = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}