using System.Windows;
using QuanLyDaiLy.ViewModels.DonViTinhViewModels;

namespace QuanLyDaiLy.Views.DonViTinhViews
{
    public partial class CapNhatDonViTinhWindow : Window
    {
        public CapNhatDonViTinhWindow(CapNhatDonViTinhPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
