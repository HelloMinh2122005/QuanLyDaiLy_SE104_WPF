using System.Windows;
using QuanLyDaiLy.ViewModels.QuanViewModels;

namespace QuanLyDaiLy.Views.QuanViews
{
    public partial class TraCuuQuanWindow : Window
    {
        public TraCuuQuanWindow(TraCuuQuanWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
