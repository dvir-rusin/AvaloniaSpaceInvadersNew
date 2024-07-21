using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSpaceInvaders.Models
{
    public class Player : Actor
    {
        public Player(string imagePath, int width, int height)
            : base(imagePath, width, height) { }
    }
}
