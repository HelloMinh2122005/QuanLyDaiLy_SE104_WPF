using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyDaiLy.ViewModels.DonViTinhViewModels
{
    public class CapNhatDonViTinhPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
