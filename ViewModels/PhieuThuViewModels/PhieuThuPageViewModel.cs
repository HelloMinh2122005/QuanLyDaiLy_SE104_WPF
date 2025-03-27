using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyDaiLy.ViewModels.PhieuThuViewModels
{
    public class PhieuThuPageViewModel : INotifyPropertyChanged
    {
        private string _hello = "Hello";
        public string Hello
        {
            get => _hello;
            set
            {
                _hello = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
