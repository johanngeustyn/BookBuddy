using BookBuddy.ViewModels;
using System;
using System.Windows;

namespace BookBuddy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor for the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                DataContext = new MainViewModel();
            }
            catch (Exception ex)
            {
                // Handle error
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
