using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.LoaiDaiLyViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace QuanLyDaiLy.ViewModels.LoaiDaiLyViewModels
{
    public class ThemLoaiDaiLyViewModel : INotifyPropertyChanged
    {
        private readonly ILoaiDaiLyService _loaiDaiLyService;

        public ThemLoaiDaiLyViewModel(ILoaiDaiLyService loaiDaiLyService)
        {
            _loaiDaiLyService = loaiDaiLyService;

            // Initialize commands
            CloseWindowCommand = new RelayCommand(CloseWindow);
            TiepNhanLoaiDaiLyCommand = new RelayCommand(async () => await TiepNhanLoaiDaiLy());
            LoaiDaiLyMoiCommand = new RelayCommand(LoaiDaiLyMoi);
        }

        // Event to notify parent view when data changes
        public event EventHandler? DataChanged;

        // Properties for binding
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

        // Commands
        public ICommand CloseWindowCommand { get; }
        public ICommand TiepNhanLoaiDaiLyCommand { get; }
        public ICommand LoaiDaiLyMoiCommand { get; }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<ThemLoaiDaiLyWindow>().FirstOrDefault()?.Close();
        }

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
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thêm loại đại lý không thành công", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoaiDaiLyMoi()
        {
            MaLoaiDaiLy = string.Empty;
            TenLoaiDaiLy = string.Empty;
            NoToiDa = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}