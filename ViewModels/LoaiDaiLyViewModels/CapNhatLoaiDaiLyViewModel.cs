using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.QuanViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Views.LoaiDaiLyViews;

namespace QuanLyDaiLy.ViewModels.LoaiDaiLyViewModels
{
    public class CapNhatLoaiDaiLyViewModel : INotifyPropertyChanged
    {
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly int _loaiDaiLyId;

        public CapNhatLoaiDaiLyViewModel(ILoaiDaiLyService loaiDaiLyService, int loaiDaiLyId)
        {
            _loaiDaiLyService = loaiDaiLyService;
            _loaiDaiLyId = loaiDaiLyId;

            CloseWindowCommand = new RelayCommand(CloseWindow);
            CapNhatLoaiDaiLyCommand = new RelayCommand(async () => await CapNhatLoaiDaiLy());
            
            _ = LoadDataAsync();
        }

        public event EventHandler? DataChanged;

        private string _maLoaiDaiLy = string.Empty;
        public string MaLoaiDaiLy
        {
            get => _maLoaiDaiLy;
            set
            {
                _maLoaiDaiLy = value;
                OnPropertyChanged();
            }
        }

        private string _tenLoaiDaiLy = string.Empty;
        public string TenLoaiDaiLy
        {
            get => _tenLoaiDaiLy;
            set
            {
                _tenLoaiDaiLy = value;
                OnPropertyChanged();
            }
        }

        private string _noToiDa = string.Empty;
        public string NoToiDa
        {
            get => _noToiDa;
            set
            {
                _noToiDa = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseWindowCommand { get; }
        public ICommand CapNhatLoaiDaiLyCommand { get; }

        private async Task LoadDataAsync()
        {
            try
            {
                var loaiDaiLy = await _loaiDaiLyService.GetLoaiDaiLyById(_loaiDaiLyId);
                MaLoaiDaiLy = loaiDaiLy.MaLoaiDaiLy.ToString();
                TenLoaiDaiLy = loaiDaiLy.TenLoaiDaiLy;
                NoToiDa = loaiDaiLy.NoToiDa.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<CapNhatLoaiDaiLyWindow>().FirstOrDefault()?.Close();
        }

        private async Task CapNhatLoaiDaiLy()
        {
            if (string.IsNullOrWhiteSpace(TenLoaiDaiLy))
            {
                MessageBox.Show("Tên loại đại lý không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(NoToiDa))
            {
                MessageBox.Show("Nợ tối đa không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var existingLoaiDaiLy = await _loaiDaiLyService.GetLoaiDaiLyById(_loaiDaiLyId);

                existingLoaiDaiLy.TenLoaiDaiLy = TenLoaiDaiLy;
                existingLoaiDaiLy.NoToiDa = long.Parse(NoToiDa);

                await _loaiDaiLyService.UpdateLoaiDaiLy(existingLoaiDaiLy);
                MessageBox.Show("Cập nhật loại đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DataChanged?.Invoke(this, EventArgs.Empty);
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}