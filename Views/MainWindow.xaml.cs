using QuanLyDaiLy.ViewModels;
using QuanLyDaiLy.Views.LoaiDaiLyViews;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuanLyDaiLy.Views.CustomAnimation;
using QuanLyDaiLy.Views.QuanViews;
using QuanLyDaiLy.Views.DashboardViews;
using QuanLyDaiLy.Views.PhieuThuViews;
using QuanLyDaiLy.Views.PhieuXuatViews;
using QuanLyDaiLy.Views.DonViTinhViews;
using QuanLyDaiLy.Views.ThamSoViews;
using QuanLyDaiLy.Views.MatHangViews;

namespace QuanLyDaiLy.Views
{
    public partial class MainWindow : Window
    {
        private readonly double collapsedWidth = 60;
        private readonly double expandedWidth = 200;

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            DataContext = viewModel;

            // Set initial width
            NavColumn.Width = new GridLength(collapsedWidth);
            NavigateToPage("Dashboard");
        }

        private void NavigationRail_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimateNavDrawerWidth(expandedWidth);
        }

        private void NavigationRail_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimateNavDrawerWidth(collapsedWidth);
        }

        private void AnimateNavDrawerWidth(double targetWidth)
        {
            // Create a duration
            var duration = new Duration(TimeSpan.FromMilliseconds(300));

            // Get the current width value as the starting point
            double currentWidth = NavColumn.ActualWidth;

            // Create animation for grid column width
            var animation = new GridLengthAnimation
            {
                Duration = duration,
                From = new GridLength(currentWidth),
                To = new GridLength(targetWidth)
            };

            // Apply the animation to the grid column
            NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                string? pageName = radioButton.Tag as string;
                if (pageName != null)
                {
                    NavigateToPage(pageName);
                }
            }
        }

        private void NavigateToPage(string pageName)
        {
            // Update page title and content based on selected navigation item
            switch (pageName)
            {
                case "Dashboard":
                    MainContent.Visibility = Visibility.Collapsed;
                    MainContent.Visibility = Visibility.Visible;
                    MainContent.Navigate(new DashboardPage());
                    break;
                case "DaiLy":
                    // Show main content, hide frame
                    MainContent.Visibility = Visibility.Visible;
                    MainContent.Visibility = Visibility.Collapsed;
                    MainContent.Content = null;
                    break;
                case "LoaiDaiLy":
                    // Hide main content, show frame
                    MainContent.Visibility = Visibility.Collapsed;
                    MainContent.Visibility = Visibility.Visible;
                    MainContent.Navigate(new LoaiDaiLyPage());
                    break;
                case "Quan":
                    MainContent.Visibility = Visibility.Collapsed;
                    MainContent.Visibility = Visibility.Visible;
                    MainContent.Navigate(new QuanPage());
                    break;
                case "MatHang":
                    MainContent.Visibility = Visibility.Collapsed;
                    MainContent.Visibility = Visibility.Visible;
                    MainContent.Navigate(new MatHangPage());
                    break;
                case "PhieuThu":
                    MainContent.Visibility = Visibility.Collapsed;
                    MainContent.Visibility = Visibility.Visible;
                    MainContent.Navigate(new PhieuThuPage());
                    break;
                case "PhieuXuat":
                    MainContent.Visibility = Visibility.Collapsed;
                    MainContent.Visibility = Visibility.Visible;
                    MainContent.Navigate(new PhieuXuatPage());
                    break;
                case "DonViTinh":
                    MainContent.Visibility = Visibility.Collapsed;
                    MainContent.Visibility = Visibility.Visible;
                    MainContent.Navigate(new DonViTinhPage());
                    break;
                case "ThamSo":
                    MainContent.Visibility = Visibility.Collapsed;
                    MainContent.Visibility = Visibility.Visible;
                    MainContent.Navigate(new ThamSoPage());
                    break;
                default:
                    break;
            }
        }
        private void MainContent_ContentRendered(object sender, EventArgs e)
        {
            // Make sure navigation buttons reflect current page
            // This ensures sync if navigation happens programmatically
        }
    }
}