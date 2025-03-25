using QuanLyDaiLy.ViewModels;
using System.Windows;

namespace QuanLyDaiLy.Views
{
    public partial class HoSoDaiLyWinDow : Window
    {
        public HoSoDaiLyWinDow(HoSoDaiLyViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
