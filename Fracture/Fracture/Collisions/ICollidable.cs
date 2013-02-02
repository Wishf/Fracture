using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public interface ICollidable
    {
        bool Collidable { get; }
        Rectangle Hitbox { get; }

        Vector2 PendingMovement { get; set; }
        Vector2 Position { get; set; }

        event EventHandler<CollisionEventArgs> Collided;
        void InvokeCollision(CollisionEventArgs args);

        void Offset(float x, float y);
        void CommitMovement();
    }
}
