using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Threading;

namespace BookBuddy.ViewModels
{
    public class ReplacingBooksViewModel : INotifyPropertyChanged
    {
        // Random generator for shuffle and random call numbers
        private Random _random = new Random();

        // Timer for the game
        private DispatcherTimer _gameTimer;

        // Store the elapsed seconds of the game
        private int _elapsedSeconds;

        // Constructor
        public ReplacingBooksViewModel()
        {
            // Initialization of commands
            ShuffleCommand = new RelayCommand(Shuffle);
            GenerateNewListCommand = new RelayCommand(GenerateNewList);
            CheckOrderCommand = new RelayCommand(CheckOrder);

            // Generate initial call numbers
            CallNumbers = GenerateRandomCallNumbers(10);

            // Initialize the game start command
            StartGameCommand = new RelayCommand(StartGame);

            // Set up the game timer
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += GameTimer_Tick;
        }

        // Command to shuffle the current list of call numbers
        public ICommand ShuffleCommand { get; }

        // Command to generate a new randomized list of call numbers
        public ICommand GenerateNewListCommand { get; }

        // Command to check if the current order of call numbers is correct
        public ICommand CheckOrderCommand { get; }

        // Command to start the game timer and related game activities
        public ICommand StartGameCommand { get; }

        // Backing field for the list of call numbers
        private ObservableCollection<string> _callNumbers;

        // Property that represents the list of call numbers in the game
        // Any changes to this collection will notify the UI to refresh
        public ObservableCollection<string> CallNumbers
        {
            get { return _callNumbers; }
            set
            {
                if (_callNumbers != value)
                {
                    _callNumbers = value;
                    OnPropertyChanged();  // Notify any UI elements bound to CallNumbers that the data has changed
                }
            }
        }

        /// <summary>
        /// Shuffles the items in the CallNumbers collection using the Fisher-Yates shuffle algorithm.
        /// After shuffling, the game is started.
        /// </summary>
        private void Shuffle()
        {
            try
            {
                // Get the count of call numbers
                int n = CallNumbers.Count;

                // Start from the last item and traverse back to the first item
                for (int i = n - 1; i > 0; i--)
                {
                    // Pick a random index less than or equal to 'i'
                    int j = _random.Next(i + 1);

                    // Swap the elements at indices 'i' and 'j'
                    var temp = CallNumbers[i];
                    CallNumbers[i] = CallNumbers[j];
                    CallNumbers[j] = temp;
                }

                // Start the game timer and any other related activities
                StartGame();
            }
            catch (Exception ex)
            {
                // In case of any exceptions during the shuffle, display an error message to the user
                MessageBox.Show($"Error shuffling: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Generates a new list of random Call Numbers and then starts the game.
        /// </summary>
        private void GenerateNewList()
        {
            try
            {
                // Generate a new list with 10 random call numbers
                CallNumbers = GenerateRandomCallNumbers(10);

                // Start the game timer and any other related activities
                StartGame();
            }
            catch (Exception ex)
            {
                // In case of any exceptions during the list generation, display an error message to the user
                MessageBox.Show($"Error generating new list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if the current order of CallNumbers matches the correct sorted order.
        /// </summary>
        private void CheckOrder()
        {
            try
            {
                // Create a new list from the current CallNumbers
                ObservableCollection<string> sortedList = new ObservableCollection<string>(CallNumbers);

                // Sort the new list for comparison
                QuickSort(sortedList, 0, sortedList.Count - 1);

                // Check if the current order of CallNumbers matches the sorted list
                if (CallNumbers.SequenceEqual(sortedList))
                {
                    // If the order is correct, stop the game timer and show a success message
                    StopGame();
                    MessageBox.Show("You got the order correct!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // If the order is incorrect, inform the user
                    MessageBox.Show("The order is incorrect. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the order check process
                MessageBox.Show($"Error checking order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Moves an item within the CallNumbers list from an old index to a new index.
        /// </summary>
        /// <param name="oldIndex">The current index of the item to move.</param>
        /// <param name="newIndex">The new index to which the item should be moved.</param>
        public void MoveItem(int oldIndex, int newIndex)
        {
            // Check if the indices are within valid bounds of the CallNumbers list
            if (oldIndex < 0 || oldIndex >= CallNumbers.Count || newIndex < 0 || newIndex >= CallNumbers.Count)
                return;

            // Get the item to be moved
            var itemToMove = CallNumbers[oldIndex];

            // Remove the item from its current position
            CallNumbers.RemoveAt(oldIndex);

            // Insert the item at the new position
            CallNumbers.Insert(newIndex, itemToMove);

            // Notify the UI that the CallNumbers list has changed
            OnPropertyChanged(nameof(CallNumbers));
        }

        /// <summary>
        /// Sorts the provided list of call numbers using the QuickSort algorithm.
        /// </summary>
        /// <param name="callNumbers">The list of call numbers to sort.</param>
        /// <param name="low">The lower index of the segment to sort.</param>
        /// <param name="high">The higher index of the segment to sort.</param>
        private void QuickSort(ObservableCollection<string> callNumbers, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(callNumbers, low, high);
                QuickSort(callNumbers, low, pi - 1);
                QuickSort(callNumbers, pi + 1, high);
            }
        }

        /// <summary>
        /// Partitions the segment of the list into two halves. It places all smaller 
        /// (than pivot) call numbers before the pivot and all greater call numbers 
        /// after the pivot, then returns the pivot's index.
        /// </summary>
        /// <param name="callNumbers">The list of call numbers to partition.</param>
        /// <param name="low">The lower index of the segment to partition.</param>
        /// <param name="high">The higher index of the segment to partition.</param>
        /// <returns>The index of the pivot.</returns>
        private int Partition(ObservableCollection<string> callNumbers, int low, int high)
        {
            string pivot = callNumbers[high];
            int i = (low - 1);
            for (int j = low; j < high; j++)
            {
                if (CompareCallNumbers(callNumbers[j], pivot) <= 0)
                {
                    i++;
                    Swap(callNumbers, i, j);
                }
            }
            Swap(callNumbers, i + 1, high);
            return i + 1;
        }

        /// <summary>
        /// Swaps the positions of two call numbers in the list.
        /// </summary>
        /// <param name="callNumbers">The list of call numbers.</param>
        /// <param name="i">The index of the first call number to swap.</param>
        /// <param name="j">The index of the second call number to swap.</param>
        private void Swap(ObservableCollection<string> callNumbers, int i, int j)
        {
            string temp = callNumbers[i];
            callNumbers[i] = callNumbers[j];
            callNumbers[j] = temp;
        }

        /// <summary>
        /// Compares two call numbers. Call numbers are first compared based on their number 
        /// and then by their string portion if the numbers are equal.
        /// </summary>
        /// <param name="a">The first call number.</param>
        /// <param name="b">The second call number.</param>
        /// <returns>-1 if a < b, 1 if a > b, 0 if a = b or there's an exception during parsing.</returns>
        private int CompareCallNumbers(string a, string b)
        {
            try
            {
                string[] partsA = a.Split(' ');
                string[] partsB = b.Split(' ');

                double numberA = double.Parse(partsA[0]);
                double numberB = double.Parse(partsB[0]);

                if (numberA < numberB) return -1;
                if (numberA > numberB) return 1;
                return string.Compare(partsA[1], partsB[1], StringComparison.Ordinal);
            }
            catch
            {
                // Handle any exceptions during the comparison of call numbers
                return 0; // Neutral comparison
            }
        }

        /// <summary>
        /// Generates a list of random call numbers. Each call number consists of a number followed by 
        /// three uppercase alphabetic characters representing the author's surname initial.
        /// </summary>
        /// <param name="count">The number of call numbers to generate.</param>
        /// <returns>An ObservableCollection of generated call numbers.</returns>
        private ObservableCollection<string> GenerateRandomCallNumbers(int count)
        {
            // Create a new collection to store the generated call numbers.
            ObservableCollection<string> callNumbers = new ObservableCollection<string>();

            // Define the alphabet string to use when generating the alphabetic characters of the call numbers.
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // Loop to generate the specified number of call numbers.
            for (int i = 0; i < count; i++)
            {
                // Generate a random number between 0 and 999.99 for the numeric part of the call number.
                double topicNumber = Math.Round(_random.NextDouble() * 999.99, 2);

                // Generate three random characters from the alphabet to form the author's surname initials.
                char firstChar = alphabet[_random.Next(0, 26)];
                char secondChar = alphabet[_random.Next(0, 26)];
                char thirdChar = alphabet[_random.Next(0, 26)];
                string authorSurname = $"{firstChar}{secondChar}{thirdChar}";

                // Combine the numeric and alphabetic parts to form the call number.
                string callNumber = $"{topicNumber} {authorSurname}";

                // Add the generated call number to the collection.
                callNumbers.Add(callNumber);
            }

            // Return the generated list of call numbers.
            return callNumbers;
        }

        /// <summary>
        /// Gets or sets the number of seconds that have elapsed since the start of the game.
        /// When the value is set, it also triggers a property change notification for UI binding.
        /// </summary>
        public int ElapsedSeconds
        {
            get => _elapsedSeconds;
            set
            {
                _elapsedSeconds = value;
                OnPropertyChanged(nameof(ElapsedSeconds));
            }
        }

        /// <summary>
        /// Event handler for the game timer's Tick event. 
        /// Increments the elapsed seconds each time it's called, typically every second.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments (not used in this method).</param>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            ElapsedSeconds++;
        }

        /// <summary>
        /// Initializes the game by resetting the elapsed seconds and starting the game timer.
        /// </summary>
        private void StartGame()
        {
            ElapsedSeconds = 0;     // Reset the elapsed seconds counter to 0 at the start of the game.
            _gameTimer.Start();     // Start the game timer to begin counting.
        }

        /// <summary>
        /// Stops the game timer, effectively pausing or ending the game.
        /// </summary>
        public void StopGame()
        {
            _gameTimer.Stop();      // Stop the game timer.
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
