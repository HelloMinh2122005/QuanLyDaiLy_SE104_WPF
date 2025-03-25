using QuanLyDaiLy.ViewModels;
using System.Windows;

namespace QuanLyDaiLy.Views
{
    /// <summary>
    /// Interaction logic for TraCuuDaiLyWindow.xaml
    /// </summary>
    public partial class TraCuuDaiLyWindow : Window
    {
        public TraCuuDaiLyWindow(TraCuuDaiLyViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
