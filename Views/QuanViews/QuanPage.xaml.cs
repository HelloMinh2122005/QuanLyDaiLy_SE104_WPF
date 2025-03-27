using QuanLyDaiLy.ViewModels.QuanViewModels;
using System.Windows.Controls;

namespace QuanLyDaiLy.Views.QuanViews
{
    public partial class QuanPage : Page
    {
        public QuanPage(QuanPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
