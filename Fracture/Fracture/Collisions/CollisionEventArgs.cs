using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fracture
{
    public class CollisionEventArgs : EventArgs
    {
        public bool TileCollision { get; protected set; }
        public bool KillTile { get; protected set; }
        public ICollidable CollidedWith { get; protected set; }

        public CollisionEventArgs(bool KillTile = false)
        {
            TileCollision = true;
            this.KillTile = KillTile;
        }

        public CollisionEventArgs(ICollidable collidedWith)
        {
            TileCollision = false;
            CollidedWith = collidedWith;
        }
    }
}
