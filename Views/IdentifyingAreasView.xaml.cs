using BookBuddy.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookBuddy.Views
{
    /// <summary>
    /// Interaction logic for IdentifyingAreasView.xaml
    /// </summary>
    public partial class IdentifyingAreasView : UserControl
    {
        public IdentifyingAreasView()
        {
            try
            {
                InitializeComponent();
                DataContext = new IdentifyingAreasViewModel();

                var viewModel = (IdentifyingAreasViewModel)DataContext;
                viewModel.StartGameCommand.Execute(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string _draggedItem;

        /// <summary>
        /// Handles the preview mouse button down event for the list view.
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
                MessageBox.Show($"Error during drag initialization: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the preview mouse move event for the list view.
        /// Continues the drag operation when the mouse is moved.
        /// </summary>
        private void ListView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_draggedItem != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    DragDrop.DoDragDrop(rightItems, _draggedItem, DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during drag: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the drop event for the Definitions list view.
        /// Rearranges items based on the dragged and dropped positions.
        /// </summary>
        private void ListView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (_draggedItem == null) return;

                var droppedPosition = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (droppedPosition == null) return;

                int oldIndex = rightItems.Items.IndexOf(_draggedItem);
                int newIndex = rightItems.Items.IndexOf(droppedPosition.Content);

                var viewModel = (IdentifyingAreasViewModel)DataContext;
                viewModel.MoveItem(oldIndex, newIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during drop: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }

}
