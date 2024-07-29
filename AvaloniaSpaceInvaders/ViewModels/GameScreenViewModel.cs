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
using Avalonia.Input.TextInput;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public class GameScreenViewModel : ReactiveObject
    {
        bool isHighScore = false;
        bool gameOver = false;
        public int GameBoardWidth { get; set; } = 1000;
        public int GameBoardHeight { get; set; } = 900;

        private ObservableCollection<ActorViewModel> _actors = new ObservableCollection<ActorViewModel>();
        public ObservableCollection<ActorViewModel> Actors
        {
            get => _actors;
            set => this.RaiseAndSetIfChanged(ref _actors, value);
        }

        private List<List<EnemyViewModel>> _enemiesList= new List<List<EnemyViewModel>>();

        private int levelSpeed = 1;
        private int _lives = 3;//change
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
        private string? _HighScoreName = null;
        public int HighScore
        {
            get => _HighScore;
            set => this.RaiseAndSetIfChanged(ref _HighScore, value);
        }
        public string HighScoreName
        {
            get => _HighScoreName;
            set => this.RaiseAndSetIfChanged(ref _HighScoreName, value);
        }

        private bool _isNameInputVisible;
        public bool IsNameInputVisible
        {
            get => _isNameInputVisible;
            set => this.RaiseAndSetIfChanged(ref _isNameInputVisible, value);
        }

        private string _playerNameInput;
        public string PlayerNameInput
        {
            get => _playerNameInput;
            set => this.RaiseAndSetIfChanged(ref _playerNameInput, value);
        }

        private string _currentName;
        public string CurrentName
        {
            get => _currentName;
            set => this.RaiseAndSetIfChanged(ref _currentName, value);
        }
        private const string HighScoreFilePath = "highscore2.json";
        private HighScoreModel _highScoreModel = new HighScoreModel();

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
        public ICommand SubmitNameCommand { get; }
        public ICommand NavigateToGameResultScreenCommand { get; }


        private void InitializeEnemies()//InitializeEnemies
        {
            for (int i = 0; i < 5; i++)
            {
                _enemiesList.Add(new List<EnemyViewModel>());
            }
        }
        public GameScreenViewModel()
        {
            if (gameOver != true)
            {
                SpawnPlayerCommand = ReactiveCommand.Create(SpawnPlayer);
                SpawnEnemyCommand = ReactiveCommand.Create(SpawnEnemies);
                SpawnShieldCommand = ReactiveCommand.Create(SpawnShield);
                SpawnRedSpaceShipCommand = ReactiveCommand.Create(SpawnRedSpaceShip);
                SpawnBulletCommand = ReactiveCommand.Create(SpawnBullet);
                SpawnEnemyBulletCommand = ReactiveCommand.Create(SpawnEnemyBullet);
                MovePlayerLeftCommand = ReactiveCommand.Create(MoveLeft);//player.MoveLeft
                MovePlayerRightCommand = ReactiveCommand.Create(MoveRight);//player.MoveRight
                SubmitNameCommand = ReactiveCommand.Create(SubmitName);

                var gameLoopTimer = new DispatcherTimer();
                gameLoopTimer.Interval = TimeSpan.FromMilliseconds(16.66);
                gameLoopTimer.Tick += GameLoop;
                gameLoopTimer.Start();

                InitializeEnemies();//create 5 lists of enemies 
                LoadHighScores();//LoadHighScore
                SpawnPlayer();//SpawnPlayer
                SpawnEnemies();//SpawnEnemies
                SpawnShield();//SpawnShield
                


                // Initialize the timer to spawn the red spaceship every 15 seconds
                DispatcherTimer redSpaceShipTimer = new DispatcherTimer();
                redSpaceShipTimer.Interval = TimeSpan.FromSeconds(15);
                if (player != null)
                {
                    redSpaceShipTimer.Tick += (s, e) => SpawnRedSpaceShip();
                }
                
                redSpaceShipTimer.Start();

                // Timer for spawning enemy bullets every 3 seconds
                DispatcherTimer enemyBulletTimer = new DispatcherTimer();
                enemyBulletTimer.Interval = TimeSpan.FromSeconds(3);
                if(player != null)
                {
                    enemyBulletTimer.Tick += (s, e) => SpawnEnemyBullet();
                }
                
                enemyBulletTimer.Start();
            }
            
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
            if (player != null)
            {
                player.LocationX += 10;
            }
        }
        private void MoveLeft()//player move left
        {   if (player != null)
            {
                player.LocationX -= 10;
            }
            
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

                    var enemy = new EnemyViewModel(relativePathToAssets, 64, 64, 1, levelSpeed, false, -1);

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










        private void SpawnShield()//spawns two rows of shields
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




        private void SpawnRedSpaceShip()//SpawnRedSpaceShip every 15 seconds
        {
            if(player!=null)
            {
                Random _random = new Random();
                int randDir = _random.Next(0, 2);
                randDir = direction[randDir];
                string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersRedSpaceShipT.png";
                redSpaceShip = new RedSpaceShipViewModel(relativePathToAssets, 64, 64, randDir, 1, true);
                if (redSpaceShip.Direction == 1)
                {
                    redSpaceShip.SetPosition(0, 0);
                }
                else
                {
                    redSpaceShip.SetPosition(GameBoardWidth - redSpaceShip.Width, 0);
                }

                Actors.Add(redSpaceShip);
            }
            
        }

        private void DeSpawnRedSpaceShip()//DeSpawnRedSpaceShip if it passes the boarders 
        {
            if(redSpaceShip == null) return;
            else if(redSpaceShip.LocationX>GameBoardWidth - redSpaceShip.Width)
            {
                _actors.Remove(redSpaceShip);
                redSpaceShip = null;
            }
            else if (redSpaceShip.LocationX<-redSpaceShip.Width)
            {
                _actors.Remove(redSpaceShip);
                redSpaceShip = null;
            }
        }



        private void SpawnBullet()//Spawn player Bullet
        {
            if (player == null || bullet != null) return;

            string relativePathToAssets = "avares://AvaloniaSpaceInvaders/Assets/SpaceInvadersBullet.ico";
            bullet = new BulletViewModel(relativePathToAssets, 32, 32, 1, 1, true);
            bullet.SetPosition(player.LocationX, player.LocationY);
            Actors.Add(bullet);

        }




        private void DeSpawnBullet()//DeSpawnBullet if it passes the boarder
        {
            if (bullet!=null && bullet.LocationY < 0)
            {
                _actors.Remove(bullet);
                bullet = null;
            }
                
        }

        private void SpawnEnemyBullet()//Spawn Enemy Bullet,picks random enemy that does not have an active bullet 
        {
            if (_enemiesList.Count==0 ||_enemiesList[0].Count==0 ) 
                return;

            Random random =new Random();

            int rand = random.Next(0, _enemiesList[0].Count-1);

            while (_enemiesList[0][rand].IsAlive == true&& _enemiesList[0].Count>1)
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

        private void DeSpawnEnemyBullet()//DeSpawn Enemy Bullet if it passes the boarder
                                         //and sets the enemy bullet is alive to be false
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
            levelSpeed += 1;
            _actors.Clear();
            _Shileds.Clear();
            _Shileds2.Clear();
            _enemyBullets.Clear();
            player = null;
            bullet = null;
            SpawnPlayer();
            SpawnEnemies();
            SpawnShield();
            SpawnRedSpaceShip();
        }

        private void GameOverFunc()// game over clears the screen and saves the current score if its new high score
        {
            //bool highscore = false;
            if (Score >= HighScore)
            {

                HighScore = Score;
                HighScoreName = CurrentName;
                IsNameInputVisible = true;  // Show the input field
                //highscore = true;
                SaveHighScores();
            }
            //NavigateToGameResultScreen();
            _actors.Clear();
            _enemiesList.Clear();
            _Shileds.Clear();
            _Shileds2.Clear();
            _enemyBullets.Clear();
            player = null;
            bullet = null;
            gameOver = true;
            SubmitName();


        }

        private void SubmitName()//function is cold when game is over 
        {
            if (!string.IsNullOrWhiteSpace(PlayerNameInput))
            {
                _highScoreModel.HighScores.Add(new HighScoreEntry//adds a new HighScoreEntry to HighScores list
                {
                    Name = PlayerNameInput,
                    HighScore = Score
                });

                // Sort the list and keep only the top 5
                _highScoreModel.HighScores.Sort((x, y) => y.HighScore.CompareTo(x.HighScore));
                if (_highScoreModel.HighScores.Count > 5)
                {
                    _highScoreModel.HighScores.RemoveAt(_highScoreModel.HighScores.Count - 1);
                }

                SaveHighScores();//send HighScores list into a Serializer
                //NavigateToGameResultScreen();
            }
        }

        //private void NavigateToGameResultScreen()
        //{
        //    App.MainWindow.Content = new GameResultScreen();
        //}

        private void LoadHighScores()
        {
            try
            {
                if (File.Exists(HighScoreFilePath))
                {
                    var highScoreJson = File.ReadAllText(HighScoreFilePath);
                    _highScoreModel = JsonSerializer.Deserialize<HighScoreModel>(highScoreJson) ?? new HighScoreModel();
                }
            }
            catch (Exception ex)
            {
                _highScoreModel = new HighScoreModel();
            }
        }

        private void SaveHighScores()
        {
            try
            {
                var highScoreJson = JsonSerializer.Serialize(_highScoreModel);
                File.WriteAllText(HighScoreFilePath, highScoreJson);
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
            if (player != null)
            {
                player.LocationX = Math.Clamp(player.LocationX, 0, GameBoardWidth - player.Width);
            }
            

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
            if ((_Shileds.Count > 0 || _Shileds2.Count>0) && _enemiesList.Any(row => row.Count > 0))
            {
                doesEnemyBulletIntersect(_enemyBullets, _Shileds, _Shileds2);
            }

            // Determine closest and farthest enemies
            if (_enemiesList.Any(row => row.Count > 0))// go over all lists
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
            if (_enemiesList.All(row => row.Count == 0)&&gameOver!=true)
            {
                GameWonFunc();
            }

            // Game over check
            if (Lives <= 0&& gameOver!=true)
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
                if(row.Count != 0)
                {
                    var lastEnemyInRow = row[row.Count - 1];
                    if (lastEnemyInRow.LocationX > farestEnemy.LocationX)
                    {
                        farestEnemy = lastEnemyInRow;
                    }
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
                if (row.Count>0)
                {
                    var firstEnemyInRow = row[0];
                    if (firstEnemyInRow.LocationX < closestEnemy.LocationX)
                    {
                        closestEnemy = firstEnemyInRow;
                    }
                }
                
            }

            return closestEnemy;
        }






        /// <summary>
        /// Handles interactions between bullets, enemies, shields, and the player.
        /// </summary>
        private void doesIntersect(List<List<EnemyViewModel>> enemiesList, List<ShieldViewModel> shields, List<ShieldViewModel> shields2)
        {
            if (bullet != null)
            {
                // Check for bullet collisions with enemies, shields, and red spaceship
                CheckBulletEnemyCollision(enemiesList);
                CheckBulletShieldCollision(shields, shields2);
                CheckBulletRedSpaceShipCollision();
            }
            if (_enemiesList.All(row => row.Count != 0))
            {
                CheckEnemyPlayerCollision(enemiesList);
            }
        }




        private Random _random = new Random();

        /// <summary>
        /// Checks if the bullet collides with any enemy and handles the collision.
        /// </summary>
        /// <param name="enemiesList">List of enemy rows in the game.</param>
        private void CheckBulletEnemyCollision(List<List<EnemyViewModel>> enemiesList)
        {
            if (bullet == null) return;

            foreach (var enemyRow in enemiesList)
            {
                // Skip empty rows or rows where the bullet is not aligned with any enemy
                if (enemyRow.Count == 0 || bullet.LocationY < enemyRow[0].LocationY || bullet.LocationY > enemyRow[0].LocationY + enemyRow[0].Height)
                    continue;

                foreach (var enemy in enemyRow.ToList())
                {
                    if (bullet.Intersects(enemy))
                    {
                        // Mark the enemy as dead, remove it from the actors and row lists, and remove the bullet
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




        /// <summary>
        /// Checks if the bullet collides with any shield and handles the collision.
        /// </summary>
        /// <param name="shields">The first list of shields.</param>
        /// <param name="shields2">The second list of shields.</param>
        private void CheckBulletShieldCollision(List<ShieldViewModel> shields, List<ShieldViewModel> shields2)
        {
            if (bullet == null) return;

            // Check collisions for both sets of shields
            CheckShieldCollision(shields);
            CheckShieldCollision(shields2);
        }




        /// <summary>
        /// Checks if the bullet collides with a shield and handles the collision.
        /// </summary>
        /// <param name="shields">List of shields to check against.</param>
        private void CheckShieldCollision(List<ShieldViewModel> shields)
        {
            if (shields.Count == 0 || bullet == null ||
                (bullet.LocationY < shields[0].LocationY || bullet.LocationY > shields[0].LocationY + shields[0].Height)) return;

            foreach (var shield in shields.ToList())
            {
                if (bullet.Intersects(shield))
                {
                    // Handle the shield being hit, updating its state, and potentially removing it
                    shield.HitCount++;
                    if (shield.HitCount * 100 % 3 == 0)
                    {
                        shield.Source.Opacity = shield.HitCount * 100;
                    }

                    if (shield.Source.Opacity > 700)
                    {
                        _actors.Remove(shield);
                        shields.Remove(shield);
                    }
                    RemoveBullet();
                    return;
                }
            }
        }



        /// <summary>
        /// Checks if the bullet collides with the red spaceship and handles the collision.
        /// </summary>
        private void CheckBulletRedSpaceShipCollision()
        {
            if (bullet == null || redSpaceShip == null || bullet.LocationY != redSpaceShip.LocationY) return;

            if (bullet.Intersects(redSpaceShip))
            {
                // Remove the red spaceship and the bullet, update the score
                _actors.Remove(redSpaceShip);
                redSpaceShip = null;
                Score += _random.Next(40, 101);
                RemoveBullet();
            }
        }



        /// <summary>
        /// Checks if any enemies collide with the player and handles the collision.
        /// </summary>
        /// <param name="enemiesList">List of enemy rows in the game.</param>
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
                        // Set lives to zero and trigger game over
                        Lives = 0;
                        GameOverFunc();
                        return;
                    }
                }
            }
        }



        /// <summary>
        /// Removes the player bullet from the game actors and sets it to null.
        /// </summary>
        private void RemoveBullet()
        {
            _actors.Remove(bullet);
            bullet = null;
        }




        /// <summary>
        /// Checks if any enemy bullets intersect with shields or the player and handles the results.
        /// </summary>
        /// <param name="enemyBullets">List of enemy bullets in the game.</param>
        /// <param name="shields">List of the first set of shields in the game.</param>
        /// <param name="shields2">List of the second set of shields in the game.</param>
        private void doesEnemyBulletIntersect(List<EnemyBulletViewModel> enemyBullets, List<ShieldViewModel> shields, List<ShieldViewModel> shields2)
        {
            var bulletsToRemove = new List<EnemyBulletViewModel>();

            // Iterate over each enemy bullet to check for intersections
            foreach (var enemyBullet in enemyBullets)
            {
                // Check for intersections with the shields
                if (CheckAndHandleShieldIntersection(enemyBullet, shields) || CheckAndHandleShieldIntersection(enemyBullet, shields2))
                {
                    bulletsToRemove.Add(enemyBullet);
                    continue;
                }

                // Check for intersections with the player
                if (CheckAndHandlePlayerIntersection(enemyBullet))
                {
                    bulletsToRemove.Add(enemyBullet);
                    break; // Stop further processing if a bullet hits the player
                }
            }

            // Remove bullets that have been processed
            foreach (var bullet in bulletsToRemove)
            {
                RemoveBullet(bullet);
            }
        }



        /// <summary>
        /// Checks if an enemy bullet intersects with any shield in the provided list and handles the intersection.
        /// </summary>
        /// <param name="enemyBullet">The enemy bullet to check.</param>
        /// <param name="shields">The list of shields to check against.</param>
        /// <returns>True if an intersection occurs, otherwise false.</returns>
        private bool CheckAndHandleShieldIntersection(EnemyBulletViewModel enemyBullet, List<ShieldViewModel> shields)
        {
            if (shields.Count > 0 && enemyBullet.LocationY >= shields[0].LocationY)
            {
                foreach (var shield in shields)
                {
                    if (enemyBullet.Intersects(shield))
                    {
                        HandleShieldHit(shield);
                        setIsAlive(enemyBullet.Id);
                        return true;
                    }
                }
            }
            return false;
        }



        /// <summary>
        /// Checks if an enemy bullet intersects with the player and handles the intersection.
        /// </summary>
        /// <param name="enemyBullet">The enemy bullet to check.</param>
        /// <returns>True if an intersection occurs, otherwise false.</returns>
        private bool CheckAndHandlePlayerIntersection(EnemyBulletViewModel enemyBullet)
        {
            if (enemyBullet.LocationY >= player.LocationY && enemyBullet.Intersects(player))
            {
                Lives--;
                setIsAlive(enemyBullet.Id);
                return true;
            }
            return false;
        }




        /// <summary>
        /// Handles the event when a shield is hit by an enemy bullet.
        /// </summary>
        /// <param name="shield">The shield that was hit.</param>
        private void HandleShieldHit(ShieldViewModel shield)
        {
            shield.HitCount++;
            if (shield.HitCount * 100 % 3 == 0)
            {
                shield.Source.Opacity = shield.HitCount * 100;
            }
            if (shield.Source.Opacity > 700)
            {
                _actors.Remove(shield);
            }
        }




        /// <summary>
        /// Removes the specified bullet from the lists of active bullets and game actors.
        /// </summary>
        /// <param name="enemyBullet">The bullet to remove.</param>
        private void RemoveBullet(EnemyBulletViewModel enemyBullet)
        {
            _enemyBullets.Remove(enemyBullet);
            _actors.Remove(enemyBullet);
        }




        /// <summary>
        /// Sets the IsAlive status of an enemy associated with a bullet ID to false.
        /// </summary>
        /// <param name="enemyId">The ID of the enemy whose status is to be updated.</param>
        private void setIsAlive(int enemyId)
        {
            for (int i = 0; i < _enemiesList[0].Count - 1; i++)
            {
                if (_enemiesList[0][i].Id == enemyId)
                {
                    _enemiesList[0][i].IsAlive = false;
                }
            }
        }

    }
}
