using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Fracture
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CollisionManager : GameComponent, ICollisionManager
    {
        List<Tile> collidableTiles;
        List<ICollidable> collidableEntities;

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public Vector2 Gravity { get; set; }

        SpatialHashmap hashmap;

        public CollisionManager(Game game, int tWidth, int tHeight)
            : base(game)
        {
            // TODO: Construct any child components here
            hashmap = new SpatialHashmap(64, 64);
            TileWidth = tWidth;
            TileHeight = tHeight;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            collidableEntities = new List<ICollidable>();
            collidableTiles = new List<Tile>();

            Game.Services.AddService(typeof(ICollisionManager), this);

            Gravity = new Vector2(0, 980f);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Rectangle tileHitbox = new Rectangle(0,0, TileWidth, TileHeight);
            Rectangle shiftedHitbox = Rectangle.Empty;
            Rectangle intersectBox = Rectangle.Empty;
            Vector2 shift = Vector2.Zero;
            
            foreach(ICollidable e in collidableEntities)
            {
                shiftedHitbox = e.Hitbox;

                foreach (TaggedVector v in hashmap.GetNeighbouringObjects(new Vector2(e.Hitbox.X, e.Hitbox.Y)))
                {
                    shiftedHitbox.Offset((int)e.PendingMovement.X, (int)e.PendingMovement.Y);

                    // tile collisions
                    if (v.Identifier is Tile)
                    {
                        tileHitbox.X = (int)v.Position.X;
                        tileHitbox.Y = (int)v.Position.Y;

                        if (tileHitbox.Intersects(shiftedHitbox))
                        {
                            e.InvokeCollision(new CollisionEventArgs(((Tile)v.Identifier).KillOnCollide));

                            intersectBox = Rectangle.Intersect(shiftedHitbox, tileHitbox);

                            // determine axis of intersection
                            bool xax = false, yax = false;

                            if (e.PendingMovement.X > 0) xax = true;
                            if (e.PendingMovement.Y > 0) yax = true;

                            // determine intersection corner
                            bool top = false, left = false;

                            if ((tileHitbox.Top - intersectBox.Top) < (intersectBox.Top - tileHitbox.Bottom)) top = true;
                            if ((tileHitbox.Left - intersectBox.Left) < (intersectBox.Left - tileHitbox.Right)) left = true;

                            // right hand side push left (negative change)
                            // left hand side push right (positive change)
                            // top push down (positive change)
                            // bottom push up (negative change)

                            if (xax) shift.X = left ? intersectBox.Width : -intersectBox.Width;
                            if (yax) shift.Y = top ? intersectBox.Height : -intersectBox.Height;

                            e.PendingMovement += shift;

                            shift = Vector2.Zero;
                        }
                    }

                    // entity collisions
                    else
                    {
                        if(((ICollidable)v.Identifier).Hitbox.Intersects(shiftedHitbox))
                            e.InvokeCollision(new CollisionEventArgs(((ICollidable)v.Identifier)));
                    }
                }

                e.CommitMovement();
                hashmap.Update(e, e.Position);
            }

            base.Update(gameTime);
        }

        public void AddTile(Vector2 tilePosition, Tile t)
        {
            if (!collidableTiles.Contains(t))
            {
                collidableTiles.Add(t);
                hashmap.Add(new TaggedVector { Position = new Vector2(tilePosition.X * TileWidth, tilePosition.Y * TileHeight),Identifier = t });
            }
        }

        public void RemoveTile(Tile t)
        {
            if (collidableTiles.Contains(t))
            {
                collidableTiles.Remove(t);
                hashmap.Remove(t);
            }
        }

        public void AddEntity(ICollidable entity)
        {
            if (!collidableEntities.Contains(entity))
            {
                collidableEntities.Add(entity);
                hashmap.Add(new TaggedVector { Position = new Vector2(entity.Hitbox.X, entity.Hitbox.Y),Identifier = entity });
            }
        }

        public void RemoveEntity(ICollidable entity)
        {
            if (collidableEntities.Contains(entity))
            {
                collidableEntities.Remove(entity);
                hashmap.Remove(entity);
            }
        }
    }
}
