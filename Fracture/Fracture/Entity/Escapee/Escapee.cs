using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public class Escapee : RainbowMan
    {
        public static readonly Color[] Tints = new Color[] { new Color { R = 255, G = 69, B = 32, A = 255 }, new Color { R = 95, G = 255, B = 32, A = 255 }, new Color { R = 32, G = 105, B = 255, A = 255 } };

        public int DeathTimer { get; set; }

        public Escapee()
        {
            collidable = true;
            hitbox = new Rectangle(0, 0, 15, 15);
        }
    }
}
