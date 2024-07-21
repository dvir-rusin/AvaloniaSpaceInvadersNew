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

namespace AvaloniaSpaceInvaders.ViewModels
{
    public abstract class ActorViewModel : ReactiveObject
    {
        protected Actor _actor;
        

        protected ActorViewModel(Actor actor)
        {
            _actor = actor;
        }

        public IImage ImageSource => _actor.Image.Source;
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
        public bool isAlive => _actor.IsAlive;

        public abstract void Move();
    }
}
