using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Interactivity;
using Avalonia.Controls;
using AvaloniaSpaceInvaders.Views;
using System;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        private Control _currentView;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Greeting => "Welcome to Avalonia!";

        public ICommand WelcomeBtn { get; }

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
            WelcomeBtn = new MyCommand(() => UserControlChange());
        }

        public void UserControlChange()
        {
            CurrentView = new SecondView();
        }
    }

    public class MyCommand : ICommand
    {
        private readonly Action _execute;

        public MyCommand(Action execute)
        {
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute();
        }
    }
}
