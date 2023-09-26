using BookBuddy.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BookBuddy.Views
{
    /// <summary>
    /// Interaction logic for ReplacingBooksView.xaml
    /// </summary>
    public partial class ReplacingBooksView : UserControl
    {
        public ReplacingBooksView()
        {
            try
            {
                InitializeComponent();
                DataContext = new ReplacingBooksViewModel();

                var viewModel = (ReplacingBooksViewModel)DataContext;
                viewModel.StartGameCommand.Execute(null);
            }
            catch (Exception ex)
            {
                // Handle error
                MessageBox.Show($"Initialization error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string _draggedItem;

        /// <summary>
        /// Handles the preview mouse button down event for list views.
        /// Starts the drag operation when an item is selected.
        /// </summary>
        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var listView = sender as ListView;
                var listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem == null) return;

                _draggedItem = (string)listViewItem.Content;

                DragDrop.DoDragDrop(listView, _draggedItem, DragDropEffects.Move);
            }
            catch (Exception ex)
            {
                // Handle error
                MessageBox.Show($"Error during drag initialization: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Finds the ancestor of the given type for a dependency object.
        /// </summary>
        public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        /// <summary>
        /// Handles the preview mouse move event for list views.
        /// Continues the drag operation when the mouse is moved.
        /// </summary>
        private void ListView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_draggedItem != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    DragDrop.DoDragDrop(lvCallNumbers, _draggedItem, DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                // Handle error
                MessageBox.Show($"Error during drag: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the drop event for list views.
        /// Rearranges items based on the dragged and dropped positions.
        /// </summary>
        private void ListView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (_draggedItem == null) return;

                var droppedPosition = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (droppedPosition == null) return;

                int oldIndex = lvCallNumbers.Items.IndexOf(_draggedItem);
                int newIndex = lvCallNumbers.Items.IndexOf(droppedPosition.Content);

                var viewModel = (ReplacingBooksViewModel)DataContext;
                viewModel.MoveItem(oldIndex, newIndex);
            }
            catch (Exception ex)
            {
                // Handle error
                MessageBox.Show($"Error during drop: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
