﻿using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSpaceInvaders.Objects
{
    public class Enemy : GameObjects
    {
        public Enemy()
        {
            Width = 30;
            Height = 20;
            Fill = Brushes.Red;
        }

        public void Move(int deltaX, int deltaY)
        {
            X += deltaX;
            Y += deltaY;
        }
    }
}
