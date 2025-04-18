using System.Windows;
using QuanLyDaiLy.ViewModels.MatHangViewModels;

namespace QuanLyDaiLy.Views.MatHangViews
{
    public partial class CapNhatMatHangWindow : Window
    {
        public CapNhatMatHangWindow(CapNhatMatHangWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
