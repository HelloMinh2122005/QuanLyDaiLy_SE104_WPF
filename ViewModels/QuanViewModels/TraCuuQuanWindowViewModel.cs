using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.QuanViews;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.DirectoryServices;

namespace QuanLyDaiLy.ViewModels.QuanViewModels
{
    public class TraCuuQuanWindowViewModel : INotifyPropertyChanged
    {
        private readonly IQuanService _quanService;

        public TraCuuQuanWindowViewModel(IQuanService quanService)
        {
            _quanService = quanService;

            // Initialize commands
            CloseCommand = new RelayCommand(CloseWindow);
            TraCuuQuanCommand = new RelayCommand(async () => await TraCuuQuan());

            _ = LoadDataAsync();
        }

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

        private ObservableCollection<Quan> _quans = [];
        public ObservableCollection<Quan> Quans
        {
            get => _quans;
            set
            {
                _quans = value;
                OnPropertyChanged();
            }
        }

        // Search Results
        private ObservableCollection<Quan> _searchResults = [];
        public ObservableCollection<Quan> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand CloseCommand { get; }
        public ICommand TraCuuQuanCommand { get; }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<TraCuuQuanWindow>().FirstOrDefault()?.Close();
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

        private async Task TraCuuQuan()
        {
            try
            {
                var quans = await _quanService.GetAllQuan();

                ObservableCollection<Quan> filteredResults = [.. quans];

                if (!string.IsNullOrEmpty(MaQuan))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaQuan.ToString().Contains(MaQuan))];
                }
                if (!string.IsNullOrEmpty(TenQuan))
                {
                    var ten = RemoveDiacritics(TenQuan.Trim().ToLower());
                    filteredResults = [.. filteredResults.Where(d => RemoveDiacritics(d.TenQuan.ToLower()).Contains(ten))];
                }

                SearchResults = [.. filteredResults];

                // Raise the event with the search results
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

        public event EventHandler<ObservableCollection<Quan>>? SearchCompleted;

        private void ApplySearchResults()
        {
            // Trigger the event with current search results
            SearchCompleted?.Invoke(this, SearchResults);

            // Close the window after applying
            CloseWindow();
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


        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? DataChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
