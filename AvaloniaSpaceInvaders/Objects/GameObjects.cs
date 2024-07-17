using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace AvaloniaSpaceInvaders.Objects
{
    public class GameObjects: ReactiveObject
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

        public GameObjects()
        {
            Fill = Brushes.White;
        }


        public virtual void Draw(DrawingContext context)
        {
            // Basic drawing logic, can be overridden by derived classes
            Rect rect = new Rect(X, Y, Width, Height);
            context.FillRectangle(Fill, rect);
        }

        public bool Intersects(GameObjects other)
        {
            return X < other.X + other.Width &&
                X + Width > other.X &&
                Y < other.Y + other.Height &&
                Y + Height > other.Y;
        }
    }
}
