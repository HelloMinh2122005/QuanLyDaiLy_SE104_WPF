using System.Windows;
using QuanLyDaiLy.ViewModels.BaoCaoViewModels;

namespace QuanLyDaiLy.Views.BaoCaoViews
{
    public partial class BaoCaoDoanhSoWindow : Window
    {
        public BaoCaoDoanhSoWindow(BaoCaoDoanhSoViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
