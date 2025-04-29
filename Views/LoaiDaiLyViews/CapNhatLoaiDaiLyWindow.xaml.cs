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
    /// Interaction logic for CapNhatLoaiDaiLyWindow.xaml
    /// </summary>
    public partial class CapNhatLoaiDaiLyWindow : Window
    {
        public CapNhatLoaiDaiLyWindow(CapNhatLoaiDaiLyViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
