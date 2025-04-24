using System.Windows;
using QuanLyDaiLy.ViewModels.MatHangViewModels;

namespace QuanLyDaiLy.Views.MatHangViews
{
    /// <summary>
    /// Interaction logic for TraCuuMatHangWindow.xaml
    /// </summary>
    public partial class TraCuuMatHangWindow : Window
    {
        public TraCuuMatHangWindow(TraCuuMatHangWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
