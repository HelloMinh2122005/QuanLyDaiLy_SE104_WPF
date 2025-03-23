using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QuanLyDaiLy.ViewModels;

namespace QuanLyDaiLy.Views
{
    /// <summary>
    /// Interaction logic for ChinhSuaDaiLyWindow.xaml
    /// </summary>
    public partial class ChinhSuaDaiLyWindow : Window
    {
        public ChinhSuaDaiLyWindow(ChinhSuaDaiLyViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
