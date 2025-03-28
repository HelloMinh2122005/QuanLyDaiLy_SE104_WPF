using QuanLyDaiLy.ViewModels.DashboardViewModels;
using System.Windows.Controls;

namespace QuanLyDaiLy.Views.DashboardViews
{
    public partial class DashboardPage : Page
    {
        public DashboardPage(DashboardPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
