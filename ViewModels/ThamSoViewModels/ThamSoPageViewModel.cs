using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.ThamSoViews;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace QuanLyDaiLy.ViewModels.ThamSoViewModels
{
    public partial class ThamSoPageViewModel : ObservableObject, IRecipient<DataReloadMessage>
    {
        private readonly IThamSoService _thamsoService;
        private readonly IServiceProvider _serviceProvider;
        bool isFirst = true;

        public ThamSoPageViewModel(
            IThamSoService thamSoService,
            IServiceProvider serviceProvider
        )
        {
            _thamsoService = thamSoService;
            _serviceProvider = serviceProvider;

            WeakReferenceMessenger.Default.RegisterAll(this);

            // Load tham số từ cơ sở dữ liệu
            _ = LoadDataAsync();
        }

        public void Receive(DataReloadMessage message)
        {
            _ = LoadDataAsync();
        }

        [ObservableProperty]
        private bool _quyDinhSoLuongDaiLyToiDa = true;

        [ObservableProperty]
        private bool _quyDinhTienThuTienNo = true;

        [ObservableProperty]
        private int _soLuongDaiLyToiDa = 4;


        private async Task LoadDataAsync()
        {
            try
            {
                var thamSo = await _thamsoService.GetThamSo();
                if (thamSo != null)
                {
                    QuyDinhSoLuongDaiLyToiDa = thamSo.QuyDinhSoLuongDaiLyToiDa;
                    QuyDinhTienThuTienNo = thamSo.QuyDinhTienThuTienNo;
                    SoLuongDaiLyToiDa = thamSo.SoLuongDaiLyToiDa;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi khi tải tham số từ cơ sở dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task SaveChangesToDatabase()
        {
            try
            {
                if (isFirst)
                {
                    var thamSo = await _thamsoService.GetThamSo();
                    if (thamSo != null)
                    {
                        SoLuongDaiLyToiDa = thamSo.SoLuongDaiLyToiDa;
                        QuyDinhSoLuongDaiLyToiDa = thamSo.QuyDinhSoLuongDaiLyToiDa;
                        QuyDinhTienThuTienNo = thamSo.QuyDinhTienThuTienNo;
                    }
                    isFirst = false;
                }
                else
                {
                    var thamSo = await _thamsoService.GetThamSo();

                    thamSo.QuyDinhSoLuongDaiLyToiDa = QuyDinhSoLuongDaiLyToiDa;
                    thamSo.QuyDinhTienThuTienNo = QuyDinhTienThuTienNo;
                    thamSo.SoLuongDaiLyToiDa = SoLuongDaiLyToiDa;

                    await _thamsoService.UpdateThamSo(thamSo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi khi lưu tham số vào cơ sở dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
