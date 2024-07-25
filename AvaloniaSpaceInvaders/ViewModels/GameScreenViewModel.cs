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
using Avalonia;
using Avalonia.Media;
using DynamicData;
using AvaloniaSpaceInvaders.Objects;
using Avalonia.Controls;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public class GameScreenViewModel : ReactiveObject
    {
        public double GameBoardWidth { get; set; } = 900;
        public double GameBoardHeight { get; set; } = 700;

        private ObservableCollection<ActorViewModel> _actors = new ObservableCollection<ActorViewModel>();
        public ObservableCollection<ActorViewModel> Actors
        {
            get => _actors;
            set => this.RaiseAndSetIfChanged(ref _actors, value);
        }

        private List<List<EnemyViewModel>> _enemiesList= new List<List<EnemyViewModel>>();
        private List<EnemyViewModel> _enemies1 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies2 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies3 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies4 = new List<EnemyViewModel>();
        private List<EnemyViewModel> _enemies5 = new List<EnemyViewModel>();

        private int levelSpeed = 1;
        private int lives = 3;
        private int score = 0;
        private bool GameStarted=false;
        private bool GameWon =false;
        private bool GameLost=false;

        private BulletViewModel bullet = null;
        private PlayerViewModel player=null;
        private RedSpaceShipViewModel redSpaceShip = null;

        private List<EnemyBulletViewModel> _enemyBullets= new List<EnemyBulletViewModel>();

        private List<ShieldViewModel> _Shileds= new List<ShieldViewModel>();
        private List<ShieldViewModel> _Shileds2 = new List<ShieldViewModel>();
        

        public ICommand SpawnPlayerCommand { get; }
        public ICommand SpawnEnemyCommand { get; }
        public ICommand SpawnShieldCommand { get; }
        public ICommand SpawnRedSpaceShipCommand { get; }
        public ICommand SpawnBulletCommand { get; }
        public ICommand SpawnEnemyBulletCommand { get; }
        public ICommand MovePlayerLeftCommand { get; }
        public ICommand MovePlayerRightCommand { get; }

        public GameScreenViewModel()
        {
            SpawnPlayerCommand = ReactiveCommand.Create(SpawnPlayer);
            SpawnEnemyCommand = ReactiveCommand.Create(SpawnEnemy);
            SpawnShieldCommand = ReactiveCommand.Create(SpawnShield);
            SpawnRedSpaceShipCommand = ReactiveCommand.Create(SpawnRedSpaceShip);
            SpawnBulletCommand = ReactiveCommand.Create(SpawnBullet);
            SpawnEnemyBulletCommand = ReactiveCommand.Create(SpawnEnemyBullet);
            MovePlayerLeftCommand = ReactiveCommand.Create(MoveLeft);//player.MoveLeft
            MovePlayerRightCommand = ReactiveCommand.Create(MoveRight);//player.MoveRight

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

        
        private void SpawnPlayer()//spawn player
        {
            string relativePathToAssets2 = "avares://AvaloniaSpaceInvaders/Assets/avalonia-logo.ico";
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersPlayer_1.ico";
            string relativePathToAssets3 = "avares://AvaloniaSpaceInvaders/Assets/avalonia-logo.ico";

            player = new PlayerViewModel(relativePathToAssets3, 64, 64,1, 1, true);
            player.SetPosition(400, 600);
            Actors.Add(player);
        }
        private void MoveRight()
        {
            player.LocationX += 10;
        }
        private void MoveLeft()
        {
            player.LocationX -= 10;
        }







        private void SpawnEnemy()//spawn enemy
        {
            string relativePathToAssets2 = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersEnemyGreenT.png";
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersEnemy.ico";
            for (int i=0;i<11;i++)
            {
                    var enemy = new EnemyViewModel(relativePathToAssets2, 64, 64, 1, levelSpeed, true);
                    enemy.SetPosition(i * 70, 1 * 64);
                    Actors.Add(enemy);
                    _enemies1.Add(enemy);
            }

            for (int i = 0; i < 11; i++)
            {
                var enemy = new EnemyViewModel(relativePathToAssets2, 64, 64, 1, levelSpeed, true);
                enemy.SetPosition(i * 70, 2 * 64);
                Actors.Add(enemy);
                _enemies2.Add(enemy);
            }

            for (int i = 0; i < 11; i++)
            {
                var enemy = new EnemyViewModel(relativePathToAssets2, 64, 64, 1, levelSpeed, true);
                enemy.SetPosition(i * 70, 3 * 64);
                Actors.Add(enemy);
                _enemies3.Add(enemy);
            }

            for (int i = 0; i < 11; i++)
            { 
                var enemy = new EnemyViewModel(relativePathToAssets2, 64, 64, 1, levelSpeed, true);
                enemy.SetPosition(i * 70, 4 * 64);
                Actors.Add(enemy);
                _enemies4.Add(enemy);


            }

            for (int i = 0; i < 11; i++)
            {
                var enemy = new EnemyViewModel(relativePathToAssets2, 64, 64, 1, levelSpeed, true);
                enemy.SetPosition(i * 70, 5 * 64);
                Actors.Add(enemy);
                _enemies5.Add(enemy);
            }
        }









        private void SpawnShield()
        {
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersShiled.png";
            for (int i = 0; i < 3; i++)
            {
                    var shield = new ShieldViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                    shield.Source.Opacity = 1;
                    shield.SetPosition(65 * i+50, 400);
                    Actors.Add(shield);
                    _Shileds.Add(shield);
                
            }

            for (int i=0;i<2;i++)
            {
                var shield = new ShieldViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                shield.Source.Opacity = 1;
                shield.SetPosition(130 * i + 50, 430);
                Actors.Add(shield);
                _Shileds2.Add(shield);
            }

            for (int i = 0; i < 3; i++)
            {
                var shield = new ShieldViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                shield.Source.Opacity = 1;
                shield.SetPosition(65 * i + 350, 400);
                Actors.Add(shield);
                _Shileds.Add(shield);
            }

            for (int i = 0; i < 2; i++)
            {
                var shield = new ShieldViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                shield.Source.Opacity = 1;
                shield.SetPosition(130 * i + 350, 430);
                Actors.Add(shield);
                _Shileds2.Add(shield);
            }

            for (int i = 0; i < 3; i++)
            {
                var shield = new ShieldViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                shield.Source.Opacity = 1;
                shield.SetPosition(65 * i + 650, 400);
                Actors.Add(shield);
                _Shileds.Add(shield);
            }

            for (int i = 0; i < 2; i++)
            {
                var shield = new ShieldViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                shield.Source.Opacity = 1;
                shield.SetPosition(130 * i + 650, 430);
                Actors.Add(shield);
                _Shileds2.Add(shield);
            }
        }




        private void SpawnRedSpaceShip()
        {
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersRedSpaceShipT.png";
            redSpaceShip = new RedSpaceShipViewModel(relativePathToAssets, 64, 64,1,1,true);
            redSpaceShip.SetPosition(0, 0);
            Actors.Add(redSpaceShip);   
        }

        private void DeSpawnRedSpaceShip()
        {
            if(redSpaceShip == null) return;
            else if(redSpaceShip.LocationX>GameBoardWidth - redSpaceShip.Width)
            {
                _actors.Remove(redSpaceShip);
                redSpaceShip = null;
            }
            else if (redSpaceShip.LocationX<0)
            {
                _actors.Add(redSpaceShip);
                redSpaceShip = null;
            }
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
                bullet = new BulletViewModel(relativePathToAssets, 32, 32, 1, 1, true);

                bullet.SetPosition(player.LocationX,player.LocationY);
                Actors.Add(bullet);
            }
            
        }




        private void DeSpawnBullet()
        {
            if (bullet.LocationY < 0)
            {
                _actors.Remove(bullet);
                bullet = null;
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
                var enemyBullet = new EnemyBulletViewModel(relativePathToAssets, 32, 32, 1, 1, true);

                //enemyBullet.SetPosition();//random bot selectror location
                enemyBullet.SetPosition(500, 0);
                _enemyBullets.Add(enemyBullet);
                _actors.Add(enemyBullet);
            }

        }
        private void DeSpawnEnemyBullet()
        {
            foreach (var enemyBullet in _enemyBullets)
            {
                if (enemyBullet.LocationY>GameBoardHeight)
                {
                    _enemyBullets.Remove(enemyBullet);
                    _actors.Remove(enemyBullet);

                    return;
                }
            }
        }





        private void GameLoop(object sender, EventArgs e)
        {
            
            //initalizing the game 
            if (GameStarted==false)
            {
                SpawnPlayer();
                SpawnEnemy();
                SpawnShield();
                GameStarted = true;
            }


            //checking if player won the game 
            if ((_enemiesList[0].Count==0 && _enemiesList[1].Count == 0 && _enemiesList[2].Count == 0 
                && _enemiesList[3].Count == 0 && _enemiesList[4].Count == 0))
            {
                GameWon = true;
            }
            //restarting game with faster enemies
            if(GameWon==true)
            {
                GameWon = false;
                lives = 3;
                levelSpeed += 1;
                _actors.Clear();
                SpawnPlayer();
                SpawnEnemy();
                SpawnShield();
            }

            //game over checking 
            if(lives<=0)
            {
                //GameOver();
                //direct to main view screen
            }

            //collision player with boarders----
            if(player.LocationX + player.Width>=GameBoardWidth)
            {
                player.LocationX = GameBoardWidth - player.Width;
            }
            if(player.LocationX<=0)
            {
                player.LocationX = 0;   
            }
            //-----------------------------------


            // despawning  bullets functinality
            if (bullet!=null)
            {
                DeSpawnBullet();
            }
            if(_enemyBullets.Count>0)
            {
                DeSpawnEnemyBullet();
            }

            if(redSpaceShip!=null)
            {
                DeSpawnRedSpaceShip();
            }
            
            EnemyViewModel farestEnemy = null;

            EnemyViewModel closestEnemy = null;
            if (_enemiesList[0].Count!=0 && _enemiesList[1].Count!=0 && _enemiesList[2].Count!=0&& _enemiesList[3].Count!=0&& _enemiesList[4].Count!=0)
            {
                closestEnemy = getClosestEnemy();
                farestEnemy = getFarestEnemy();
            } 
            // checking for player bullet collision 
            if (player!=null&& bullet != null)
            {
                doesIntersect(_enemiesList, _Shileds,_Shileds2);
            }
            //checking for enemy bullet collision
            if(_Shileds.Count>0 && _enemiesList[0].Count != 0 && _enemiesList[1].Count != 0 && _enemiesList[2].Count != 0 && _enemiesList[3].Count != 0 && _enemiesList[4].Count != 0)
            {
                doesEnemyBulletIntersect(_enemyBullets, _Shileds, _Shileds2);
            }
            // moving all the actor with their own implementation
            foreach (var actor in Actors)
            {
                actor.Move();
                
            }
            //enemies movement functionality( if enemies hit the right wall)
            if (farestEnemy != null)
            {
                if (farestEnemy.LocationX + farestEnemy.Width >= GameBoardWidth)//
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

            //enemies movement functionality( if enemies hit the left wall)
            if (closestEnemy != null)
            {
                if (closestEnemy.LocationX <=0)
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
        //function for reciving the closest enemy to right wall
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


        //function for reciving the closest enemy to right wall
        private EnemyViewModel getClosestEnemy()
        {
            EnemyViewModel enemy1 = _enemiesList[0][0];
            EnemyViewModel enemy2 = _enemiesList[1][0];
            EnemyViewModel enemy3 = _enemiesList[2][0];
            EnemyViewModel enemy4 = _enemiesList[3][0];
            EnemyViewModel enemy5 = _enemiesList[4][0];

            EnemyViewModel closestEnemy = enemy1;

            if (enemy2.LocationX <= closestEnemy.LocationX)
                closestEnemy = enemy2;
            if (enemy3.LocationX <= closestEnemy.LocationX)
                closestEnemy = enemy3;
            if (enemy4.LocationX <= closestEnemy.LocationX)
                closestEnemy = enemy4;
            if (enemy5.LocationX <= closestEnemy.LocationX)
                closestEnemy = enemy5;
            return closestEnemy;
        }



        private void doesIntersect(List <List<EnemyViewModel>> _enemiesList, List<ShieldViewModel> _shields, List<ShieldViewModel> _shields2)//check if player bullet Intersect with anything
        {
            if (bullet!=null && _enemiesList[0].Count>0
                && bullet.LocationY>= _enemiesList[0][0].LocationY && bullet.LocationY <= _enemiesList[0][0].LocationY + _enemiesList[0][0].Height)
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
                        score += 40;
                        return;
                    }
                }
            }

            if (bullet != null && _enemiesList[1].Count>0  && 
                bullet.LocationY >= _enemiesList[1][0].LocationY && bullet.LocationY <= _enemiesList[1][0].LocationY + _enemiesList[1][0].Height)
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
                        score += 40;
                        return;
                    }
                }
            }

            if (bullet != null && _enemiesList[2].Count > 0 
                && bullet.LocationY >= _enemiesList[2][0].LocationY && bullet.LocationY <= _enemiesList[2][0].LocationY + _enemiesList[2][0].Height)
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
                        score += 40;
                        return;

                    }
                }
            }

            if (bullet != null && _enemiesList[3].Count > 0 &&
                bullet.LocationY >= _enemiesList[3][0].LocationY && bullet.LocationY <= _enemiesList[3][0].LocationY + _enemiesList[3][0].Height)
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
                        score += 40;
                        return;

                    }
                }
            }

            if (bullet != null && _enemiesList[4].Count > 0 
                && bullet.LocationY >= _enemiesList[4][0].LocationY && bullet.LocationY <= _enemiesList[4][0].LocationY + _enemiesList[4][0].Height)
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
                        score += 40;
                        return;

                    }
                }
            }
            if(_shields.Count>0&& bullet.LocationY == _shields[0].LocationY)
            {
                foreach (var shiled in _shields)
                {
                     if (bullet.Intersects(shiled))
                     {

                        shiled.Source.Opacity += 100;
                        if(shiled.Source.Opacity>1000)
                        {
                            _actors.Remove(shiled);
                            _shields.Remove(shiled);
                        }
                        _actors.Remove(bullet);
                        bullet = null;
                        return;
                     }
                }
            }

            if (_shields2.Count > 0 && bullet.LocationY == _shields2[0].LocationY)
            {
                foreach (var shiled in _shields2)
                {
                    if (bullet.Intersects(shiled))
                    {
                        shiled.Source.Opacity += 100;
                        if (shiled.Source.Opacity > 1000)
                        {
                            _actors.Remove(shiled);
                            _shields2.Remove(shiled);
                        }
                        _actors.Remove(bullet);
                        bullet = null;
                        return;
                    }
                }

            }


            //red space ship collision functionality
            if(redSpaceShip!=null && bullet.LocationY == redSpaceShip.LocationY)
            {
                if(bullet.Intersects(redSpaceShip))
                {
                    _actors.Remove(redSpaceShip);
                    redSpaceShip = null;
                    score += 40;//change the score to be random 
                }
            }
        }

        private void doesEnemyBulletIntersect(List<EnemyBulletViewModel> _enemyBullets,List<ShieldViewModel>_shields, List<ShieldViewModel> _shields2)//check if enemy bullet intersects with anything
        {
            foreach(var enemyBullet in _enemyBullets)//enemy bullet intersects with a shiled 
            {
                if(enemyBullet.LocationY == _shields[0].LocationY)
                {
                    foreach(var shiled in _shields)
                    {
                        if (enemyBullet.Intersects(shiled))
                        {
                            //neede to add opacity shiled changes 
                            //shiled.Source.Transitions.Clear();
                            shiled.Source.Opacity += 100;
                            if (shiled.Source.Opacity > 1000)
                            {
                                _actors.Remove(shiled);
                                _shields.Remove(shiled);
                            }
                            _enemyBullets.Remove(enemyBullet);
                            _actors.Remove(enemyBullet);
                            return;
                        }
                    }
                }

                if (enemyBullet.LocationY == _shields2[0].LocationY)
                {
                    foreach (var shiled in _shields2)
                    {
                        if (enemyBullet.Intersects(shiled))
                        {
                            //neede to add opacity shiled changes 
                            //shiled.Source.Transitions.Clear();
                            shiled.Source.Opacity += 100;
                            if (shiled.Source.Opacity > 1000)
                            {
                                _actors.Remove(shiled);
                                _shields2.Remove(shiled);
                            }
                            _enemyBullets.Remove(enemyBullet);
                            _actors.Remove(enemyBullet);
                            return;
                        }
                    }
                }
            }

            foreach (var enemyBullet in _enemyBullets)//enemy bullet intersects with a player 
            {
                if (enemyBullet.LocationY == player.LocationY)
                {
                    
                    if (enemyBullet.Intersects(player))
                    {
                        lives--;
                        _enemyBullets.Remove(enemyBullet);
                        _actors.Remove(enemyBullet);
                        return;
                    }
                    
                }
            }
        }
    }
}
