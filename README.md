# Space_Invaders_Avalonia
Space Invaders in Avalonia


## Summary

###  creating a port of space invaders using Avalonia.

  ### Title Screen
  - Button to start the game 
  ### Game Screen
  - The board contains 55 aliens 5 rows and 11 columns.
  - On the top of the board there is a score count, and a lives count.
  - The board  contains 3 destructable shields.
  ### Alien Logic
  - The aliens moves on an x axis starting from left to right, when one of the aliens on the edge of the screen hit the edge the aliens will drop one row down and then turn around.
  - The aliens shoot randomly at the player, their bombs can damage the shields but not the aliens themselves.
  - When the aliens hit the edge or when an alien dies they should move faster.
  - when the aliens reach the ground the game is over
  - If all the aliens are destroyed new level begins, and the aliens will be closer to the player in later levels.
  - A random bonus alien ship will appear on the top of the screen and should move faster than the aliens.
  ### Shield Logic
  - The three shields are destructible, the destructibility is split to a grid
  - Both the aliens and the player can destory the shields
  ### Player logic
  - The player moves on the x-axis.
  - The player shoots one missile at a time.
  - There can only be 1 missle on the screen at the time for the player.
  - The player has 3 lives, when hit by a bomb the player dies. When respawning the player the game should freeze until respawn
  ### Scoring 
  - There are points for killing aliens, basic aliens give 10 points, medium aliens give 20 points and hard aliens give 40 points.
  - When killing a bonus alien ship give a random number of points from these points (50,100,150,200,300)
  ### High score system 
  - When the game is over a highscore leaderboard appears with the top 5 players






![Space Invaders start screen](https://github.com/user-attachments/assets/e160fec7-2907-4a15-bce5-f46c3f66d7bd)






![Space Invaders main screen](https://github.com/user-attachments/assets/d440af2a-9ef8-4d53-ba50-bfee12507e70)





![Space Invaders leaderboard screen](https://github.com/user-attachments/assets/b52d2a93-3293-4d19-a5e0-4a46b252f449)






