using System.Windows;
using QuanLyDaiLy.ViewModels.PhieuXuatViewModels;

namespace QuanLyDaiLy.Views.PhieuXuatViews
{
    public partial class CapNhatPhieuXuatWindow : Window
    {
        public CapNhatPhieuXuatWindow(CapNhatPhieuXuatWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
