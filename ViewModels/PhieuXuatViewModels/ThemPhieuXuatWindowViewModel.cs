using System.ComponentModel;

namespace QuanLyDaiLy.ViewModels.PhieuXuatViewModels
{
    public class ThemPhieuXuatWindowViewModel : INotifyPropertyChanged
    {
        // Event to notify parent view when data changes
        public event EventHandler? DataChanged;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
