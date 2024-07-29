using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaSpaceInvaders.Views;

public partial class GameResultScreen : UserControl
{
    public GameResultScreen()
    {
        InitializeComponent();
    }

    private void BackToMainMenu()
    {
        MainWindow window = (MainWindow)this.VisualRoot;
        window.CurrentView.Content = new MainView();
    }
}