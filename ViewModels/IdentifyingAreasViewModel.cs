using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;

namespace BookBuddy.ViewModels
{
    internal class IdentifyingAreasViewModel : INotifyPropertyChanged
    {
        // Random generator for shuffle and random call numbers
        private Random _random = new Random();

        // Boolean to switch between matching call numbers to descriptions and descriptions to call numbers
        private bool isMatchingDescriptionToCallNumbers;

        // Used to store the values in the left column of my view, can either be call numbers or descriptions
        public ObservableCollection<string> LeftItems
        {
            get { return isMatchingDescriptionToCallNumbers ? CallNumbers : Descriptions; }
        }

        // Used to store the values in the right column of my view, can either be call numbers or descriptions
        public ObservableCollection<string> RightItems
        {
            get { return isMatchingDescriptionToCallNumbers ? Descriptions : CallNumbers; }
        }

        // Timer for the game
        private DispatcherTimer _gameTimer;

        // Store the elapsed seconds of the game
        private int _elapsedSeconds;

        // Constructor
        public IdentifyingAreasViewModel()
        {
            // Initialise to true, meaning the first game will have the call numbers on the left
            isMatchingDescriptionToCallNumbers = true;

            // Initialisation of commands
            GenerateNewQuestionCommand = new RelayCommand(GenerateNewQuestion);
            CheckMatchesCommand = new RelayCommand(CheckMatches);

            // Initialise the game start command
            StartGameCommand = new RelayCommand(StartGame);

            // Set up the game timer
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += GameTimer_Tick;

            // Generate initial questions
            GenerateNewQuestion();
        }

        // Command to generate a new randomised list of call numbers
        public ICommand GenerateNewQuestionCommand { get; }

        // Command to check the matches between the left and right items
        public ICommand CheckMatchesCommand { get; }

        // Command to start the game timer and related game activities
        public ICommand StartGameCommand { get; }

        // Dictionary for Dewey Decimal Classification data
        private readonly Dictionary<string, string> deweyDecimalTopLevel = new Dictionary<string, string>
        {
            {"000", "Computer science, general works, and information"},
            {"100", "Philosophy and psychology"},
            {"200", "Religion"},
            {"300", "Social sciences"},
            {"400", "Language"},
            {"500", "Natural sciences & mathematics"},
            {"600", "Technology"},
            {"700", "Arts & recreation"},
            {"800", "Literature"},
            {"900", "History & geography"}
        };

        // Collection for call numbers
        private ObservableCollection<string> CallNumbers = new ObservableCollection<string>();

        // Collection for descriptions
        private ObservableCollection<string> Descriptions = new ObservableCollection<string>();

        /// <summary>
        /// Generates a new question based on the boolean initialised previously and then starts the game.
        /// </summary>
        public void GenerateNewQuestion()
        {
            try
            {
                var random = _random;

                // Reset the CallNumbers and Descriptions collections
                CallNumbers.Clear();
                Descriptions.Clear();

                if (isMatchingDescriptionToCallNumbers)
                {
                    // Select four random call numbers for the question
                    var selectedCallNumbers = deweyDecimalTopLevel.Keys.OrderBy(arg => random.Next()).Take(4).ToList();

                    foreach (var term in selectedCallNumbers)
                    {
                        CallNumbers.Add(term);
                    }

                    // Get the correct descriptions for the selected call numbers
                    var correctDescriptions = selectedCallNumbers.Select(t => deweyDecimalTopLevel[t]).ToList();

                    // Get three random incorrect descriptions
                    var incorrectDescriptions = deweyDecimalTopLevel.Values.Except(correctDescriptions).OrderBy(arg => random.Next()).Take(3).ToList();

                    // Combine correct and incorrect descriptions and shuffle them
                    var allDescriptions = correctDescriptions.Concat(incorrectDescriptions).OrderBy(arg => random.Next()).ToList();

                    foreach (var definition in allDescriptions)
                    {
                        Descriptions.Add(definition);
                    }
                }
                else
                {
                    // Select four random descriptions for the question
                    var selectedDescriptions = deweyDecimalTopLevel.Values.OrderBy(arg => random.Next()).Take(4).ToList();

                    foreach (var definition in selectedDescriptions)
                    {
                        Descriptions.Add(definition);
                    }

                    // Get the correct call numbers for the selected descriptions
                    var correctCallNumbers = selectedDescriptions.Select(d => deweyDecimalTopLevel.First(pair => pair.Value == d).Key).ToList();

                    // Get three random incorrect call numbers
                    var incorrectCallNumbers = deweyDecimalTopLevel.Keys.Except(correctCallNumbers).OrderBy(arg => random.Next()).Take(3).ToList();

                    // Combine correct and incorrect call numbers and shuffle them
                    var allCallNumbers = correctCallNumbers.Concat(incorrectCallNumbers).OrderBy(arg => random.Next()).ToList();

                    foreach (var term in allCallNumbers)
                    {
                        CallNumbers.Add(term);
                    }
                }

                StartGame();

                // Notify the UI that the lists have changed
                OnPropertyChanged(nameof(LeftItems));
                OnPropertyChanged(nameof(RightItems));
            }
            catch (Exception ex)
            {
                // Show a user-friendly error message
                MessageBox.Show($"An error occurred while generating a new question: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Checks if the call numbers and description items match.
        /// </summary>
        public void CheckMatches()
        {
            try
            {
                int correctMatches = 0;

                // Get the minimum count between the left and right column to avoid out of range exceptions
                int minCount = Math.Min(LeftItems.Count, RightItems.Count);

                if (isMatchingDescriptionToCallNumbers)
                {
                    for (int i = 0; i < minCount; i++)
                    {
                        if (deweyDecimalTopLevel.ContainsKey(LeftItems[i]) && RightItems[i] == deweyDecimalTopLevel[LeftItems[i]])
                        {
                            correctMatches++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < minCount; i++)
                    {
                        if (deweyDecimalTopLevel.ContainsKey(RightItems[i]) && LeftItems[i] == deweyDecimalTopLevel[RightItems[i]])
                        {
                            correctMatches++;
                        }
                    }
                }

                if (correctMatches == minCount)
                {
                    // Toggle the state for the next game after checking current answers
                    isMatchingDescriptionToCallNumbers = !isMatchingDescriptionToCallNumbers;

                    StopGame();
                    MessageBox.Show("All matches are correct!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    GenerateNewQuestion();
                }
                else
                {
                    MessageBox.Show("The matches are incorrect. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking matches: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Moves an item within the right column, which could either be call numbers or descriptions, from an old index to a new index.
        /// </summary>
        /// <param name="oldIndex">The current index of the item to move.</param>
        /// <param name="newIndex">The new index to which the item should be moved.</param>
        public void MoveItem(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= RightItems.Count) return;
            if (newIndex < 0 || newIndex >= RightItems.Count) return;
            if (oldIndex == newIndex) return;

            var itemToMove = RightItems[oldIndex];
            RightItems.RemoveAt(oldIndex);
            RightItems.Insert(newIndex, itemToMove);

            // Debug logging:
            System.Diagnostics.Debug.WriteLine($"Moved item from index {oldIndex} to {newIndex}. New order:");
            foreach (var def in RightItems)
            {
                System.Diagnostics.Debug.WriteLine(def);
            }
            System.Diagnostics.Debug.WriteLine($"isMatchingDescriptionToCallNumbers: {isMatchingDescriptionToCallNumbers}");
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
