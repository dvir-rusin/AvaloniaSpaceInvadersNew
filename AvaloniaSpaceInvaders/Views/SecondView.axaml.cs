using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using AvaloniaSpaceInvaders.objects;
using AvaloniaSpaceInvaders.ViewModels;
using System;
namespace AvaloniaSpaceInvaders.Views
{
    public partial class SecondView : UserControl
    {
        private DispatcherTimer _gameTimer;

        public SecondView()
        {
            InitializeComponent();
            this.Focusable = true;
            this.Focus();

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is SecondViewModel viewModel)
            {
                if (e.Key == Key.A)
                {
                    viewModel.Player.MoveLeft();
                }
                else if (e.Key == Key.D)
                {
                    viewModel.Player.MoveRight();
                }
                else if (e.Key == Key.Space)
                {
                    viewModel.ShootCommand.Execute();
                }
            }
        }
    }
}
