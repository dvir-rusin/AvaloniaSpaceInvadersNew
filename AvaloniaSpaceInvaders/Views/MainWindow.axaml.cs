using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaSpaceInvaders.Views;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();
        CurrentView.Content = new MainView();
        this.WindowState= WindowState.Maximized;
    }
    public void ClickHandler(object sender, RoutedEventArgs args)
    {
        CurrentView.Content = new GameScreen();

    }

    public void ClickHandler2(object sender, RoutedEventArgs args)
    {
        CurrentView.Content = new MainView();

    }

    public void ClickHandler3(object sender, RoutedEventArgs args)
    {
        CurrentView.Content = new GameResultScreen();

    }

}
