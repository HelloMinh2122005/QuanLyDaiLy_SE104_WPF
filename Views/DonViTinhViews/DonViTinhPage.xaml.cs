using QuanLyDaiLy.ViewModels.DonViTinhViewModels;
using System.Windows.Controls;

namespace QuanLyDaiLy.Views.DonViTinhViews
{
    public partial class DonViTinhPage : Page
    {
        public DonViTinhPage(DonViTinhPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
