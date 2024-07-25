using AvaloniaSpaceInvaders.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AvaloniaSpaceInvaders.ViewModels
{
    public class RedSpaceShipViewModel : ActorViewModel
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
        public RedSpaceShipViewModel(string imagePath, int width, int height,int direction,int speed,bool isalive) 
            : base(imagePath,width,height, isalive) 
        {
            _direction = direction;
            Speed = speed;
            _isAlive = isalive;
        }

        public override void Move()
        {
            if(Direction == 1)
            {
                LocationX += 3;
            }
            else if(Direction == -1) 
            {
                LocationX-= 3;
            }
        }

        protected override Actor CreateActor(string imagePath, int width, int height,bool isalive)
        {
            return new RedSpaceShip(imagePath,width,height, Direction,Speed,IsAlive);
        }
    }
}
