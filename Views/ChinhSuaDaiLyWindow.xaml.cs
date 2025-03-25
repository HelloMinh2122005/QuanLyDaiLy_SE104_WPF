using System.Windows;
using QuanLyDaiLy.ViewModels;

namespace QuanLyDaiLy.Views
{
    public partial class ChinhSuaDaiLyWindow : Window
    {
        public ChinhSuaDaiLyWindow(ChinhSuaDaiLyViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
