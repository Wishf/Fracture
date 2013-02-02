using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public class Player : RainbowMan
    {
        public Player()
        {
            collidable = true;
            hitbox = new Rectangle(0, 0, 15, 15);
            Tint = Color.Black;
            Alive = true;
        }
    }
}
