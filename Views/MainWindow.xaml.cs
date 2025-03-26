using QuanLyDaiLy.ViewModels;
using QuanLyDaiLy.Views.LoaiDaiLyViews;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

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
            NavigateToPage("DaiLy");
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
                //case "Dashboard":
                //    MainContent.Navigate(new DashboardPage());
                //    break;
                case "DaiLy":
                    // Instead of navigating to a new page, use the current view
                    MainContent.Content = null;
                    break;
                case "LoaiDaiLy":
                    MainContent.Navigate(new LoaiDaiLyPage());
                    break;
                //case "Quan":
                //    PageTitle.Text = "Quận";
                //    MainContent.Navigate(new QuanPage());
                //    break;
                //case "MatHang":
                //    PageTitle.Text = "Mặt hàng";
                //    MainContent.Navigate(new MatHangPage());
                //    break;
                //case "PhieuThu":
                //    PageTitle.Text = "Phiếu thu";
                //    MainContent.Navigate(new PhieuThuPage());
                //    break;
                //case "PhieuXuat":
                //    PageTitle.Text = "Phiếu xuất";
                //    MainContent.Navigate(new PhieuXuatPage());
                //    break;
                //case "DonViTinh":
                //    PageTitle.Text = "Đơn vị tính";
                //    MainContent.Navigate(new DonViTinhPage());
                //    break;
                //case "ThamSo":
                //    PageTitle.Text = "Tham số";
                //    MainContent.Navigate(new ThamSoPage());
                //    break;
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

    // Custom animation class for GridLength
    public class GridLengthAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));

        public GridLength From
        {
            get { return (GridLength)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public GridLength To
        {
            get { return (GridLength)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public override Type TargetPropertyType => typeof(GridLength);

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            // Use From value directly when provided
            double fromVal = From.Value;

            // If From is not specified (or auto), use the default value
            if (fromVal == 0 && From.GridUnitType == GridUnitType.Auto)
                fromVal = ((GridLength)defaultOriginValue).Value;

            double toVal = To.Value;
            if (toVal == 0 && To.GridUnitType == GridUnitType.Auto)
                toVal = ((GridLength)defaultDestinationValue).Value;

            if (animationClock.CurrentProgress == null)
                return new GridLength(fromVal);

            double progress = animationClock.CurrentProgress.Value;
            return new GridLength((1 - progress) * fromVal + progress * toVal, GridUnitType.Pixel);
        }
    }
}