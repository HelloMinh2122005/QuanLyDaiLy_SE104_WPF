using System.Windows;
using QuanLyDaiLy.ViewModels.PhieuXuatViewModels;

namespace QuanLyDaiLy.Views.PhieuXuatViews
{
    public partial class TraCuuPhieuXuatWindow : Window
    {
        public TraCuuPhieuXuatWindow(TraCuuPhieuXuatWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
