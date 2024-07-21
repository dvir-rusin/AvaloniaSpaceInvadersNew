using Avalonia.Threading;
using AvaloniaSpaceInvaders.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public class GameScreenViewModel : ReactiveObject
    {
        private ObservableCollection<ActorViewModel> _actors = new ObservableCollection<ActorViewModel>();
        public ObservableCollection<ActorViewModel> Actors
        {
            get => _actors;
            set => this.RaiseAndSetIfChanged(ref _actors, value);
        }

        public ICommand SpawnPlayerCommand { get; }
        public ICommand SpawnEnemyCommand { get; }
        public ICommand SpawnShieldCommand { get; }
        public ICommand SpawnRedSpaceShipCommand { get; }
        public ICommand SpawnBulletCommand { get; }

        public GameScreenViewModel()
        {
            SpawnPlayerCommand = ReactiveCommand.Create(SpawnPlayer);
            SpawnEnemyCommand = ReactiveCommand.Create(SpawnEnemy);
            SpawnShieldCommand = ReactiveCommand.Create(SpawnShield);
            SpawnRedSpaceShipCommand = ReactiveCommand.Create(SpawnRedSpaceShip);
            SpawnBulletCommand = ReactiveCommand.Create(SpawnBullet);

            var gameLoopTimer = new DispatcherTimer();
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(16.66);
            gameLoopTimer.Tick += GameLoop;
            gameLoopTimer.Start();
        }

        private void SpawnPlayer()
        {
            var player = new PlayerViewModel(new Player("avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersPlayer_1.ico", 64, 64));
            player.SetPosition(400, 400);
            Actors.Add(player);
        }

        private void SpawnEnemy()
        {
            for(int i=0;i<3;i++)
            {
                var enemy = new EnemyViewModel(new Enemy("avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersEnemy.ico", 64, 64,true));
                enemy.SetPosition(i * 50, i * 50);
                Actors.Add(enemy);
            }
            
        }

        private void DeSpawnEnemy()//can change this hole function to be if interact, delete 
        {
            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(Enemy)&& actor.isAlive == false)
                {
                    Actors.Remove(actor);

                }
            }
        }


        private void SpawnShield()//needs to change the path to be shiled path / only rec with no img
        {

            var shield = new ShieldViewModel(new Shield("avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersPlayer_1.ico", 64, 64));
            Actors.Add(shield);
        }

        private void SpawnRedSpaceShip()
        {
            for (int i = 0; i < 3; i++)
            {
                var redSpaceShip = new RedSpaceShipViewModel(new RedSpaceShip("avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersRedSpaceShip.ico", 64, 64));
                redSpaceShip.SetPosition(i, i * 50);
                Actors.Add(redSpaceShip);
            }
                
        }

        private void SpawnBullet()
        {
            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(Bullet))
                {

                    return;
                }
                
            }
            var bullet = new BulletViewModel(new Bullet("avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersBullet.ico", 64, 64));

            bullet.SetPosition(FindplayerX(), FindplayerY());
            Actors.Add(bullet);
        }

        private void DeSpawnBullet()
        {
            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(Bullet)&& actor.LocationY<0)
                {
                    Actors.Remove(actor);

                }
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            foreach (var actor in Actors)
            {
                actor.Move();
            }
        }

        protected double FindplayerX()
        {
            
            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(Player))
                    return actor.LocationX;

            }
            return 0.0;
        }

        private double FindplayerY()
        {

            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(Player))
                    return actor.LocationY;

            }
            return 0.0;
        }
    }
}
