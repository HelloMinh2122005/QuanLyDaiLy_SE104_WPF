// ViewModels/HoSoDaiLyViewModel.cs
using QuanLyDaiLy.Views;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyDaiLy.ViewModels
{
    public class HoSoDaiLyViewModel : INotifyPropertyChanged
    {
        public HoSoDaiLyViewModel()
        {
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }

        public ICommand CloseWindowCommand { get; }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<HoSoDaiLyWinDow>().FirstOrDefault()?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}