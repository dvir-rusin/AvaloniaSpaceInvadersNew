using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSpaceInvaders.Objects
{
 

        public class Bullet : GameObjects
        {
            private bool isAlive;

            public Bullet()
            {
                Y = 0;
                X = 0;
                Width = 5;
                Height = 10;
                Fill = Brushes.Yellow; // bullet color
                isAlive = true;


            }

            public void Move(int deltaY)
            {
                Y += deltaY;
            }


            
        }
    
}
