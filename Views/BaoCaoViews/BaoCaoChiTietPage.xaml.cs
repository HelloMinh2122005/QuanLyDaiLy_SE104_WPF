using System.Windows.Controls;
using QuanLyDaiLy.ViewModels.BaoCaoViewModels;

namespace QuanLyDaiLy.Views.BaoCaoViews
{
    public partial class BaoCaoChiTietPage : Page
    {
        public BaoCaoChiTietPage(BaoCaoChiTietViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
