using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public abstract class Entity: ICollidable
    {
        Vector2 pos;
        public Vector2 Position
        {
            get { return pos; }
            set
            {
                pos = value;
                hitbox.X = (int)pos.X;
                hitbox.Y = (int)pos.Y;
            }
        }

        internal bool collidable;
        public bool Collidable
        {
            get { return collidable; }
        }

        internal Rectangle hitbox;
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public event EventHandler<CollisionEventArgs> Collided;

        public void InvokeCollision(CollisionEventArgs args)
        {
            if (Collided != null)
                Collided(this, args);
        }

        public void Offset(float x, float y)
        {
            pos.X += x;
            pos.Y += y;
        }

        public Vector2 PendingMovement { get; set; }


        public void CommitMovement()
        {
            Position += PendingMovement;
            PendingMovement = Vector2.Zero;
        }
    }
}
