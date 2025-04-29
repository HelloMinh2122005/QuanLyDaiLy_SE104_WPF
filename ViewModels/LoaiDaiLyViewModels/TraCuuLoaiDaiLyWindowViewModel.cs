using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.LoaiDaiLyViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace QuanLyDaiLy.ViewModels.LoaiDaiLyViewModels
{
    public class TraCuuLoaiDaiLyWindowViewModel : INotifyPropertyChanged
    {
        private readonly ILoaiDaiLyService _loaiDaiLyService;

        public TraCuuLoaiDaiLyWindowViewModel(ILoaiDaiLyService loaiDaiLyService)
        {
            _loaiDaiLyService = loaiDaiLyService;

            // Initialize commands
            CloseCommand = new RelayCommand(CloseWindow);
            TraCuuLoaiDaiLyCommand = new RelayCommand(async () => await TraCuuLoaiDaiLy());

            _ = LoadDataAsync();
        }

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

        private string _noDaiLyFrom = string.Empty;
        public string NoDaiLyFrom
        {
            get => _noDaiLyFrom;
            set
            {
                _noDaiLyFrom = value;
                OnPropertyChanged();
            }
        }

        private string _noDaiLyTo = string.Empty;
        public string NoDaiLyTo
        {
            get => _noDaiLyTo;
            set
            {
                _noDaiLyTo = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<LoaiDaiLy> _loaiDaiLies = [];
        public ObservableCollection<LoaiDaiLy> LoaiDaiLies
        {
            get => _loaiDaiLies;
            set
            {
                _loaiDaiLies = value;
                OnPropertyChanged();
            }
        }

        // Search Results
        private ObservableCollection<LoaiDaiLy> _searchResults = [];
        public ObservableCollection<LoaiDaiLy> SearchResults
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
        public ICommand TraCuuLoaiDaiLyCommand { get; }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<TraCuuLoaiDaiLyWindow>().FirstOrDefault()?.Close();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();

                LoaiDaiLies.Clear();
                LoaiDaiLies = [.. listLoaiDaiLy];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task TraCuuLoaiDaiLy()
        {
            try
            {
                var loaiDaiLies = await _loaiDaiLyService.GetAllLoaiDaiLy();

                ObservableCollection<LoaiDaiLy> filteredResults = [.. loaiDaiLies];

                if (!string.IsNullOrEmpty(MaLoaiDaiLy))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaLoaiDaiLy.ToString().Contains(MaLoaiDaiLy))];
                }
                if (!string.IsNullOrEmpty(TenLoaiDaiLy))
                {
                    var ten = RemoveDiacritics(TenLoaiDaiLy.Trim().ToLower());
                    filteredResults = [.. filteredResults.Where(d => RemoveDiacritics(d.TenLoaiDaiLy.ToLower()).Contains(ten))];
                }
                if (!string.IsNullOrEmpty(NoDaiLyFrom))
                {
                    if (int.TryParse(NoDaiLyFrom, out int noFrom))
                    {
                        filteredResults = [.. filteredResults.Where(d => d.NoToiDa >= noFrom)];
                    }
                }
                if (!string.IsNullOrEmpty(NoDaiLyTo))
                {
                    if (int.TryParse(NoDaiLyTo, out int noTo))
                    {
                        filteredResults = [.. filteredResults.Where(d => d.NoToiDa <= noTo)];
                    }
                }


                SearchResults = [.. filteredResults];

                // Raise the event with the search results
                SearchCompleted?.Invoke(this, SearchResults);
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

        public event EventHandler<ObservableCollection<LoaiDaiLy>>? SearchCompleted;

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
