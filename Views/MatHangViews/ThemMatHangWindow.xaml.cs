using System.Windows;
using QuanLyDaiLy.ViewModels.MatHangViewModels;

namespace QuanLyDaiLy.Views.MatHangViews
{
    public partial class ThemMatHangWindow : Window
    {
        public ThemMatHangWindow(ThemMatHangWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
