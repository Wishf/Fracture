using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public class Tileset
    {
        public int TileWidth { get; protected set; }
        public int TileHeight { get; protected set; }
        public Texture2D Texture { get; protected set; }

        int tileCount, tilesPerRow;
        Rectangle[] tileBoxes;

        public Rectangle this[int id]
        {
            get
            {
                if (id < tileCount && id > -1)
                {
                    if (tileBoxes[id] == Rectangle.Empty)
                        GenerateTilebox(id);

                    return tileBoxes[id];
                }

                throw new Exception("Index out of bounds of tileset");
            }
        }

        private void GenerateTilebox(int id)
        {
            int x = (id % tilesPerRow)*TileWidth;
            int y = (id / tilesPerRow)*TileHeight;

            tileBoxes[id] = new Rectangle(x,y, TileWidth, TileHeight);
        }

        public Tileset(Texture2D texture, int tWidth, int tHeight)
        {
            Texture = texture;
            TileWidth = tWidth;
            TileHeight = tHeight;

            tilesPerRow = texture.Width / tWidth;
            tileCount = (texture.Height / tHeight) * tilesPerRow;

            tileBoxes = new Rectangle[tileCount];
        }

        public Vector2 ConvertTileToWorldCoordinates(Vector2 tileCoordinates)
        {
            Vector2 wCoords = new Vector2();
            wCoords.X = tileCoordinates.X * TileWidth;
            wCoords.Y = tileCoordinates.Y * TileHeight;

            return wCoords;
        }
    }
}
