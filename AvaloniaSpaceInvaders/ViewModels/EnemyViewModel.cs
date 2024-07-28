using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AvaloniaSpaceInvaders.Models;
using ReactiveUI;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public class EnemyViewModel : ActorViewModel
    {
        private int _direction;
        private int Speed;
        private bool _isAlive;
        private int _id;

        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

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

        public int SPEED
        {
            get => Speed;
            set => this.RaiseAndSetIfChanged(ref Speed, value);
        }
        public EnemyViewModel(string imagePath, int width, int height, int direction , int speed,bool isalive,int Id) 
            : base(imagePath , width , height,isalive) 
        {
            _direction= direction;
            Speed = speed;  
            _isAlive= isalive;
            _id= Id;
        }

        protected override Actor CreateActor(string imagePath, int width, int height,bool isalive)
        {
            return new Enemy(imagePath, width, height, Direction, Speed, IsAlive,Id);
        }

        public override void Move()
        {
            LocationX += Speed * Direction;
        }
        public void Shoot()
        {

        }
    }
}
