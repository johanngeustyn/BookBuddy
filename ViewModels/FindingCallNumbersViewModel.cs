using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using BookBuddy.Services;
using BookBuddy.Models;

namespace BookBuddy.ViewModels
{
    internal class FindingCallNumbersViewModel : INotifyPropertyChanged
    {
        // Timer for the game
        private DispatcherTimer _gameTimer;

        // Store the elapsed seconds of the game
        private int _elapsedSeconds;

        // Tree data service to load data into my tree
        private readonly DeweyTreeDataService _dataService;

        // Seperate class to handle quiz logic
        private readonly QuizService _quizService;

        public FindingCallNumbersViewModel()
        {
            // Initialise and load my data into my tree data storage
            _dataService = new DeweyTreeDataService();
            List<DeweyTreeNode> data = _dataService.LoadData();
            
            // Initialise quiz logic
            _quizService = new QuizService(data);

            // Initialisation of commands
            GenerateNewQuestionCommand = new RelayCommand(GenerateNewQuestion);
            CheckAnswerCommand = new RelayCommand(CheckAnswer);

            // Initialise the game start command
            StartGameCommand = new RelayCommand(StartGame);

            // Set up the game timer
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += GameTimer_Tick;

            // Initialise the score
            Score = 0;

            // Generate initial question
            GenerateNewQuestion();
        }

        // Command triggered by my view, used to generate a new question
        public ICommand GenerateNewQuestionCommand { get; }

        // Command triggered by my view to check the current selected option
        public ICommand CheckAnswerCommand { get; }

        // Command to start the game timer and related game activities
        public ICommand StartGameCommand { get; }

        // Stores the correct answer to compare the selected option to
        private DeweyTreeNode correctAnswer;

        // Method to generate a new question
        private void GenerateNewQuestion()
        {
            try
            {
                // Use my quiz logic to get a random third level option
                ThirdLevel = _quizService.GetRandomThirdLevelEntry();

                // Get second and top level options from the third level option
                DeweyTreeNode correctSecondLevel = _quizService.FindParentOf(ThirdLevel);
                DeweyTreeNode correctTopLevel = _quizService.FindParentOf(correctSecondLevel);

                // Set the correct answer the the top level
                correctAnswer = correctTopLevel;
                
                // Logging to test without looking at the json file
                Console.WriteLine("Correct answer for new question:");
                Console.WriteLine(correctAnswer);

                // Use my quiz service to get the options for that level, ensuring one of them is the correct answer and the rest is random
                var options = _quizService.GetOptionsForLevel(correctTopLevel);
                
                // Clear and populate my list with the new options
                PossibleCallNumbers.Clear();
                foreach (var option in options)
                {
                    PossibleCallNumbers.Add(option);
                }

                // Start the game timer
                StartGame();

                // Notify the UI
                OnPropertyChanged(nameof(PossibleCallNumbers));
            }
            catch (Exception ex)
            {
                // Display error message
                MessageBox.Show($"An error occurred while generating a new question: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to get the next question once a question was correctly answered
        private void GenerateNextQuestion()
        {
            try
            {
                // Get third level's parent
                DeweyTreeNode correctNextLevel = _quizService.FindParentOf(ThirdLevel);

                // Set the new correct answer
                correctAnswer = correctNextLevel;

                // Logging to test without looking at the json file
                Console.WriteLine("Correct answer for next question:");
                Console.WriteLine(correctAnswer);

                // Use my quiz service to get the options for that level, ensuring one of them is the correct answer and the rest is random
                var options = _quizService.GetOptionsForLevel(correctNextLevel);

                // Clear and populate my list with the new options
                PossibleCallNumbers.Clear();
                foreach (var option in options)
                {
                    PossibleCallNumbers.Add(option);
                }

                // Notify the UI
                OnPropertyChanged(nameof(PossibleCallNumbers));
            }
            catch (Exception ex)
            {
                // Display error message
                MessageBox.Show($"An error occurred while generating a new question: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to check if the selected option is the correct one
        private void CheckAnswer()
        {
            // Check if selected option is the correct one
            if (SelectedOption == correctAnswer)
            {
                // Get the parent to check if I should ask the next question or generate a new one
                DeweyTreeNode parentLevel = _quizService.FindParentOf(SelectedOption);

                // Custom message box
                DisplayMessageBox("Correct! Proceed to the next question.");

                // If there is a parent meaning we are on the second level then we can safely stop the game and generate a new one, otherwise go the the next question
                if (parentLevel != null)
                {
                    Score++;
                    StopGame();
                    GenerateNewQuestion();
                }
                else
                {
                    GenerateNextQuestion();
                }
            }
            else
            {
                // Incorrect option selected, display the message box
                DisplayMessageBox("Incorrect. Please try again.");

                // Stop the game and start a new question
                StopGame();
                GenerateNewQuestion();
            }
        }

        // Method to display the custom message box in the UI
        private void DisplayMessageBox(string message)
        {
            MessageBoxText = message;
            ShowMessageBox = true;
        }

        // Variables used by my view
        private ObservableCollection<DeweyTreeNode> _possibleCallNumbers = new ObservableCollection<DeweyTreeNode>();
        public ObservableCollection<DeweyTreeNode> PossibleCallNumbers
        {
            get => _possibleCallNumbers;
            set
            {
                _possibleCallNumbers = value;
            }
        }

        private DeweyTreeNode _thirdLevel;
        public DeweyTreeNode ThirdLevel
        {
            get => _thirdLevel;
            set
            {
                _thirdLevel = value;
                OnPropertyChanged();
            }
        }

        private DeweyTreeNode _selectedOption;
        public DeweyTreeNode SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged();
            }
        }

        private string _messageBoxText;
        public string MessageBoxText
        {
            get => _messageBoxText;
            set
            {
                _messageBoxText = value;
                OnPropertyChanged();
            }
        }

        private bool _showMessageBox;
        public bool ShowMessageBox
        {
            get => _showMessageBox;
            set
            {
                _showMessageBox = value;
                OnPropertyChanged(nameof(MessageBoxText));
                OnPropertyChanged(nameof(ShowMessageBox));  // Notify UI of the change
            }
        }

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }

        public int ElapsedSeconds
        {
            get => _elapsedSeconds;
            set
            {
                _elapsedSeconds = value;
                OnPropertyChanged(nameof(ElapsedSeconds));
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            ElapsedSeconds++;
        }

        private void StartGame()
        {
            ElapsedSeconds = 0;
            _gameTimer.Start();
        }

        public void StopGame()
        {
            _gameTimer.Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
