using QuanLyDaiLy.ViewModels.ThamSoViewModels;
using System.Windows.Controls;

namespace QuanLyDaiLy.Views.ThamSoViews
{
    public partial class ThamSoPage : Page
    {
        public ThamSoPage(ThamSoPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
