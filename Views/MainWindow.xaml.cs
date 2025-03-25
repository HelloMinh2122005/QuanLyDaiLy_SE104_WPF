using QuanLyDaiLy.ViewModels;
using System.Windows;

namespace QuanLyDaiLy.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            DataContext = viewModel;
        }
    }
}
