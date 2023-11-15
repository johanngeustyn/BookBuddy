using BookBuddy.Models;
using BookBuddy.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BookBuddy.Views
{
    /// <summary>
    /// Interaction logic for FindingCallNumbersView.xaml
    /// </summary>
    public partial class FindingCallNumbersView : UserControl
    {
        // Toggle button that is currently checked
        private ToggleButton _lastChecked;
        
        // View model
        private readonly FindingCallNumbersViewModel _viewModel;
        
        public FindingCallNumbersView()
        {
            try
            {
                InitializeComponent();
                DataContext = new FindingCallNumbersViewModel();

                // Set the data context
                _viewModel = (FindingCallNumbersViewModel)DataContext;

                // Start the first game's timer
                _viewModel.StartGameCommand.Execute(null);
            }
            catch (Exception ex)
            {
                MessageBoxText.Text = ex.Message;
                ShowMessage();
            }
        }

        // Set the selected option in my view model, ensuring only a single option is selected
        private void OnOptionChecked(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;

            // Uncheck the previously checked button.
            if (_lastChecked != null && _lastChecked != button)
            {
                _lastChecked.IsChecked = false;
            }

            _lastChecked = button;

            if (button != null)
            {
                _viewModel.SelectedOption = button.DataContext as DeweyTreeNode;
            }
        }

        // Clear the last checked item when another option is selected, ensuring only one option is selected at a given time
        private void OnOptionUnchecked(object sender, RoutedEventArgs e)
        {
            if (_lastChecked == sender)
                _lastChecked = null;
        }

        // View model message box close
        private void CloseMessageBox(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowMessageBox = false;
        }

        // View model message box show
        public void ShowMessage()
        {
            _viewModel.ShowMessageBox = true;
        }
    }
}
