using QuanLyDaiLy.ViewModels.DashboardViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuanLyDaiLy.Views.DashboardViews
{
    public partial class DashboardPage : Page
    {
        public DashboardPage(DashboardPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void HideBorderButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that was clicked
            Button button = sender as Button;

            // Find the parent border to hide
            if (button != null)
            {
                // Navigate up the visual tree to find the parent Border
                FrameworkElement parent = button;
                while (parent != null && !(parent is Border))
                {
                    parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
                }

                // Hide the border if found
                if (parent is Border border)
                {
                    border.Visibility = Visibility.Collapsed;

                    // Find the parent StackPanel containing all borders
                    FrameworkElement currentElement = border;
                    StackPanel parentStackPanel = null;

                    while (currentElement != null && parentStackPanel == null)
                    {
                        var nextParent = VisualTreeHelper.GetParent(currentElement) as FrameworkElement;
                        if (nextParent is StackPanel panel && panel.Orientation == Orientation.Horizontal)
                        {
                            parentStackPanel = panel;
                            break;
                        }
                        currentElement = nextParent;
                    }

                    // Check if all borders in the StackPanel are collapsed
                    if (parentStackPanel != null)
                    {
                        bool allCollapsed = true;

                        foreach (var child in parentStackPanel.Children)
                        {
                            if (child is Border childBorder && childBorder.Visibility == Visibility.Visible)
                            {
                                allCollapsed = false;
                                break;
                            }
                        }

                        // If all borders are collapsed, collapse the StackPanel too
                        if (allCollapsed)
                        {
                            parentStackPanel.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        // New method to handle the refresh button click
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Find the WrapPanel that contains all widgets
            var scrollViewer = FindVisualChild<ScrollViewer>(this);
            if (scrollViewer != null)
            {
                var wrapPanel = FindVisualChild<WrapPanel>(scrollViewer);
                if (wrapPanel != null)
                {
                    // Make all borders (widgets) visible
                    RestoreVisibility(wrapPanel);
                }
            }
        }

        // Helper method to restore visibility of all widgets
        private void RestoreVisibility(DependencyObject parent)
        {
            // Make this element visible if it's a Border
            if (parent is Border border)
            {
                border.Visibility = Visibility.Visible;
            }

            // Make this element visible if it's a StackPanel
            if (parent is StackPanel stackPanel)
            {
                stackPanel.Visibility = Visibility.Visible;
            }

            // Visit all children recursively
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                RestoreVisibility(child);
            }
        }

        // Helper method to find a visual child of a specific type
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null;

            if (parent is T foundChild)
                return foundChild;

            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
