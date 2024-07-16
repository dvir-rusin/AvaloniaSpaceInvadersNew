using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSpaceInvaders.Objects
{
    public class Shield : GameObjects
    {
        private int[,] damageGrid;

        public Shield()
        {
            Width = 60;
            Height = 40;
            Fill = Brushes.Green; // Shield color
            InitializeDamageGrid();
        }

        private void InitializeDamageGrid()
        {
            damageGrid = new int[4, 3];
            
        }
        

        public void TakeDamage(int x, int y)
        {
            int gridX = (int)(x / (Width / 4));
            int gridY = (int)(y / (Height / 3));
            if (gridX >= 0 && gridX < 4 && gridY >= 0 && gridY < 3)
            {
                damageGrid[gridX, gridY]++;
            }
        }

        public override void Draw(DrawingContext context)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (damageGrid[i, j] < 3)
                    {
                        float rectX = X + i * (Width / 4);
                        float rectY = Y + j * (Height / 3);
                        Rect rect = new Rect(rectX, rectY, Width / 4, Height / 3);
                        context.FillRectangle(Fill, rect);
                    }
                }
            }
        }
    }
}
