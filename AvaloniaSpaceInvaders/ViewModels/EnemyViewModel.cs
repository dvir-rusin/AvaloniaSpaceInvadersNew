using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaSpaceInvaders.Models;
namespace AvaloniaSpaceInvaders.ViewModels
{
    public class EnemyViewModel : ActorViewModel
    {
        public EnemyViewModel(Enemy enemy) : base(enemy) { }

        public override void Move()
        {
            // Implement enemy movement logic
        }
    }
}
