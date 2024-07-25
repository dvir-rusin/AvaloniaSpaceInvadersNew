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
using Avalonia.Interactivity;
using AvaloniaSpaceInvaders.Views;
using AvaloniaSpaceInvaders.objects;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public class GameScreenViewModel : ReactiveObject
    {
        public double GameBoardWidth { get; set; } = 900;
        public double GameBoardHeight { get; set; } = 900;

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
        private int _lives = 3;
        public int Lives
        {
            get => _lives;
            set => this.RaiseAndSetIfChanged(ref _lives, value);
        }
        private int _score = 0;
        public int Score
        {
            get => _score;
            set => this.RaiseAndSetIfChanged(ref _score, value);
        }
        private bool GameStarted=false;
        private bool GameWon =false;
        private bool GameLost=false;

        private BulletViewModel bullet = null;
        private PlayerViewModel player=null;
        private RedSpaceShipViewModel redSpaceShip = null;

        private List<EnemyBulletViewModel> _enemyBullets= new List<EnemyBulletViewModel>();

        private List<ShieldViewModel> _Shileds= new List<ShieldViewModel>();
        private List<ShieldViewModel> _Shileds2 = new List<ShieldViewModel>();

        private int[] direction = { 1, -1 };
        public ICommand SpawnPlayerCommand { get; }
        public ICommand SpawnEnemyCommand { get; }
        public ICommand SpawnShieldCommand { get; }
        public ICommand SpawnRedSpaceShipCommand { get; }
        public ICommand SpawnBulletCommand { get; }
        public ICommand SpawnEnemyBulletCommand { get; }
        public ICommand MovePlayerLeftCommand { get; }
        public ICommand MovePlayerRightCommand { get; }


        private void InitializeEnemies()
        {
            for (int i = 0; i < 5; i++)
            {
                _enemiesList.Add(new List<EnemyViewModel>());
            }
        }
        public GameScreenViewModel()
        {
            SpawnPlayerCommand = ReactiveCommand.Create(SpawnPlayer);
            SpawnEnemyCommand = ReactiveCommand.Create(SpawnEnemies);
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

            InitializeEnemies();//create 5 lists of enemies ^
                                //|

            // Initialize the timer to spawn the red spaceship every 20 seconds
            DispatcherTimer redSpaceShipTimer = new DispatcherTimer();
            redSpaceShipTimer.Interval = TimeSpan.FromSeconds(20);
            redSpaceShipTimer.Tick += (s, e) => SpawnRedSpaceShip();
            redSpaceShipTimer.Start();
        }

        
        private void SpawnPlayer()//spawn player
        {
            string relativePathToAssets2 = "avares://AvaloniaSpaceInvaders/Assets/avalonia-logo.ico";
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersPlayer_1.ico";
            string relativePathToAssets3 = "avares://AvaloniaSpaceInvaders/Assets/avalonia-logo.ico";

            player = new PlayerViewModel(relativePathToAssets3, 64, 64,1, 1, true);
            player.SetPosition((GameBoardWidth-player.Width )/2, 580);
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







         private void SpawnEnemies()
        {
            int enemySpacing = 6;
            int enemyLevel = 1;
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersEnemyGreenT.png";

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, levelSpeed, true);
                    enemy.SetPosition(j * enemy.Width + enemySpacing, enemyLevel * enemy.Width);
                    Actors.Add(enemy);
                    _enemiesList[i].Add(enemy);
                }
                enemyLevel++;
            }
        }









        private void SpawnShield()
        {
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersShiled.png";
            int enemypositionY = 50;
            int COUNTER = 0;
            for (int i = 0; i < 9; i++)//TOP ROW OF SHILEDS
            {
                if(i==3)
                {
                    enemypositionY = 350;
                    COUNTER = 0;
                }
                if(i==6)
                {
                    COUNTER = 0;
                    enemypositionY = 650;
                }
                    var shield = new ShieldViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                    shield.Source.Opacity = 1;
                    shield.SetPosition(65 * COUNTER+ enemypositionY, 400);
                    Actors.Add(shield);
                    _Shileds.Add(shield);
                    COUNTER++;
            }

            enemypositionY = 50;
            COUNTER = 0;
            for (int i=0;i<6;i++)//bottom row
            {
                if(i==2)
                {
                    enemypositionY = 350;
                    COUNTER = 0;
                }
                if(i==4)
                {
                    enemypositionY = 650;
                    COUNTER = 0;
                }
                var shield = new ShieldViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                shield.Source.Opacity = 1;
                shield.SetPosition(130 * COUNTER + enemypositionY, 430);
                Actors.Add(shield);
                _Shileds2.Add(shield);
                COUNTER++;
            }

        
        }




        private void SpawnRedSpaceShip()
        {
            Random _random= new Random();
            int randDir = _random.Next(0,1);
            randDir = direction[randDir];
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersRedSpaceShipT.png";
            redSpaceShip = new RedSpaceShipViewModel(relativePathToAssets, 64, 64, randDir, 1,true);
            if(redSpaceShip.Direction == 1)
            {
                redSpaceShip.SetPosition(0, 0);
            }
            else
            {
                redSpaceShip.SetPosition(GameBoardWidth-redSpaceShip.Width, 0);
            }
            
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
            if (player == null || bullet != null) return;

            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersBullet.ico";
            bullet = new BulletViewModel(relativePathToAssets, 32, 32, 1, 1, true);
            bullet.SetPosition(player.LocationX, player.LocationY);
            Actors.Add(bullet);

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
            if (_enemiesList.All(row => row.Count == 0)) return;

            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersBullet.ico";
            var enemyBullet = new EnemyBulletViewModel(relativePathToAssets, 32, 32, 1, 1, true);
            enemyBullet.SetPosition(500, 0); // Randomize the position
            _enemyBullets.Add(enemyBullet);
            _actors.Add(enemyBullet);

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
        private void GameWonFunc()
        {
            GameWon = false;
            Lives = 3;
            levelSpeed += 1;
            _actors.Clear();
            SpawnPlayer();
            SpawnEnemies();
            SpawnShield();
        }

        private void GameOverFunc()
        {
            //UserControlChange();
        }

        //public void UserControlChange(object sender, RoutedEventArgs args)
        //{
        //    MainWindow window = this.;
        //    window.CurrentView.Content = new MainView();
        //}



        private void GameLoop(object sender, EventArgs e)
        {
            
            //initalizing the game 
            if (GameStarted==false)
            {
                SpawnPlayer();
                SpawnEnemies();
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
                GameWonFunc();
            }

            //game over checking 
            if(Lives <=0)
            {
                GameOverFunc();
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

        private Random _random = new Random();

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
                        Score += _random.Next(40,101);
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
                        Score += _random.Next(40, 101);
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
                        Score += _random.Next(40, 101);
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
                        Score += _random.Next(40, 101);
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
                        Score += _random.Next(40, 101);
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
                    Score += _random.Next(40, 101);//change the score to be random 
                }
            }



            // Check collision between player and enemies
            if (player != null && _enemiesList[0].Count>0 && player.LocationY == _enemiesList[0][_enemiesList[0].Count-1].LocationY)
            {
                foreach(var enemy in _enemiesList[0])
                {
                    if (enemy.Intersects(player))
                    {
                        Lives = 0;
                        GameOverFunc();
                    }
                }         
            }


            if (player != null && _enemiesList[1].Count > 0 && player.LocationY == _enemiesList[1][_enemiesList[1].Count - 1].LocationY)
            {
                foreach (var enemy in _enemiesList[1])
                {
                    if (enemy.Intersects(player))
                    {
                        Lives = 0;
                        GameOverFunc();
                    }
                }
            }

            if (player != null && _enemiesList[2].Count > 0 && player.LocationY == _enemiesList[2][_enemiesList[2].Count - 1].LocationY)
            {
                foreach (var enemy in _enemiesList[2])
                {
                    if (enemy.Intersects(player))
                    {
                        Lives = 0;
                        GameOverFunc();
                    }
                }
            }

            if (player != null && _enemiesList[3].Count > 0 && player.LocationY == _enemiesList[3][_enemiesList[3].Count - 1].LocationY)
            {
                foreach (var enemy in _enemiesList[3])
                {
                    if (enemy.Intersects(player))
                    {
                        Lives = 0;
                        GameOverFunc();
                    }
                }
            }

            if (player != null && _enemiesList[4].Count > 0 && player.LocationY == _enemiesList[4][_enemiesList[4].Count - 1].LocationY)
            {
                foreach (var enemy in _enemiesList[4])
                {
                    if (enemy.Intersects(player))
                    {
                        Lives = 0;
                        GameOverFunc();
                    }
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
                        Lives--;
                        _enemyBullets.Remove(enemyBullet);
                        _actors.Remove(enemyBullet);
                        return;
                    }
                    
                }
            }
        }
    }
}
