using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public class TaggedVector
    {
        public Vector2 Position { get; set; }
        public object Identifier { get; set; }
    }
}
