using QuanLyDaiLy.ViewModels;
using System.Windows;

namespace QuanLyDaiLy.Views
{
    public partial class TraCuuDaiLyWindow : Window
    {
        public TraCuuDaiLyWindow(TraCuuDaiLyViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
