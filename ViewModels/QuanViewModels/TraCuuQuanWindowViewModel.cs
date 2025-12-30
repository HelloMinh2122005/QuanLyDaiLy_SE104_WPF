using System.Windows;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Helpers;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.QuanViews;

namespace QuanLyDaiLy.ViewModels.QuanViewModels
{
    public partial class TraCuuQuanWindowViewModel : ObservableObject
    {
        private readonly IQuanService _quanService;

        public TraCuuQuanWindowViewModel(IQuanService quanService)
        {
            _quanService = quanService;

            _ = LoadDataAsync();
        }

        [ObservableProperty]
        private string _maQuan = string.Empty;

        [ObservableProperty]
        private string _tenQuan = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Quan> _quans = [];

        public ObservableCollection<Quan> SearchResults = [];

        private async Task LoadDataAsync()
        {
            try
            {
                var listQuan = await _quanService.GetAllQuan();

                Quans.Clear();
                Quans = [.. listQuan];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Close()
        {
            Application.Current.Windows.OfType<TraCuuQuanWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task TraCuuQuan()
        {
            try
            {
                var filteredResults = await _quanService.GetAllQuan();

                if (!string.IsNullOrEmpty(MaQuan))
                {
                    filteredResults = filteredResults.Where(d => d.MaQuan.ToString().Contains(MaQuan));
                }

                if (!string.IsNullOrEmpty(TenQuan))
                {
                    var ten = StringHelper.RemoveDiacritics(TenQuan.Trim().ToLower());
                    filteredResults = filteredResults.Where(d => StringHelper.RemoveDiacritics(d.TenQuan.ToLower()).Contains(ten));
                }

                SearchResults = [.. filteredResults];

                ApplySearchResults();

                if (SearchResults.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào phù hợp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplySearchResults()
        {
            WeakReferenceMessenger.Default.Send(new SearchCompletedMessage<Quan>(SearchResults));
            Close();
        }
    }
}
