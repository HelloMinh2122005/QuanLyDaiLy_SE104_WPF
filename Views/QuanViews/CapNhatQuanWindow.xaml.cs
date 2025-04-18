using System.Windows;
using QuanLyDaiLy.ViewModels.QuanViewModels;

namespace QuanLyDaiLy.Views.QuanViews
{
    public partial class CapNhatQuanWindow : Window
    {
        public CapNhatQuanWindow(ChinhSuaQuanViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;   
        }
    }
}
