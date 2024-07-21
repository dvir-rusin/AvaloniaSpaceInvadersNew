using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaSpaceInvaders.Models;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public class PlayerViewModel : ActorViewModel
    {
        public PlayerViewModel(Player player) : base(player) 
        {
        
        }
        public double getX(Player player)
        {
            return player.LocationX;
        }

        public double getY(Player player)
        {
            return player.LocationY;
        }

        public override void Move()
        {
            // Implement player movement logic
        }
    }
}
