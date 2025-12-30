using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.QuanViews;
using QuanLyDaiLy.Messages;
using System.Windows;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Views.MatHangViews;

namespace QuanLyDaiLy.ViewModels.QuanViewModels
{
    public partial class ThemQuanViewModel : ObservableObject
    {
        private readonly IQuanService _quanService;

        public ThemQuanViewModel(IQuanService quanService)
        {
            _quanService = quanService;
        }

        // Binding properties with ObservableProperty attribute
        [ObservableProperty]
        private string _maQuan = "";

        [ObservableProperty]
        private string _tenQuan = "";

        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<ThemQuanWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task TiepNhanQuan()
        {
            if (string.IsNullOrWhiteSpace(TenQuan))
            {
                MessageBox.Show("Tên quận không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Thêm quận thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm quận: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void QuanMoi()
        {
            try
            {
                MaQuan = string.Empty;
                TenQuan = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo quận mới: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}