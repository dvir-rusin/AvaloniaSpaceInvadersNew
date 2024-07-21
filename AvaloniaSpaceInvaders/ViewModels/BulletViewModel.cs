using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaSpaceInvaders.Models;
namespace AvaloniaSpaceInvaders.ViewModels
{
    public class BulletViewModel : ActorViewModel
    {
        public BulletViewModel(Bullet bullet) : base(bullet) { }

        public override void Move()
        {
            // Implement bullet movement logic
        }
    }
}
