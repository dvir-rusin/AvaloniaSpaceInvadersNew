using Avalonia.Media;
using AvaloniaSpaceInvaders.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using ReactiveUI;
using AvaloniaSpaceInvaders.Objects;
using Avalonia.Controls;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public abstract class ActorViewModel : ReactiveObject
    {
        protected Actor _actor;
        
        
        

        protected ActorViewModel(string imagePath, int width, int height, bool isalive)
        {
            _actor = CreateActor(imagePath, width, height, isalive); 
        }
        protected abstract Actor CreateActor(string imagePath, int width, int height,bool isalive);

        public IImage img
        {
                get => _actor.Image.Source;
                set
                {
                    _actor.Image.Source = value;
                    this.RaisePropertyChanged(nameof(Source));
                }
            }
        public Image Source
            {
                get => _actor.Image;
                set
                {
                    _actor.Image = value;
                    this.RaisePropertyChanged(nameof(Source));
                }
            }
        public double LocationX
        {
            get => _actor.LocationX;
            set
            {
                _actor.LocationX = value;
                this.RaisePropertyChanged(nameof(LocationX));
            }
        }
        public double LocationY
        {
            get => _actor.LocationY;
            set
            {
                _actor.LocationY = value;
                this.RaisePropertyChanged(nameof(LocationY));
            }
        }

        public void SetPosition(double x, double y)
        {
            LocationX = x;
            LocationY = y;
        }
        public double Width => _actor.Width;
        public double Height => _actor.Height;
             
        public bool IsAlive
        {
            get => _actor.IsAlive;
            set
            {
                _actor.IsAlive = value;
                this.RaisePropertyChanged(nameof(IsAlive));
            }
        }
        
        public abstract void Move();

        public bool Intersects(ActorViewModel other)
        {
            return LocationX < other.LocationX + other.Width &&
                LocationX + Width > other.LocationX &&
                LocationX < other.LocationX + other.Height &&
                LocationX + Height > other.LocationX;
        }

        internal bool Intersects(EnemyViewModel enemy)
        {
            return LocationX < enemy.LocationX + enemy.Width &&
                LocationX + Width > enemy.LocationX &&
                LocationX < enemy.LocationX + enemy.Height &&
                LocationX + Height > enemy.LocationX;
        }

        internal bool Intersects(ShieldViewModel shiled)
        {
            return LocationX < shiled.LocationX + shiled.Width &&
                LocationX + Width > shiled.LocationX &&
                LocationX < shiled.LocationX + shiled.Height &&
                LocationX + Height > shiled.LocationX;
        }
    }
}
