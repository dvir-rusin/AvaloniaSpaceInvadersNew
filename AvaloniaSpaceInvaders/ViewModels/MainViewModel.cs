using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Interactivity;
using Avalonia.Controls;
using AvaloniaSpaceInvaders.Views;
using System;
using ReactiveUI;
using System.Data;
using AvaloniaSpaceInvaders.ViewModels;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        public string Greeting => "Welcome to Avalonia!";



        private Control _currentView;
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand WelcomeBtnCommand { get; }

        public Control CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentView)));
            }
        }

        public MainViewModel()
        {
            CurrentView = new MainView(); // Initialize with the initial view
            WelcomeBtnCommand = ReactiveCommand.Create(UserControlChange);

        }

        public void UserControlChange()
        {
            CurrentView = new GameScreen();
        }
    }

   
}
