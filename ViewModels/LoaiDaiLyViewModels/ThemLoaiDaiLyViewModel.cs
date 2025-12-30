using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.LoaiDaiLyViews;

namespace QuanLyDaiLy.ViewModels.LoaiDaiLyViewModels
{
    public partial class ThemLoaiDaiLyViewModel : ObservableObject
    {
        private readonly ILoaiDaiLyService _loaiDaiLyService;

        public ThemLoaiDaiLyViewModel(ILoaiDaiLyService loaiDaiLyService)
        {
            _loaiDaiLyService = loaiDaiLyService;
        }

        // Binding properties with ObservableProperty attribute
        [ObservableProperty]
        private string _maLoaiDaiLy = "";

        [ObservableProperty]
        private string _tenLoaiDaiLy = "";

        [ObservableProperty]
        private string _noToiDa = "";

        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<ThemLoaiDaiLyWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task TiepNhanLoaiDaiLy()
        {
            if (string.IsNullOrWhiteSpace(TenLoaiDaiLy))
            {
                MessageBox.Show("Tên loại đại lý không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(NoToiDa))
            {
                MessageBox.Show("Số nợ tối đa không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                MaLoaiDaiLy = (await _loaiDaiLyService.GenerateAvailableId()).ToString();

                LoaiDaiLy loaiDaiLy = new()
                {
                    MaLoaiDaiLy = int.Parse(MaLoaiDaiLy),
                    TenLoaiDaiLy = TenLoaiDaiLy,
                    NoToiDa = int.Parse(NoToiDa)
                };

                await _loaiDaiLyService.AddLoaiDaiLy(loaiDaiLy);
                MessageBox.Show("Thêm loại đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void LoaiDaiLyMoi()
        {
            try
            {
                MaLoaiDaiLy = string.Empty;
                TenLoaiDaiLy = string.Empty;
                NoToiDa = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo loại đại lý mới: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}