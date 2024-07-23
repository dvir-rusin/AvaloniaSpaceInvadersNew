using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaSpaceInvaders.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        
    }

    public void UserControlChange(object sender, RoutedEventArgs args)
    {
        MainWindow window = (MainWindow)this.VisualRoot;
        window.CurrentView.Content = new GameScreen();
    }


}
