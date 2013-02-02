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
    public class TilemapRenderer : DrawableGameComponent, ITilemapRenderer
    {
        Tilemap map;
        public Tilemap Map
        {
            get { return map; }
            set
            {
                if (map != null && collisionManager != null)
                    foreach (TaggedVector p in map.CollidableTiles)
                        collisionManager.RemoveTile((Tile)p.Identifier);

                map = value;

                if(collisionManager != null)
                    foreach (TaggedVector p in map.CollidableTiles)
                        collisionManager.AddTile(p.Position, (Tile)p.Identifier);
            }
        }
        public Tileset Tileset { get; set; }

        SpriteBatch spriteBatch;
        ICollisionManager collisionManager;
        GameCamera camera;

        public TilemapRenderer(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            collisionManager = (ICollisionManager)Game.Services.GetService(typeof(ICollisionManager));
            camera = (GameCamera)Game.Services.GetService(typeof(GameCamera));

            Game.Services.AddService(typeof(ITilemapRenderer), this);

            if (map != null)
                foreach (TaggedVector p in map.CollidableTiles)
                    collisionManager.AddTile(p.Position, (Tile)p.Identifier);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Map != null && Tileset != null)
            {
                Vector2 destination = new Vector2();

                spriteBatch.Begin();

                for (int x = 0; x < Map.Width; x++)
                {
                    for (int y = 0; y < Map.Height; y++)
                    {
                        if (Map[x, y].TextureID > -1)
                        {
                            destination.X = x * Tileset.TileWidth;
                            destination.Y = y * Tileset.TileHeight;

                            spriteBatch.Draw(Tileset.Texture, camera.DrawOffset + destination, Tileset[Map[x, y].TextureID], Color.White);
                        }
                    }
                }

                spriteBatch.End();

            }

            base.Draw(gameTime);
        }


        public Vector2 GetPlayerSpawn()
        {
            return Tileset.ConvertTileToWorldCoordinates(Map.SpawnTilePosition) + new Vector2(12 - 4f, 0);
        }

        public List<Vector2> GetEscapeeSpawns()
        {
            List<Vector2> transformedEscapeeSpawns = new List<Vector2>();
            foreach (Vector2 spawn in Map.EnemySpawnPositions)
                transformedEscapeeSpawns.Add(Tileset.ConvertTileToWorldCoordinates(spawn) + new Vector2(12 - 4f, 0));

            return transformedEscapeeSpawns;
        }

        public Vector2 EscapeDoor
        {
            get { return Tileset.ConvertTileToWorldCoordinates(Map.EscapeDoor); }
        }

        public Rectangle EscapeDoorHitbox
        {
            get
            {
                return new Rectangle((int)EscapeDoor.X, (int)EscapeDoor.Y, 24, 24);
            }
        }
    }
}
