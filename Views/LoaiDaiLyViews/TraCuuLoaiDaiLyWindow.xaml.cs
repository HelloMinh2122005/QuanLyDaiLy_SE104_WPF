using QuanLyDaiLy.ViewModels.LoaiDaiLyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLyDaiLy.Views.LoaiDaiLyViews
{
    /// <summary>
    /// Interaction logic for TraCuuLoaiDaiLyWindow.xaml
    /// </summary>
    public partial class TraCuuLoaiDaiLyWindow : Window
    {
        public TraCuuLoaiDaiLyWindow(TraCuuLoaiDaiLyWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
