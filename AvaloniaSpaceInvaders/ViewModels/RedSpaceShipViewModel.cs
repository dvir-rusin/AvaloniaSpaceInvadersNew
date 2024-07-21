using AvaloniaSpaceInvaders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AvaloniaSpaceInvaders.ViewModels
{
    public class RedSpaceShipViewModel : ActorViewModel
    {
        public RedSpaceShipViewModel(RedSpaceShip redSpaceShip) : base(redSpaceShip) { }

        public override void Move()
        {
            // Implement red spaceship movement logic
        }
    }
}
