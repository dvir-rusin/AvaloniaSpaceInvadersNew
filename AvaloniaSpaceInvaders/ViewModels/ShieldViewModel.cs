using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaSpaceInvaders.Models;
using ReactiveUI;

namespace AvaloniaSpaceInvaders.ViewModels
{

    
    public class ShieldViewModel : ActorViewModel
    {
        private int _direction;
        private int Speed { get; }
        private bool _isAlive;

        public int Direction
        {
            get => _direction;
            set => this.RaiseAndSetIfChanged(ref _direction, value);
        }

        public bool IsAlive
        {
            get => _isAlive;
            set => this.RaiseAndSetIfChanged(ref _isAlive, value);
        }
        public ShieldViewModel(string imagePath, int width, int height,int direction,int speed,bool isalive) 
            : base(imagePath, width, height,isalive) 
        {
            _direction = direction;
            Speed = speed;
            _isAlive = isalive;
        }

        public override void Move()
        {
            // Implement shield movement logic if any
        }

        protected override Actor CreateActor(string imagePath, int width, int height,bool isalive)
        {
            return new Shield(imagePath, width, height,Direction,Speed,IsAlive);
        }
    }
}
