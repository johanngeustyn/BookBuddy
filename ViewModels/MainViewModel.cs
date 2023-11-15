using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace BookBuddy.ViewModels
{
    /// <summary>
    /// Main view model that serves as the primary data context for the main view.
    /// Provides navigation and management of sub-view models.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentViewModel;

        /// <summary>
        /// Gets or sets the current view model that the main view is displaying.
        /// </summary>
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        /// <summary>
        /// Command to navigate to the ReplacingBooks view.
        /// </summary>
        public ICommand NavigateToReplacingBooksCommand { get; private set; }

        /// <summary>
        /// Command to navigate to the IdentifyingAreas view.
        /// </summary>
        public ICommand NavigateToIdentifyingAreasCommand { get; private set; }

        /// <summary>
        /// Command to navigate to the FindingCallNumbers view.
        /// </summary>
        public ICommand NavigateToFindingCallNumbersCommand { get; private set; }

        /// <summary>
        /// Constructor for the MainViewModel.
        /// </summary>
        public MainViewModel()
        {
            try
            {
                // Initialize the commands.
                NavigateToReplacingBooksCommand = new RelayCommand(NavigateToReplacingBooks);
                NavigateToIdentifyingAreasCommand = new RelayCommand(NavigateToIdentifyingAreas);
                NavigateToFindingCallNumbersCommand = new RelayCommand(NavigateToFindingCallNumbers);
            }
            catch (Exception ex)
            {
                // Handle error
                throw ex;
            }
        }

        /// <summary>
        /// Sets the CurrentViewModel to a new instance of ReplacingBooksViewModel.
        /// </summary>
        private void NavigateToReplacingBooks()
        {
            try
            {
                CurrentViewModel = new ReplacingBooksViewModel();
            }
            catch (Exception ex)
            {
                // Handle error
                throw ex;
            }
        }

        /// <summary>
        /// Sets the CurrentViewModel to a new instance of IdentifyingAreasViewModel.
        /// </summary>
        private void NavigateToIdentifyingAreas()
        {
            try
            {
                CurrentViewModel = new IdentifyingAreasViewModel();
            }
            catch (Exception ex)
            {
                // Handle error
                throw ex;
            }
        }

        /// <summary>
        /// Sets the CurrentViewModel to a new instance of FindingCallNumbersViewModel.
        /// </summary>
        private void NavigateToFindingCallNumbers()
        {
            try
            {
                CurrentViewModel = new FindingCallNumbersViewModel();
            }
            catch (Exception ex)
            {
                // Handle error
                throw ex;
            }
        }

        /// <summary>
        /// Event that's raised when a property changes to notify the view.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the given property.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
