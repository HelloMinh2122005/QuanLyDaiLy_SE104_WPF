using System.Windows;
using QuanLyDaiLy.ViewModels.DonViTinhViewModels;

namespace QuanLyDaiLy.Views.DonViTinhViews
{
    public partial class ThemDonViTinhWindow : Window
    {
        public ThemDonViTinhWindow(ThemDonViTinhPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
