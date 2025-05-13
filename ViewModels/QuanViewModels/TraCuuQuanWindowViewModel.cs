//using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.QuanViews;
using System.Windows;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;

namespace QuanLyDaiLy.ViewModels.QuanViewModels
{
    public partial class TraCuuQuanWindowViewModel : ObservableObject
    {
        private readonly IQuanService _quanService;

        [ObservableProperty]
        private string _maQuan = string.Empty;

        [ObservableProperty]
        private string _tenQuan = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Quan> _quans = [];

        public ObservableCollection<Quan> SearchResults = [];

        public TraCuuQuanWindowViewModel(IQuanService quanService)
        {
            _quanService = quanService;

            _ = LoadDataAsync();
        }

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
                    var ten = RemoveDiacritics(TenQuan.Trim().ToLower());
                    filteredResults = [.. filteredResults.Where(d => RemoveDiacritics(d.TenQuan.ToLower()).Contains(ten))];

                    // filteredResults = filteredResults.Where(d => d.TenQuan.Normalize().Contains(TenQuan.Normalize()));
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

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
            var builder = new System.Text.StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

        private void ApplySearchResults()
        {
            WeakReferenceMessenger.Default.Send(new SearchCompletedMessage<Quan>(SearchResults));
            Close();
        }
    }
}
