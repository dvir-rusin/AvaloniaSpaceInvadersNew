using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using AvaloniaSpaceInvaders.objects;
using AvaloniaSpaceInvaders.Objects;
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
                    //viewModel.MoveLeftCommand.Execute().Subscribe();
                }
                if (e.Key == Key.D)
                {
                    viewModel.Player.MoveRight();
                    //viewModel.MoveRightCommand.Execute().Subscribe();
                }
                if (e.Key == Key.S)
                {
                    viewModel.ShootCommand.Execute().Subscribe();
                }
            }
        }
    }
}
