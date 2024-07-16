using AvaloniaSpaceInvaders.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSpaceInvaders.objects
{
    public class Player : GameObjects
    {
        public Player()
        {

            Width = 40;
            Height = 20;
            //Paint.Color = SKColors.Blue;
        }

        public void MoveLeft()
        {
            X -= 10; // Adjust the movement speed as needed
        }

        public void MoveRight()
        {
            X += 10; // Adjust the movement speed as needed
        }

        public void Shoot()
        {
            //UpdateBulletPos();
        }
    }
}
