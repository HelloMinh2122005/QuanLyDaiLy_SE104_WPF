using QuanLyDaiLy.ViewModels.PhieuThuViewModels;
using System.Windows.Controls;

namespace QuanLyDaiLy.Views.PhieuXuatViews
{
    public partial class PhieuXuatPage : Page
    {
        public PhieuXuatPage(PhieuThuPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
