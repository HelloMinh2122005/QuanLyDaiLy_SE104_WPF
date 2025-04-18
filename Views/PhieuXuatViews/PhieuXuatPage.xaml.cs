using System.Windows.Controls;
using QuanLyDaiLy.ViewModels.PhieuXuatViewModels;

namespace QuanLyDaiLy.Views.PhieuXuatViews
{
    public partial class PhieuXuatPage : Page
    {
        public PhieuXuatPage(PhieuXuatPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
