using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Input;
using AvaloniaSpaceInvaders.Models;
using ReactiveUI;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public class PlayerViewModel : ActorViewModel
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
        public PlayerViewModel(string imagePath, int width, int height,int direction,int speed, bool isalive) 
            : base(imagePath, width,height,isalive) 
        {
            _direction = direction;
            Speed = speed;
            _isAlive = isalive;
        }
        public double getX(Player player)
        {
            return player.LocationX;
        }

        public double getY(Player player)
        {
            return player.LocationY;
        }
        

        public override void Move()
        {
            
        }

        protected override Actor CreateActor(string imagePath, int width, int height,bool isalive)
        {
            return new Player(imagePath, width, height,Direction,Speed,IsAlive);
        }
    }
}
