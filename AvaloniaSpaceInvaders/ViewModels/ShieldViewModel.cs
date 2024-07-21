using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaSpaceInvaders.Models;

namespace AvaloniaSpaceInvaders.ViewModels
{
    public class ShieldViewModel : ActorViewModel
    {
        public ShieldViewModel(Shield shield) : base(shield) { }

        public override void Move()
        {
            // Implement shield movement logic if any
        }
    }
}
