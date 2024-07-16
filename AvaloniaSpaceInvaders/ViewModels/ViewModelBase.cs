using Avalonia;
using Avalonia.Media;
using AvaloniaSpaceInvaders.Objects;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace AvaloniaSpaceInvaders.ViewModels;

public class ViewModelBase : ReactiveObject
{
    private int _score;
    private int _lives;
    private ObservableCollection<GameObjects> _gameObjects;


    public ViewModelBase()
    {
        Score = 0;
        Lives = 3;
        GameObjects = new ObservableCollection<GameObjects>();
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

    public ObservableCollection<GameObjects> GameObjects
    {
        get => _gameObjects;
        set => this.RaiseAndSetIfChanged(ref _gameObjects, value);
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




