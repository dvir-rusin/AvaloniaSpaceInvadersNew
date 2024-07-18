using Avalonia.Media;
using AvaloniaSpaceInvaders.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;


namespace AvaloniaSpaceInvaders.Objects
{
 

        public class Bullet : GameObjects
        {
        private bool _bulletActive=false;

        public Bullet()
            {
                Y = 0;
                X = 0;
                Width = 5;
                Height = 10;
                Fill = Brushes.Yellow; // bullet color
                


            }

            public void Move(int deltaY)
            {
                Y += deltaY;
            }

        private void Shoot(Bullet bullet,Player player)
        {
            if (!bullet._bulletActive)
            {
                // Reset bullet position to the player's position
                bullet.X = player.X + player.Width / 2 - bullet.Width / 2;
                bullet.Y = player.Y - bullet.Height;
                _bulletActive = true;
                // Setup an observable to move the bullet upwards
                Observable.Interval(TimeSpan.FromMilliseconds(50))
                          .Where(_ => bullet.Y > -bullet.Height)
                          .Subscribe(_ => MoveBulletUp(bullet));
            }
        }

        private void MoveBulletUp(Bullet bullet)
        {
            bullet.Move(-10); // Adjust the speed as needed


        }



    }
    
}
