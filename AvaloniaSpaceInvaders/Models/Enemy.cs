using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSpaceInvaders.Models
{
    
    public class Enemy : Actor
    {
        public bool IsAlive { get; set; } = true;
        public Enemy(string imagePath, int width, int height, bool isAlive)
            : base(imagePath, width, height) 
        {
            IsAlive = isAlive;
        }
    }
}
