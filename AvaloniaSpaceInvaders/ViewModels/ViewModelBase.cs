using Avalonia;
using Avalonia.Media;
using AvaloniaSpaceInvaders.objects;
using AvaloniaSpaceInvaders.Objects;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace AvaloniaSpaceInvaders.ViewModels;

public class ViewModelBase : ReactiveObject
{
    private int _score;
    private int _lives;
    private Player _player;
    private Bullet _bullet;
    private ObservableCollection<Enemy> _enemies;
    private ObservableCollection<Shield> _shields;
    private bool _bulletActive;


    // Initialize bullet off-screen



    public ViewModelBase()
    {
        Score = 0;
        Lives = 3;

        Player = new Player { X = 400, Y = 500, Width = 40, Height = 20, Fill = Brushes.Blue };
        Enemies = new ObservableCollection<Enemy>();
        Shields = new ObservableCollection<Shield>();
        Bullet = new Bullet { X = 600, Y = 550, Fill = Brushes.Yellow };
        _bulletActive = false;

        // Example initialization of enemies and shields
        for (int i = 0; i < 5; i++)
        {
            Enemies.Add(new Enemy { X = i * 50, Y = 50, Width = 30, Height = 20, Fill = Brushes.Red });
        }

        for (int i = 0; i < 3; i++)
        {
            Shields.Add(new Shield { X = i * 200 + 50, Y = 300, Width = 60, Height = 40, Fill = Brushes.Green });
        }

        MoveLeftCommand = ReactiveCommand.Create(MoveLeft);
        MoveRightCommand = ReactiveCommand.Create(MoveRight);
        ShootCommand = ReactiveCommand.Create(Shoot);

        // Setup an observable to move the bullet upwards
        Observable.Interval(TimeSpan.FromMilliseconds(50))
                  .Where(_ => Bullet.Y > -Bullet.Height)
                  .Subscribe(_ => MoveBulletUp());

    }

    public Player Player
    {
        get => _player;
        set => this.RaiseAndSetIfChanged(ref _player, value);
    }

    public Bullet Bullet
    {
        get => _bullet;
        set => this.RaiseAndSetIfChanged(ref _bullet, value);
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

    public int Score
    {
        get => _score;
        set => this.RaiseAndSetIfChanged(ref _score, value);
    }

    public int Lives
    {
        get => _lives;
        set => this.RaiseAndSetIfChanged(ref _lives, value);
    }

    public ReactiveCommand<Unit, Unit> MoveLeftCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveRightCommand { get; }
    public ReactiveCommand<Unit, Unit> ShootCommand { get; }


    private void MoveLeft()
    {
        Player.X -= 10; // Adjust the movement speed as needed
    }

    private void MoveRight()
    {
        Player.X += 10; // Adjust the movement speed as needed
    }

    private void Shoot()
    {
        if (!_bulletActive)
        {
            // Reset bullet position to the player's position
            Bullet.X = Player.X + Player.Width / 2 - Bullet.Width / 2;
            Bullet.Y = Player.Y - Bullet.Height;
            _bulletActive = true;
        }
    }

    private void MoveBulletUp()
    {
        Bullet.Move(-10); // Adjust the speed as needed


        foreach (var enemy in Enemies)
        {
            if (Bullet.Intersects(enemy))
            {
                    // Handle collision (e.g., remove enemy, increase score, deactivate bullet)
                Enemies.Remove(enemy);
                Score += 10;
                _bulletActive = false;
                Bullet.X = -10; // Move bullet off-screen
                Bullet.Y = -10;
                return;
            }
        }
        // Check for collisions with shields
        foreach (var shield in Shields)
        {
             if (Bullet.Intersects(shield))
             {
                 // Handle collision (e.g., deactivate bullet)
                 _bulletActive = false;
                 Bullet.X = -10; // Move bullet off-screen
                 Bullet.Y = -10;
                 return;
             }
        }
        // Check if the bullet goes off-screen
        if (Bullet.Y + Bullet.Height < 0)
        {
            _bulletActive = false;
            Bullet.X = -10; // Move bullet off-screen
            Bullet.Y = -10;
        }
    }




}

public class GameObject : ReactiveObject
{
    private int _x;
    private int _y;
    private int _width;
    private int _height;
    private IBrush _fill;

    public int X
    {
        get => _x;
        set => this.RaiseAndSetIfChanged(ref _x, value);
    }

    public int Y
    {
        get => _y;
        set => this.RaiseAndSetIfChanged(ref _y, value);
    }
    public int Width
    {
        get => _width;
        set => this.RaiseAndSetIfChanged(ref _width, value);
    }

    public int Height
    {
        get => _height;
        set => this.RaiseAndSetIfChanged(ref _height, value);
    }

    public IBrush Fill
    {
        get => _fill;
        set => this.RaiseAndSetIfChanged(ref _fill, value);
    }


    public void Draw(DrawingContext context)
    {

        // Basic drawing logic, can be overridden by derived classes
        Rect rect = new Rect(X, Y, Width, Height);
        context.FillRectangle(Fill, rect);
    }


}




