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
using System.Text.Json;
using System.IO;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public class GameScreenViewModel : ReactiveObject
    {
        public int GameBoardWidth { get; set; } = 1000;
        public int GameBoardHeight { get; set; } = 900;

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

        private int _HighScore = 0;
        public int HighScore
        {
            get => _HighScore;
            set => this.RaiseAndSetIfChanged(ref _HighScore, value);
        }
        private const string HighScoreFilePath = "highscore.json";//FILE to save high score

        private BulletViewModel bullet = null;
        private PlayerViewModel player=null;
        private RedSpaceShipViewModel redSpaceShip = null;

        private List<EnemyBulletViewModel> _enemyBullets= new List<EnemyBulletViewModel>();

        private List<ShieldViewModel> _Shileds= new List<ShieldViewModel>();
        private List<ShieldViewModel> _Shileds2 = new List<ShieldViewModel>();

        private int[] direction = { 1, -1 } ;
        public ICommand SpawnPlayerCommand { get; }
        public ICommand SpawnEnemyCommand { get; }
        public ICommand SpawnShieldCommand { get; }
        public ICommand SpawnRedSpaceShipCommand { get; }
        public ICommand SpawnBulletCommand { get; }
        public ICommand SpawnEnemyBulletCommand { get; }
        public ICommand MovePlayerLeftCommand { get; }
        public ICommand MovePlayerRightCommand { get; }


        private void InitializeEnemies()//InitializeEnemies
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

            InitializeEnemies();//create 5 lists of enemies 
            LoadHighScore();//LoadHighScore
            SpawnPlayer();//SpawnPlayer
            SpawnEnemies();//SpawnEnemies
            SpawnShield();//SpawnShield


            // Initialize the timer to spawn the red spaceship every 15 seconds
            DispatcherTimer redSpaceShipTimer = new DispatcherTimer();
            redSpaceShipTimer.Interval = TimeSpan.FromSeconds(15);
            redSpaceShipTimer.Tick += (s, e) => SpawnRedSpaceShip();
            redSpaceShipTimer.Start();

            // Timer for spawning enemy bullets every 3 seconds
            DispatcherTimer enemyBulletTimer = new DispatcherTimer();
            enemyBulletTimer.Interval = TimeSpan.FromSeconds(3);
            enemyBulletTimer.Tick += (s, e) => SpawnEnemyBullet();
            enemyBulletTimer.Start();
        }

        
        private void SpawnPlayer()//spawn player, creates, sets pos and adds to actors(the way to see on screen)
        {
            string relativePathToAssets2 = "avares://AvaloniaSpaceInvaders/Assets/avalonia-logo.ico";
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersPlayer_1.ico";
            string relativePathToAssets3 = "avares://AvaloniaSpaceInvaders/Assets/avalonia-logo.ico";

            player = new PlayerViewModel(relativePathToAssets3, 64, 64,1, 1, true);
            player.SetPosition((GameBoardWidth-player.Width )/2, 550);
            Actors.Add(player);
        }
        private void MoveRight()//player move right 
        {
            player.LocationX += 10;
        }
        private void MoveLeft()//player move left
        {
            player.LocationX -= 10;
        }







        private void SpawnEnemies()//SpawnEnemies 5*11,creates ,sets pos and adds to actors(the way to see on screen)
        {
            int enemySpacing = 6;
            int enemyLevel = 1;
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersEnemyGreenT.png";

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    
                    var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, levelSpeed, false,-1);

                    enemy.SetPosition(j * enemy.Width + enemySpacing, enemyLevel * enemy.Width);
                    Actors.Add(enemy);
                    _enemiesList[i].Add(enemy);
                }
                enemyLevel++;
            }
            setId();
        }

        private void setId()//sets the id for enemies and enemy bullets, a way to connect the two
        {
            for(int i = 0; i < _enemiesList[0].Count-1;i++)
            {
                _enemiesList[0][i].Id = i;
            }
        }










        private void SpawnShield()
        {
            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersShiled.png";

            //TOP ROW OF SHILEDS
            int enemypositionX = 100;
            int COUNTER = 0;
            for (int i = 0; i < 9; i++)
            {
                if(i==3)
                {
                    enemypositionX = 400;
                    COUNTER = 0;
                }
                if(i==6)
                {
                    COUNTER = 0;
                    enemypositionX = 700;
                }
                    var shield = new ShieldViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                    shield.Source.Opacity = 1;
                    shield.SetPosition(65 * COUNTER+ enemypositionX, 400);
                    Actors.Add(shield);
                    _Shileds.Add(shield);
                    COUNTER++;
            }




            //bottom row
            enemypositionX = 100;
            COUNTER = 0;
            for (int i=0;i<6;i++)
            {
                if(i==2)
                {
                    enemypositionX = 400;
                    COUNTER = 0;
                }
                if(i==4)
                {
                    enemypositionX = 700;
                    COUNTER = 0;
                }
                var shield = new ShieldViewModel(relativePathToAssets, 64, 64, 1, 1, true);
                shield.Source.Opacity = 1;
                shield.SetPosition(130 * COUNTER + enemypositionX, 430);
                Actors.Add(shield);
                _Shileds2.Add(shield);
                COUNTER++;
            }

        
        }




        private void SpawnRedSpaceShip()//SpawnRedSpaceShip 
        {
            Random _random= new Random();
            int randDir = _random.Next(0,2);
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

        private void DeSpawnRedSpaceShip()//DeSpawnRedSpaceShip
        {
            if(redSpaceShip == null) return;
            else if(redSpaceShip.LocationX>GameBoardWidth - redSpaceShip.Width)
            {
                _actors.Remove(redSpaceShip);
                redSpaceShip = null;
            }
            else if (redSpaceShip.LocationX<0)
            {
                _actors.Remove(redSpaceShip);
                redSpaceShip = null;
            }
        }



        private void SpawnBullet()//SpawnBullet
        {
            if (player == null || bullet != null) return;

            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersBullet.ico";
            bullet = new BulletViewModel(relativePathToAssets, 32, 32, 1, 1, true);
            bullet.SetPosition(player.LocationX, player.LocationY);
            Actors.Add(bullet);

        }




        private void DeSpawnBullet()//DeSpawnBullet
        {
            if (bullet!=null && bullet.LocationY < 0)
            {
                _actors.Remove(bullet);
                bullet = null;
            }
                
        }

        private void SpawnEnemyBullet()//SpawnEnemyBullet
        {
            if (_enemiesList[0].Count==0) 
                return;

            Random random =new Random();

            int rand = random.Next(0, _enemiesList[0].Count-1);

            while (_enemiesList[0][rand].IsAlive == true)
            {
                rand = random.Next(0, _enemiesList[0].Count - 1);
            }

            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersBullet.ico";
            var enemyBullet = new EnemyBulletViewModel(relativePathToAssets, 32, 32, 1, 1, false, _enemiesList[0][rand].Id);
            enemyBullet.SetPosition(_enemiesList[0][rand].LocationX, _enemiesList[0][rand].LocationY);
            _enemiesList[0][rand].IsAlive= true;
            _enemyBullets.Add(enemyBullet);
            _actors.Add(enemyBullet);

        }

        private void DeSpawnEnemyBullet()//DeSpawnEnemyBullet
        {
            foreach (var enemyBullet in _enemyBullets)
            {
                if (enemyBullet.LocationY>GameBoardHeight)
                {
                    _enemyBullets.Remove(enemyBullet);
                    _actors.Remove(enemyBullet);
                    for(int i = 0; i < _enemiesList[0].Count-1;i++)
                    {
                        if (_enemiesList[0][i].Id==enemyBullet.Id)
                        {
                            _enemiesList[0][i].IsAlive= false;
                        }
                    }
                    
                    return;
                }
            }
        }
        private void GameWonFunc()//GameWonFunc - clears actors, and spawns everything again
        {
            Lives = 3;
            levelSpeed += 1;
            _actors.Clear();
            SpawnPlayer();
            SpawnEnemies();
            SpawnShield();
            SpawnRedSpaceShip();
        }

        private void GameOverFunc()//clears the screen and saves the current score if its new high score
        {
            if (Score >= HighScore)
            {
                HighScore = Score;
                SaveHighScore();
            }
            _actors.Clear();
            
        }


        private void LoadHighScore()//loads high score from file 
        {
            try
            {
                if (File.Exists(HighScoreFilePath))
                {
                    var highScoreJson = File.ReadAllText(HighScoreFilePath);//gets file data
                    var highScoreData = JsonSerializer.Deserialize<HighScoreModel>(highScoreJson);//Deserialize the data from file
                    HighScore = highScoreData?.HighScore ?? 0;//if (highscore data from file == null) sets to 0
                }
            }
            catch (Exception ex)//if there was a problem sets (high score = 0)
            {
                // Handle exceptions, e.g., logging
                HighScore = 0;
            }
        }

        private void SaveHighScore()
        {
            try
            {
                var highScoreData = new HighScoreModel { HighScore = HighScore };//sets the current highscore to
                                                                                 //highscore viewmodel data
                var highScoreJson = JsonSerializer.Serialize(highScoreData);//Serialize the current viewmodel highscore
                File.WriteAllText(HighScoreFilePath, highScoreJson);//writes the highscore in the file
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., logging
            }
        }



        private void GameLoop(object sender, EventArgs e)//happends every 16.6 sec
        {
            // Move all actors
            foreach (var actor in Actors)
            {
                actor.Move();//call all actors move functions 
            }

            // Collision: player with borders(value,min,max)
            player.LocationX = Math.Clamp(player.LocationX, 0, GameBoardWidth - player.Width);

            // Despawn bullets and other objects
            if (bullet != null) DeSpawnBullet();
            if (_enemyBullets.Count > 0) DeSpawnEnemyBullet();
            if (redSpaceShip != null) DeSpawnRedSpaceShip();

            // Check for player bullet collision
            if (player != null)
            {
                doesIntersect(_enemiesList, _Shileds, _Shileds2);
            }

            // Check for enemy bullet collision
            if (_Shileds.Count > 0 && _enemiesList.All(row => row.Count > 0))
            {
                doesEnemyBulletIntersect(_enemyBullets, _Shileds, _Shileds2);
            }

            // Determine closest and farthest enemies
            if (_enemiesList.All(row => row.Count > 0))
            {
                var closestEnemy = getClosestEnemy();
                var farestEnemy = getFarestEnemy();

                // Handle enemies hitting the walls
                if (farestEnemy?.LocationX + farestEnemy.Width >= GameBoardWidth)
                {
                    ChangeEnemiesDirection(-1);//x-=1
                }
                if (closestEnemy?.LocationX <= 0)
                {
                    ChangeEnemiesDirection(1);//X+=1
                }
            }

            // Check if the player has won the game
            if (_enemiesList.All(row => row.Count == 0))
            {
                GameWonFunc();
            }

            // Game over check
            if (Lives <= 0)
            {
                GameOverFunc();
                // Direct to main view screen
            }
        }

        //ENEMIES DIRECTION/ MOVEMENT HANDLE
        private void ChangeEnemiesDirection(int direction)
        {
            foreach (var row in _enemiesList)
            {
                foreach (var enemy in row)
                {
                    enemy.LocationY += 2;
                    enemy.Direction = direction;
                }
            }
        }




        // Function for receiving the farthest enemy to the right wall
        private EnemyViewModel getFarestEnemy()
        {
            EnemyViewModel farestEnemy = _enemiesList[0][_enemiesList[0].Count - 1];

            foreach (var row in _enemiesList)
            {
                var lastEnemyInRow = row[row.Count - 1];
                if (lastEnemyInRow.LocationX > farestEnemy.LocationX)
                {
                    farestEnemy = lastEnemyInRow;
                }
            }

            return farestEnemy;
        }

        // Function for receiving the closest enemy to the left wall
        private EnemyViewModel getClosestEnemy()
        {
            EnemyViewModel closestEnemy = _enemiesList[0][0];

            foreach (var row in _enemiesList)
            {
                var firstEnemyInRow = row[0];
                if (firstEnemyInRow.LocationX < closestEnemy.LocationX)
                {
                    closestEnemy = firstEnemyInRow;
                }
            }

            return closestEnemy;
        }




        

        private void doesIntersect(List<List<EnemyViewModel>> enemiesList, List<ShieldViewModel> shields, List<ShieldViewModel> shields2)
        {
            if(bullet != null)
            {
                CheckBulletEnemyCollision(enemiesList);
                CheckBulletShieldCollision(shields, shields2);
                CheckBulletRedSpaceShipCollision();
            }
            
            CheckEnemyPlayerCollision(enemiesList);
        }


        private Random _random = new Random();
        private void CheckBulletEnemyCollision(List<List<EnemyViewModel>> enemiesList)
        {
            if (bullet == null) return;

            foreach (var enemyRow in enemiesList)
            {
                if (enemyRow.Count == 0 || bullet.LocationY < enemyRow[0].LocationY || bullet.LocationY > enemyRow[0].LocationY + enemyRow[0].Height)
                    continue;

                foreach (var enemy in enemyRow.ToList())
                {
                    if (bullet.Intersects(enemy))
                    {
                        enemy.IsAlive = false;
                        _actors.Remove(enemy);
                        enemyRow.Remove(enemy);
                        RemoveBullet();
                        Score += _random.Next(40, 101);
                        return;
                    }
                }
            }
        }

        private void CheckBulletShieldCollision(List<ShieldViewModel> shields, List<ShieldViewModel> shields2)
        {
            if (bullet == null) return;

            CheckShieldCollision(shields);
            CheckShieldCollision(shields2);
        }

        private void CheckShieldCollision(List<ShieldViewModel> shields)
        {
            if (shields.Count == 0 || bullet==null||bullet.LocationY != shields[0].LocationY) return;

            foreach (var shield in shields.ToList())
            {
                if (bullet.Intersects(shield))
                {
                    shield.Source.Opacity += 100;
                    if (shield.Source.Opacity > 1000)
                    {
                        _actors.Remove(shield);
                        shields.Remove(shield);
                    }
                    RemoveBullet();
                    return;
                }
            }
        }

        private void CheckBulletRedSpaceShipCollision()
        {
            if (bullet == null || redSpaceShip == null || bullet.LocationY != redSpaceShip.LocationY) return;

            if (bullet.Intersects(redSpaceShip))
            {
                _actors.Remove(redSpaceShip);
                redSpaceShip = null;
                Score += _random.Next(40, 101);
                RemoveBullet();
            }
        }

        private void CheckEnemyPlayerCollision(List<List<EnemyViewModel>> enemiesList)
        {
            if (player == null) return;

            foreach (var enemyRow in enemiesList)
            {
                if (enemyRow.Count == 0 || player.LocationY != enemyRow.Last().LocationY) continue;

                foreach (var enemy in enemyRow)
                {
                    if (enemy.Intersects(player))
                    {
                    Lives = 0;
                    GameOverFunc();
                    return;
                    }
                }
            }
        }

        private void RemoveBullet()
        {
            _actors.Remove(bullet);
            bullet = null;
        }

        private void doesEnemyBulletIntersect(List<EnemyBulletViewModel> _enemyBullets,List<ShieldViewModel>_shields, List<ShieldViewModel> _shields2)//check if enemy bullet intersects with anything
        {
            foreach(var enemyBullet in _enemyBullets)//enemy bullet intersects with a shiled 
            {
                if(enemyBullet.LocationY >= _shields[0].LocationY)
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
                            //set bullet alive = false , can now spawn bullets 
                            //_enemiesList[0][enemyBullet.Id].IsAlive = false;
                            for(int i = 0; i < _enemiesList[0].Count - 1; i++)
                            {
                                if (_enemiesList[0][i].Id== enemyBullet.Id)
                                {
                                    _enemiesList[0][i].IsAlive = false;
                                    
                                }
                            }
                            _enemyBullets.Remove(enemyBullet);
                            _actors.Remove(enemyBullet);
                            return;
                        }
                    }
                }

                if (enemyBullet.LocationY >= _shields2[0].LocationY)
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
                            //set bullet alive = false , can now spawn bullets 
                            //_enemiesList[0][enemyBullet.Id].IsAlive = false;
                            for (int i = 0; i < _enemiesList[0].Count - 1; i++)
                            {
                                if (_enemiesList[0][i].Id == enemyBullet.Id)
                                {
                                    _enemiesList[0][i].IsAlive = false;
                                    
                                }
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
                if (enemyBullet.LocationY >= player.LocationY)
                {
                    
                    if (enemyBullet.Intersects(player))
                    {
                        //set bullet alive = false , can now spawn bullets 
                        //_enemiesList[0][enemyBullet.Id].IsAlive = false;
                        for (int i = 0; i < _enemiesList[0].Count - 1; i++)
                        {
                            if (_enemiesList[0][i].Id == enemyBullet.Id)
                            {
                                _enemiesList[0][i].IsAlive = false;

                            }
                        }
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
