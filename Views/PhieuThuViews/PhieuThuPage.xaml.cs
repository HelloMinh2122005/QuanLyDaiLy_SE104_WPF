using QuanLyDaiLy.ViewModels.PhieuThuViewModels;
using System.Windows.Controls;

namespace QuanLyDaiLy.Views.PhieuThuViews
{
    public partial class PhieuThuPage : Page
    {
        public PhieuThuPage(PhieuThuPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
