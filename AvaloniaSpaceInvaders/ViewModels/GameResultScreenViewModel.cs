using AvaloniaSpaceInvaders.Models;
using AvaloniaSpaceInvaders.Views;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;

namespace AvaloniaSpaceInvaders.ViewModels
{
    /// <summary>
    /// ViewModel for the GameResultScreen. Handles displaying high scores and navigation back to the main menu.
    /// </summary>
    public class GameResultScreenViewModel : ReactiveObject
    {
        /// <summary>
        /// Collection of high score entries to be displayed on the screen.
        /// </summary>
        public ObservableCollection<HighScoreEntry> HighScores { get; } = new ObservableCollection<HighScoreEntry>();

        /// <summary>
        /// Command to navigate back to the main menu.
        /// </summary>
        public ICommand BackToMainMenuCommand { get; }

        /// <summary>
        /// File path for storing high score data.
        /// </summary>
        public string filePath = "highscore2.json";

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultScreenViewModel"/> class.
        /// Loads the high scores and sets up commands.
        /// </summary>
        public GameResultScreenViewModel()
        {
            LoadHighScores();
            BackToMainMenuCommand = ReactiveCommand.Create(BackToMainMenu);
        }

        /// <summary>
        /// Loads the high scores from a JSON file and populates the <see cref="HighScores"/> collection.
        /// </summary>
        private void LoadHighScores()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    // Read high score data from the file
                    var highScoreJson = File.ReadAllText(filePath);
                    var highScoreModel = JsonSerializer.Deserialize<HighScoreModel>(highScoreJson);

                    if (highScoreModel != null)
                    {
                        HighScores.Clear();
                        // Add each high score entry to the collection
                        foreach (var entry in highScoreModel.HighScores)
                        {
                            HighScores.Add(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., logging
                // You could add logging here for debugging purposes
            }
        }

        /// <summary>
        /// Navigates back to the main menu by changing the current view in the main window.
        /// </summary>
        private void BackToMainMenu()
        {
            // Ensure App.MainWindow is not null before accessing it
            if (App.MainWindow != null)
            {
                App.MainWindow.Content = new MainView();
            }
            else
            {
                // Handle the case where MainWindow is not set, e.g., log an error or throw an exception
                throw new InvalidOperationException("MainWindow is not set.");
            }
        }
    }
}
