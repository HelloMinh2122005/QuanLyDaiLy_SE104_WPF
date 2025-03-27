using QuanLyDaiLy.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuanLyDaiLy.Views.CustomAnimation;
using Microsoft.Extensions.DependencyInjection;

namespace QuanLyDaiLy.Views
{
    public partial class MainWindow : Window
    {
        private readonly double collapsedWidth = 60;
        private readonly double expandedWidth = 200;
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(MainWindowViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            DataContext = viewModel;

            // configure the window
            WindowState = WindowState.Maximized;

            NavColumn.Width = new GridLength(collapsedWidth);

            Loaded += (s, e) =>
            {
                NavigateToPage("Dashboard");
            };
        }

        private void NavigationRail_MouseEnter(object sender, MouseEventArgs e)
        {
            // Start the expand animation
            var expandStoryboard = (System.Windows.Media.Animation.Storyboard)FindResource("ExpandNavRail");
            expandStoryboard.Begin();
        }

        private void NavigationRail_MouseLeave(object sender, MouseEventArgs e)
        {
            // Start the collapse animation
            var collapseStoryboard = (System.Windows.Media.Animation.Storyboard)FindResource("CollapseNavRail");
            collapseStoryboard.Begin();
        }

        private void AnimateNavDrawerWidth(double targetWidth)
        {
            var duration = new Duration(TimeSpan.FromMilliseconds(300));

            double currentWidth = NavColumn.ActualWidth;

            var animation = new GridLengthAnimation
            {
                Duration = duration,
                From = new GridLength(currentWidth),
                To = new GridLength(targetWidth)
            };

            NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton.Tag is string pageName)
                {
                    NavigateToPage(pageName);
                }
            }
        }

        private void NavigateToPage(string pageName)
        {
            MainContent.Visibility = Visibility.Collapsed;

            switch (pageName)
            {
                case "Dashboard":
                    var dashboardPage = _serviceProvider.GetRequiredService<DashboardViews.DashboardPage>();
                    MainContent.Navigate(dashboardPage);
                    break;
                case "DaiLy":
                    MainContent.Content = null;
                    break;
                case "LoaiDaiLy":
                    var loaiDaiLyPage = _serviceProvider.GetRequiredService<LoaiDaiLyViews.LoaiDaiLyPage>();
                    MainContent.Navigate(loaiDaiLyPage);
                    break;
                case "Quan":
                    var quanPage = _serviceProvider.GetRequiredService<QuanViews.QuanPage>();
                    MainContent.Navigate(quanPage);
                    break;
                case "MatHang":
                    var matHangPage = _serviceProvider.GetRequiredService<MatHangViews.MatHangPage>();
                    MainContent.Navigate(matHangPage);
                    break;
                case "PhieuThu":
                    var phieuThuPage = _serviceProvider.GetRequiredService<PhieuThuViews.PhieuThuPage>();
                    MainContent.Navigate(phieuThuPage);
                    break;
                case "PhieuXuat":
                    var phieuXuatPage = _serviceProvider.GetRequiredService<PhieuXuatViews.PhieuXuatPage>();
                    MainContent.Navigate(phieuXuatPage);
                    break;
                case "DonViTinh":
                    var donViTinhPage = _serviceProvider.GetRequiredService<DonViTinhViews.DonViTinhPage>();
                    MainContent.Navigate(donViTinhPage);
                    break;
                case "ThamSo":
                    var thamSoPage = _serviceProvider.GetRequiredService<ThamSoViews.ThamSoPage>();
                    MainContent.Navigate(thamSoPage);
                    break;
                default:
                    break;
            }

            // Now set it back to visible
            MainContent.Visibility = Visibility.Visible;

            // Re-apply Z-index fixes after navigation
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
            }, System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void MainContent_ContentRendered(object sender, EventArgs e)
        {
            // Make sure navigation buttons reflect current page
            // This ensures sync if navigation happens programmatically
        }
    }
}