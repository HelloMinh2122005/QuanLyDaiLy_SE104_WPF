using QuanLyDaiLy.ViewModels.MatHangViewModels;
using System.Windows.Controls;

namespace QuanLyDaiLy.Views.MatHangViews
{
    public partial class MatHangPage : Page
    {
        public MatHangPage(MatHangPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
