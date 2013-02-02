using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public class GameCamera
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        Vector2 focus;
        public Vector2 Focus
        {
            get { return focus; }
            set
            {
                focus = value;
                DrawOffset = new Vector2((int)-(focus.X - (Width / 2)), (int)-(focus.Y - (Height / 2)));
            }
        }

        public Vector2 DrawOffset { get; protected set; }
    }
}
