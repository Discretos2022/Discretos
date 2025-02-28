using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9.Objects
{
    public class SnowTrigger : TriggerAction
    {
        public override void Action()
        {
            Play.Wind = new Vector2(4, 0);
        }
    }
}
