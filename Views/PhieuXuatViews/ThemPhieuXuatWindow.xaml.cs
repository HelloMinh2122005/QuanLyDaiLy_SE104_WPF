using System.Windows;
using QuanLyDaiLy.ViewModels.PhieuXuatViewModels;

namespace QuanLyDaiLy.Views.PhieuXuatViews
{
    public partial class ThemPhieuXuatWindow : Window
    {
        public ThemPhieuXuatWindow(ThemPhieuXuatWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
