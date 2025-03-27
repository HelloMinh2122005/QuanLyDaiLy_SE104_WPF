using QuanLyDaiLy.ViewModels.QuanViewModels;
using System.Windows;

namespace QuanLyDaiLy.Views.QuanViews
{
    public partial class ThemQuanWindow : Window
    {
        public ThemQuanWindow(ThemQuanViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
