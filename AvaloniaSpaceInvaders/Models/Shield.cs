﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSpaceInvaders.Models
{
    public class Shield : Actor
    {
        
        public int? Direction { get; set; } = null;
        public int? Speed { get; set; } = null;
        public bool IsAlive { get; set; } = true;

        public int? HitCount { get; set; } = null;

        public Shield(string imagePath, int width, int height, int direction, int speed, bool isalive)
            : base(imagePath, width, height , isalive) 
        {
            IsAlive = isalive;
            Speed = speed;
            Direction = direction;
        }
        
    }
}
