using System.Windows;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Helpers;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.LoaiDaiLyViews;

namespace QuanLyDaiLy.ViewModels.LoaiDaiLyViewModels
{
    public partial class TraCuuLoaiDaiLyWindowViewModel : ObservableObject
    {
        private readonly ILoaiDaiLyService _loaiDaiLyService;

        public TraCuuLoaiDaiLyWindowViewModel(ILoaiDaiLyService loaiDaiLyService)
        {
            _loaiDaiLyService = loaiDaiLyService;

            //// Initialize commands
            //CloseCommand = new RelayCommand(CloseWindow);
            //TraCuuLoaiDaiLyCommand = new RelayCommand(async () => await TraCuuLoaiDaiLy());

            _ = LoadDataAsync();
        }

        [ObservableProperty]
        private string _maLoaiDaiLy = string.Empty;

        [ObservableProperty]
        private string _tenLoaiDaiLy = string.Empty;

        [ObservableProperty]
        private string _noDaiLyFrom = string.Empty;

        [ObservableProperty]
        private string _noDaiLyTo = string.Empty;

        [ObservableProperty]
        private ObservableCollection<LoaiDaiLy> _loaiDaiLies = [];

        public ObservableCollection<LoaiDaiLy> SearchResults = [];

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

        [RelayCommand]
        private void Close()
        {
            Application.Current.Windows.OfType<TraCuuLoaiDaiLyWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task TraCuuLoaiDaiLy()
        {
            try
            {
                var filteredResults = await _loaiDaiLyService.GetAllLoaiDaiLy();

                if (!string.IsNullOrEmpty(MaLoaiDaiLy))
                {
                    filteredResults = filteredResults.Where(d => d.MaLoaiDaiLy.ToString().Contains(MaLoaiDaiLy));
                }

                if (!string.IsNullOrEmpty(TenLoaiDaiLy))
                {
                    var ten = StringHelper.RemoveDiacritics(TenLoaiDaiLy.Trim().ToLower());
                    filteredResults = filteredResults.Where(d => StringHelper.RemoveDiacritics(d.TenLoaiDaiLy.ToLower()).Contains(ten));
                }

                if (!string.IsNullOrEmpty(NoDaiLyFrom))
                {
                    if (int.TryParse(NoDaiLyFrom, out int noFrom))
                    {
                        filteredResults = filteredResults.Where(d => d.NoToiDa >= noFrom);
                    }
                }

                if (!string.IsNullOrEmpty(NoDaiLyTo))
                {
                    if (int.TryParse(NoDaiLyTo, out int noTo))
                    {
                        filteredResults = filteredResults.Where(d => d.NoToiDa <= noTo);
                    }
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
            WeakReferenceMessenger.Default.Send(new SearchCompletedMessage<LoaiDaiLy>(SearchResults));
            Close();
        }
    }
}
