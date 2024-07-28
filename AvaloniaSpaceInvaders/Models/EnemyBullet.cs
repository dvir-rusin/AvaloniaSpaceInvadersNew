using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSpaceInvaders.Models
{
    public class EnemyBullet : Actor
    {
        public int? Id { get; set; } = -1;
        public int? Direction { get; set; } = null;
        public int? Speed { get; set; } = null;
        public bool IsAlive { get; set; } = true;
        public EnemyBullet(string imagePath, int width, int height, int direction, int speed, bool isalive,int id)
            : base(imagePath, width, height, isalive)

        {
            IsAlive = isalive;
            Speed = speed;
            Direction = direction;
            Id = id;
        }
    }
}
