using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ReactiveUI;
using Avalonia.Platform;

namespace AvaloniaSpaceInvaders.Models
{
    public abstract class Actor : ReactiveObject
    {
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Image Image { get; set; }

        public bool IsAlive { get; set; }

        protected Actor(string imagePath, int width, int height)
        {
            Width = width;
            Height = height;
            IsAlive= true;
            Image = new Image
            {
                Width = Width,
                Height = Height,
                Source = new Bitmap(AssetLoader.Open(new Uri(imagePath))) // Adjust image loading as needed
            };
        }
    }
}
