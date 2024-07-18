using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaSpaceInvaders.objects;
using AvaloniaSpaceInvaders.Objects;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvaloniaSpaceInvaders.ViewModels;

public class SecondViewModel: ViewModelBase
{
    private Player _player;
    private ObservableCollection<Enemy> _enemies;
    private ObservableCollection<Shield> _shields;

    public SecondViewModel()
    {
        // Initialize player
        Player = new Player { X = 200, Y = 400, Width = 40, Height = 20, Fill = Brushes.Blue };

        // Initialize enemies
        Enemies = new ObservableCollection<Enemy>();
        for (int i = 0; i < 5; i++)
        {
            Enemies.Add(new Enemy { X = i * 50, Y = 50, Width = 30, Height = 20, Fill = Brushes.Red });
        }

        // Initialize shields
        Shields = new ObservableCollection<Shield>();
        for (int i = 0; i < 3; i++)
        {
            Shields.Add(new Shield { X = i * 200 + 50, Y = 300, Width = 60, Height = 40, Fill = Brushes.Green });
        }


    }

    public Player Player
    {
        get => _player;
        set => this.RaiseAndSetIfChanged(ref _player, value);
    }

    public ObservableCollection<Enemy> Enemies
    {
        get => _enemies;
        set => this.RaiseAndSetIfChanged(ref _enemies, value);
    }

    public ObservableCollection<Shield> Shields
    {
        get => _shields;
        set => this.RaiseAndSetIfChanged(ref _shields, value);
    }
}

