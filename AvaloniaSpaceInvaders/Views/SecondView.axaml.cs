using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaSpaceInvaders.objects;
using AvaloniaSpaceInvaders.Objects;
using AvaloniaSpaceInvaders.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace AvaloniaSpaceInvaders.Views;

public partial class SecondView : UserControl
{
    private Player player;
    private List<Enemy> enemies;
    private Bullet playerBullet;
    private Bullet enemyBullet;
    private List<Shield> shields;
    private int score;
    private int lives;
    private bool isGameOver;
    private Canvas gameCanvas;
    private DispatcherTimer gameTimer;
    public SecondView()
    {
        InitializeComponent();

        InitGame();

    }

    private void InitGame()
    {
        score = 0;
        lives = 3;
        isGameOver = false;

        player = new Player { X = 200, Y = 400 };
        enemies = new List<Enemy>();
        playerBullet = new Bullet();
        enemyBullet = new Bullet();
        shields = new List<Shield>();

        CreateEnemies();
        CreateShields();
        
    }

    private void CreateEnemies()
    {
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 11; col++)
            {
                enemies.Add(new Enemy { X = col * 40, Y = row * 40 });
            }
        }
    }

    private void CreateShields()
    {
        shields.Add(new Shield { X = 50, Y = 300 });
        shields.Add(new Shield { X = 150, Y = 300 });
        shields.Add(new Shield { X = 250, Y = 300 });
        shields.Add(new Shield { X = 350, Y = 300 });
    }

    private void OnRendering(object sender, EventArgs e)
    {
        if (isGameOver) return;

        UpdateGame();
        gameCanvas.InvalidateVisual();
    }

    private void GamePage_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter || e.Key == Key.Space || e.Key == Key.Return)
        {
            //TitleScreen.IsVisible = false;

            //GameScreen.IsVisible = true;
            StartGameLoop();
        }
    }

    private void StartGameLoop()
    {
        gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // Approximately 60 FPS
        };
        gameTimer.Tick += GameLoop;
        gameTimer.Start();
    }

    private void GameLoop(object sender, EventArgs e)
    {
        if (isGameOver)
        {
            gameTimer.Stop();
            return;
        }

        UpdateGame();

        gameCanvas.InvalidateVisual();
    }

    private void UpdateGame()
    {
        // Update player movement
        // Update alien movement
        // Update bullets
        // Check collisions
        CheckCollisions();

        // Update game state
    }

    private void UpdateBulletPos()
    {
        playerBullet.X = player.X;
        playerBullet.Y = player.Y;
    }

    private void CheckCollisions()
    {
        foreach (var enemy in enemies)
        {
            if (playerBullet.Intersects(enemy))
            {
                UpdateScore(40);
                enemies.Remove(enemy);
            }

        }

        if (enemyBullet.Intersects(player))
        {
            LoseLife();
        }

        foreach (var shield in shields)
        {
            if (enemyBullet.Intersects(shield))
            {
                shield.TakeDamage(enemyBullet.X, enemyBullet.Y);
            }
            if (playerBullet.Intersects(shield))
            {
                shield.TakeDamage(playerBullet.X,playerBullet.Y);
            }
        }


        // Check for collisions between bullets and enemies, shields, player, etc.
    }
    
    public override void Render(DrawingContext context)
    {
        base.Render(context);
        var canvas = context;

        if (player != null)
            player.Draw(canvas);

        foreach (var enemy in enemies)
        {
            enemy.Draw(canvas);
        }

        if (enemyBullet != null)
            enemyBullet.Draw(canvas);

        if (playerBullet != null)
            playerBullet.Draw(canvas);

        foreach (var shield in shields)
        {
            shield.Draw(canvas);
        }
    }
    



    private void UpdateScore(int points)
    {
        score += points;
        var scoreLabel = this.FindControl<TextBlock>("ScoreLabel");
        scoreLabel.Text = $"Score: {score}";
    }

    private void LoseLife()
    {
        lives--;
        var livesLabel = this.FindControl<TextBlock>("LivesLabel");
        livesLabel.Text = $"Lives: {lives}";

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        // Display game over message and handle restart logic
    }

}

