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
    public class ChinhSuaQuanViewModel : INotifyPropertyChanged
    {
        private readonly IQuanService _quanService;
        private readonly int _quanId;

        public ChinhSuaQuanViewModel(IQuanService quanService, int quanId)
        {
            _quanService = quanService;
            _quanId = quanId;

            CloseWindowCommand = new RelayCommand(CloseWindow);
            EditQuanCommand = new RelayCommand(async () => await CapNhatQuan());

            _ = LoadDataAsync();
        }

        public event EventHandler? DataChanged;

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

        public ICommand CloseWindowCommand { get; }
        public ICommand EditQuanCommand { get; }

        private async Task LoadDataAsync()
        {
            try
            {
                var quan = await _quanService.GetQuanById(_quanId);
                MaQuan = quan.MaQuan.ToString();
                TenQuan = quan.TenQuan;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu quận: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<CapNhatQuanWindow>().FirstOrDefault()?.Close();
        }

        private async Task CapNhatQuan()
        {
            if (string.IsNullOrWhiteSpace(TenQuan))
            {
                MessageBox.Show("Tên quận không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var existingQuan = await _quanService.GetQuanById(_quanId);

                existingQuan.TenQuan = TenQuan;

                await _quanService.UpdateQuan(existingQuan);
                MessageBox.Show("Cập nhật quận thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DataChanged?.Invoke(this, EventArgs.Empty);
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật quận: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}