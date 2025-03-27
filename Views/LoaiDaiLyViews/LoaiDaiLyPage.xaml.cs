using QuanLyDaiLy.ViewModels.LoaiDaiLyViewModels;
using System.Windows.Controls;

namespace QuanLyDaiLy.Views.LoaiDaiLyViews
{
    public partial class LoaiDaiLyPage : Page
    {
        public LoaiDaiLyPage(LoaiDaiLyPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
