using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9.Core
{
    public struct Hitbox
    {

        public bool isEnabled = true;
        public bool isSolid = true;

        public Rectangle rectangle;


        public Hitbox(int x, int y, int width, int height)
        {
            rectangle = new Rectangle(x, y, width, height);
        }

        

    }



}
