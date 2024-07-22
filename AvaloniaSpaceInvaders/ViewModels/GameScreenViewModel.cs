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

        private List<EnemyViewModel> _enemies1 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies2 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies3 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies4 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies5 = new List<EnemyViewModel>();

        private BulletViewModel bullet;
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
            string relativePathToAssets2 = "avares://AvaloniaSpaceInvaders/Assets/avalonia-logo.ico";
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersPlayer_1.ico";
            var player = new PlayerViewModel(relativePathToAssets, 64, 64,1,1,true);
            player.SetPosition(400, 400);
            Actors.Add(player);
        }

        private void SpawnEnemy()
        {
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersEnemy.ico";
            for (int i=0;i<11;i++)
            {
                
                
                    var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                    enemy.SetPosition(i * 64, 0 * 64);
                    Actors.Add(enemy);
                    _enemies1.Add(enemy);
                
                
            }
            for (int i = 0; i < 11; i++)
            {


                var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                enemy.SetPosition(i * 64, 1 * 64);
                Actors.Add(enemy);
                _enemies2.Add(enemy);



            }
            for (int i = 0; i < 11; i++)
            {


                var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                enemy.SetPosition(i * 64, 2 * 64);
                Actors.Add(enemy);
                _enemies3.Add(enemy);



            }
            for (int i = 0; i < 11; i++)
            {


                var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                enemy.SetPosition(i * 64, 3 * 64);
                Actors.Add(enemy);
                _enemies4.Add(enemy);



            }
            for (int i = 0; i < 11; i++)
            {


                var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                enemy.SetPosition(i * 64, 4 * 64);
                Actors.Add(enemy);
                _enemies5.Add(enemy);



            }

        }

        private void DeSpawnEnemy()//can change this hole function to be if interact, delete 
        {
            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(Enemy)&& actor.IsAlive == false)
                {
                    Actors.Remove(actor);

                }
            }
        }


        private void SpawnShield()//needs to change the path to be shiled path / only rec with no img
        {
            
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersPlayer_1.ico";

            var shield = new ShieldViewModel(relativePathToAssets, 64, 64,1,1,true);
            Actors.Add(shield);
        }

        private void SpawnRedSpaceShip()
        {string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersRedSpaceShip.ico";
            for (int i = 0; i < 3; i++)
            {
                var redSpaceShip = new RedSpaceShipViewModel(relativePathToAssets, 64, 64,1,1,true);
                redSpaceShip.SetPosition(i, i * 50);
                Actors.Add(redSpaceShip);
                
            }
                
        }

        private void SpawnBullet()
        {
            bool flag = false;
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersBullet.ico";
            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(Bullet))
                {

                    return;
                }
                if (actor.GetType() == typeof(PlayerViewModel))
                {

                    flag = true;
                }

            }

            if(flag == false)
            {
                return; 
            }
            else
            {
                bullet = new BulletViewModel(relativePathToAssets, 64, 64, 1, 1, true);

                bullet.SetPosition(FindplayerX(), FindplayerY());
                Actors.Add(bullet);
            }
            
        }

        private void DeSpawnBullet()
        {
            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(BulletViewModel)&& actor.LocationY<0)
                {
                    Actors.Remove(actor);

                }
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            doesIntersect(_enemies1, _enemies2, _enemies3, _enemies4, _enemies5);

            foreach (var actor in Actors)
            {
                actor.Move();
                
            }
            
        }
        private void doesIntersect(List <EnemyViewModel> _enemies1, 
            List<EnemyViewModel> _enemies2,
            List<EnemyViewModel> _enemies3,
            List<EnemyViewModel> _enemies4,
            List<EnemyViewModel> _enemies5)
        {
            if (bullet!=null && bullet.LocationY>=_enemies1[0].LocationY && bullet.LocationY <= _enemies1[0].LocationY + _enemies1[0].Height)
            {
                foreach (EnemyViewModel enemy in _enemies1)
                {
                    if(bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        _enemies1.Remove(enemy);
                        _actors.Remove(bullet);
                        return;
                    }
                }
            }

            if (bullet != null && bullet.LocationY >= _enemies2[0].LocationY && bullet.LocationY <= _enemies2[0].LocationY + _enemies2[0].Height)
            {
                foreach (var enemy in _enemies2)
                {
                    if (bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        _enemies2.Remove(enemy);
                        _actors.Remove(bullet);
                        return;
                    }
                }
            }

            if (bullet != null && bullet.LocationY >= _enemies3[0].LocationY && bullet.LocationY <= _enemies3[0].LocationY + _enemies3[0].Height)
            {
                foreach (var enemy in _enemies3)
                {
                    if (bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        _enemies3.Remove(enemy);
                        _actors.Remove(bullet);
                        return;

                    }
                }
            }

            if (bullet != null && bullet.LocationY >= _enemies4[0].LocationY && bullet.LocationY <= _enemies4[0].LocationY + _enemies4[0].Height)
            {
                foreach (var enemy in _enemies4)
                {
                    if (bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        _enemies4.Remove(enemy);
                        _actors.Remove(bullet);
                        return;

                    }
                }
            }

            if (bullet != null && bullet.LocationY >= _enemies5[0].LocationY && bullet.LocationY <= _enemies5[0].LocationY + _enemies5[0].Height)
            {
                foreach (var enemy in _enemies5)
                {
                    if (bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        _enemies5.Remove(enemy);
                        _actors.Remove(bullet);
                        return;

                    }
                }
            }


        }
        /*
        protected ActorViewModel FindBullet()
        {
            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(BulletViewModel))
                    return actor;

            }
            return null;
        }
        */

        protected double FindplayerX()
        {
            
            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(PlayerViewModel))
                    return actor.LocationX;

            }
            return 0.0;
        }

        private double FindplayerY()
        {

            foreach (var actor in Actors)
            {
                if (actor.GetType() == typeof(PlayerViewModel))
                    return actor.LocationY;

            }
            return 0.0;
        }
    }
}
