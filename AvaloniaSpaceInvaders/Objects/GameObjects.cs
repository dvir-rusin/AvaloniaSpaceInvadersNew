using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSpaceInvaders.Objects
{
    public class GameObjects
    {

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public IBrush Fill { get; set; }


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
