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

        private List<List<EnemyViewModel>> _enemiesList= new List<List<EnemyViewModel>>();
        private List<EnemyViewModel> _enemies1 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies2 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies3 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies4 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies5 = new List<EnemyViewModel>();
        

        private BulletViewModel bullet = null;

        private PlayerViewModel player=null;

        private List<EnemyBulletViewModel> _enemyBullets= new List<EnemyBulletViewModel>();
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
        public ICommand SpawnEnemyBulletCommand { get; }

        public GameScreenViewModel()
        {
            SpawnPlayerCommand = ReactiveCommand.Create(SpawnPlayer);
            SpawnEnemyCommand = ReactiveCommand.Create(SpawnEnemy);
            SpawnShieldCommand = ReactiveCommand.Create(SpawnShield);
            SpawnRedSpaceShipCommand = ReactiveCommand.Create(SpawnRedSpaceShip);
            SpawnBulletCommand = ReactiveCommand.Create(SpawnBullet);
            SpawnEnemyBulletCommand = ReactiveCommand.Create(SpawnEnemyBullet);

            var gameLoopTimer = new DispatcherTimer();
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(16.66);
            gameLoopTimer.Tick += GameLoop;
            gameLoopTimer.Start();

            _enemiesList.Add(_enemies1);
            _enemiesList.Add(_enemies2);
            _enemiesList.Add(_enemies3);
            _enemiesList.Add(_enemies4);
            _enemiesList.Add(_enemies5);
        }

        public double GameBoardWidth { get; set; } = 700;
        public double GameBoardHeight { get; set; } = 650;

        private void SpawnPlayer()
        {
            string relativePathToAssets2 = "avares://AvaloniaSpaceInvaders/Assets/avalonia-logo.ico";
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersPlayer_1.ico";
            player = new PlayerViewModel(relativePathToAssets, 64, 64,1,1,true);
            player.SetPosition(400, 600);
            Actors.Add(player);
        }

        private void SpawnEnemy()
        {
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersEnemy.ico";
            for (int i=0;i<11;i++)
            {
                
                
                    var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                    enemy.SetPosition(i * 64, 1 * 64);
                    Actors.Add(enemy);
                    _enemies1.Add(enemy);
                
                
            }
            for (int i = 0; i < 11; i++)
            {


                var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                enemy.SetPosition(i * 64, 2 * 64);
                Actors.Add(enemy);
                _enemies2.Add(enemy);



            }
            for (int i = 0; i < 11; i++)
            {


                var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                enemy.SetPosition(i * 64, 3 * 64);
                Actors.Add(enemy);
                _enemies3.Add(enemy);



            }
            for (int i = 0; i < 11; i++)
            {


                var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                enemy.SetPosition(i * 64, 4 * 64);
                Actors.Add(enemy);
                _enemies4.Add(enemy);



            }
            for (int i = 0; i < 11; i++)
            {


                var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                enemy.SetPosition(i * 64, 5 * 64);
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
        {
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersRedSpaceShip.ico";
          
            
            var redSpaceShip = new RedSpaceShipViewModel(relativePathToAssets, 64, 64,1,1,true);
            redSpaceShip.SetPosition(0, 0);
            Actors.Add(redSpaceShip);
                
            
                
        }

        private void SpawnBullet()
        {
            bool doesPlayerExist = false;
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersBullet.ico";
            foreach (var actor in Actors)
            {
                if (bullet!=null)
                {
                    return;
                }
                if (player!=null)
                {

                    doesPlayerExist = true;
                }

            }

            if(doesPlayerExist == false)
            {
                return; 
            }
            else
            {
                bullet = new BulletViewModel(relativePathToAssets, 64, 64, 1, 1, true);

                bullet.SetPosition(player.LocationX,player.LocationY);
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

        private void SpawnEnemyBullet()
        {
            bool enemiesExist = false;
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersBullet.ico";
            if (_enemiesList[0].Count > 0|| _enemiesList[1].Count > 0|| _enemiesList[2].Count > 0|| _enemiesList[3].Count > 0|| _enemiesList[4].Count > 0)
            {
                enemiesExist = true;
            }
            if (enemiesExist == false)
            {
                return;
            }
            else
            {
                var enemyBullet = new EnemyBulletViewModel(relativePathToAssets, 64, 64, 1, 1, true);

                //enemyBullet.SetPosition();//random bot selectror location
                _enemyBullets.Add(enemyBullet);
                _actors.Add(enemyBullet);
            }

        }
        private void DeSpawnEnemyBullet()
        {
            foreach (var enemyBullet in _enemyBullets)
            {
                if (enemyBullet.LocationY>700)
                {
                    _enemyBullets.Remove(enemyBullet);
                    _enemyBullets.Remove(enemyBullet);


                }
            }
        }





        private void GameLoop(object sender, EventArgs e)
        {
            EnemyViewModel farestEnemy = null;

            EnemyViewModel closestEnemy = null;
            if (_enemiesList[0].Count!=0 && _enemiesList[1].Count!=0 && _enemiesList[2].Count!=0&& _enemiesList[3].Count!=0&& _enemiesList[4].Count!=0)
            {
                closestEnemy = getClosestEnemy();
                farestEnemy = getFarestEnemy();
            } 
            if (player!=null&& bullet != null)
            {
                doesIntersect(_enemiesList);
            }
                
                
            foreach (var actor in Actors)
            {
                actor.Move();
                
            }
            if (farestEnemy != null)
            {
                if (farestEnemy.LocationX + farestEnemy.Width >= GameBoardWidth+150)//aditional 150 cuz of main window screen
                {

                    foreach (EnemyViewModel enemy in _enemiesList[0])
                    {
                        enemy.LocationY += 2;
                        enemy.Direction = -1;
                    }
                    foreach (EnemyViewModel enemy in _enemiesList[1])
                    {
                        enemy.LocationY += 2;
                        enemy.Direction = -1;
                    }
                    foreach (EnemyViewModel enemy in _enemiesList[2])
                    {
                        enemy.LocationY += 2;
                        enemy.Direction = -1;
                    }
                    foreach (EnemyViewModel enemy in _enemiesList[3])
                    {
                        enemy.LocationY += 2;
                        enemy.Direction = -1;
                    }
                    foreach (EnemyViewModel enemy in _enemiesList[4])
                    {
                        enemy.LocationY += 2;
                        enemy.Direction = -1;
                    }
                }
            }

            if (closestEnemy != null)
            {
                if (closestEnemy.LocationX <=0-150)//-150 cuz of main window screen
                {

                    foreach (EnemyViewModel enemy in _enemiesList[0])
                    {
                        enemy.LocationY += 2;
                        enemy.Direction = 1;
                    }
                    foreach (EnemyViewModel enemy in _enemiesList[1])
                    {
                        enemy.LocationY += 2;
                        enemy.Direction = 1;
                    }
                    foreach (EnemyViewModel enemy in _enemiesList[2])
                    {
                        enemy.LocationY += 2;
                        enemy.Direction = 1;
                    }
                    foreach (EnemyViewModel enemy in _enemiesList[3])
                    {
                        enemy.LocationY += 2;
                        enemy.Direction = 1;
                    }
                    foreach (EnemyViewModel enemy in _enemiesList[4])
                    {
                        enemy.LocationY += 2;
                        enemy.Direction = 1;
                    }
                }
            }


        }
        private EnemyViewModel getFarestEnemy()
        {
            EnemyViewModel enemy1= _enemiesList[0][_enemiesList[0].Count-1];
            EnemyViewModel enemy2= _enemiesList[1][_enemiesList[1].Count-1];
            EnemyViewModel enemy3 = _enemiesList[2][_enemiesList[2].Count-1];
            EnemyViewModel enemy4 = _enemiesList[3][_enemiesList[3].Count - 1];
            EnemyViewModel enemy5 = _enemiesList[4][_enemiesList[4].Count - 1];

            EnemyViewModel farestEnemy = enemy1;
            
            
            if(enemy2.LocationX> farestEnemy.LocationX)
                farestEnemy=enemy2;
            if(enemy3.LocationX > farestEnemy.LocationX)
                farestEnemy = enemy3;
            if(enemy4.LocationX > farestEnemy.LocationX)
                farestEnemy = enemy4;
            if(enemy5.LocationX > farestEnemy.LocationX)
                farestEnemy = enemy5;
            return farestEnemy;


        }

        private EnemyViewModel getClosestEnemy()
        {
            EnemyViewModel enemy1 = _enemiesList[0][0];
            EnemyViewModel enemy2 = _enemiesList[1][0];
            EnemyViewModel enemy3 = _enemiesList[2][0];
            EnemyViewModel enemy4 = _enemiesList[3][0];
            EnemyViewModel enemy5 = _enemiesList[4][0];

            EnemyViewModel closestEnemy = enemy1;


            if (enemy2.LocationX < closestEnemy.LocationX)
                closestEnemy = enemy2;
            if (enemy3.LocationX < closestEnemy.LocationX)
                closestEnemy = enemy3;
            if (enemy4.LocationX < closestEnemy.LocationX)
                closestEnemy = enemy4;
            if (enemy5.LocationX < closestEnemy.LocationX)
                closestEnemy = enemy5;
            return closestEnemy;


        }



        private void doesIntersect(List <List<EnemyViewModel>> _enemiesList)
        {
            if (bullet!=null && bullet.LocationY>= _enemiesList[0][0].LocationY && bullet.LocationY <= _enemiesList[0][0].LocationY + _enemiesList[0][0].Height)
            {
                foreach (EnemyViewModel enemy in _enemiesList[0])
                {
                    if(bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        _enemiesList[0].Remove(enemy);
                        _actors.Remove(bullet);
                        bullet = null;
                        return;
                    }
                }
            }

            if (bullet != null && bullet.LocationY >= _enemiesList[1][0].LocationY && bullet.LocationY <= _enemiesList[1][0].LocationY + _enemiesList[1][0].Height)
            {
                foreach (var enemy in _enemiesList[1])
                {
                    if (bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        _enemiesList[1].Remove(enemy);
                        _actors.Remove(bullet);
                        bullet = null;
                        return;
                    }
                }
            }

            if (bullet != null && bullet.LocationY >= _enemiesList[2][0].LocationY && bullet.LocationY <= _enemiesList[2][0].LocationY + _enemiesList[2][0].Height)
            {
                foreach (var enemy in _enemiesList[2])
                {
                    if (bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        _enemiesList[2].Remove(enemy);
                        _actors.Remove(bullet);
                        bullet = null;
                        return;

                    }
                }
            }

            if (bullet != null && bullet.LocationY >= _enemiesList[3][0].LocationY && bullet.LocationY <= _enemiesList[3][0].LocationY + _enemiesList[3][0].Height)
            {
                foreach (var enemy in _enemiesList[3])
                {
                    if (bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        _enemiesList[3].Remove(enemy);
                        _actors.Remove(bullet);
                        bullet = null;
                        return;

                    }
                }
            }

            if (bullet != null && bullet.LocationY >= _enemiesList[4][0].LocationY && bullet.LocationY <= _enemiesList[4][0].LocationY + _enemiesList[4][0].Height)
            {
                foreach (var enemy in _enemiesList[4])
                {
                    if (bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        _enemiesList[4].Remove(enemy);
                        _actors.Remove(bullet);
                        bullet = null;
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

        //protected double FindplayerX()
        //{
            
        //    foreach (var actor in Actors)
        //    {
        //        if (actor.GetType() == typeof(PlayerViewModel))
        //            return actor.LocationX;

        //    }
        //    return 0.0;
        //}

        //private double FindplayerY()
        //{

        //    foreach (var actor in Actors)
        //    {
        //        if (actor.GetType() == typeof(PlayerViewModel))
        //            return actor.LocationY;

        //    }
        //    return 0.0;
        //}
    }
}
