using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaSpaceInvaders.ViewModels;

namespace AvaloniaSpaceInvaders.Views
{
    public partial class GameScreen : UserControl
    {
        public GameScreen()
        {
            InitializeComponent();
        }
        public void UserControlChange(object sender, RoutedEventArgs args)
        {
            MainWindow window = (MainWindow)this.VisualRoot;
            window.CurrentView.Content = new GameScreen();
        }
    }
}
