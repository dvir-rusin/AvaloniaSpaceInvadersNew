using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaSpaceInvaders.Models;
using ReactiveUI;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public class BulletViewModel : ActorViewModel
    {
        private int _direction;
        private int Speed { get; }
        private bool _isAlive;

        public bool IsAlive
        {
            get => _isAlive;
            set => this.RaiseAndSetIfChanged(ref _isAlive, value);
        }

        public int Direction
        {
            get => _direction;
            set => this.RaiseAndSetIfChanged(ref _direction, value);
        }
        public BulletViewModel(string imagePath, int width, int height,int direction,int speed,bool isalive ) 
            : base(imagePath, width, height,isalive) 
        {
            IsAlive= isalive;
            _direction = direction;
            Speed = speed;
           
        }

        public override void Move()
        {
            LocationY -= 10;

        }

        protected override Actor CreateActor(string imagePath, int width, int height,bool isalive)
        {
            return new Bullet(imagePath,width,height,Direction,Speed, IsAlive);
        }
    }
}
