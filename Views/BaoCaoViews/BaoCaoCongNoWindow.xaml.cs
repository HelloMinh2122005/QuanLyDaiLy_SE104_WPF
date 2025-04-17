using System.Windows;
using QuanLyDaiLy.ViewModels.BaoCaoViewModels;

namespace QuanLyDaiLy.Views.BaoCaoViews
{
    public partial class BaoCaoCongNoWindow : Window
    {
        public BaoCaoCongNoWindow(BaoCaoCongNoViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
