using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public interface ICollisionManager
    {
        void AddTile(Vector2 tilePosition, Tile t);
        void RemoveTile(Tile t);

        void AddEntity(ICollidable entity);
        void RemoveEntity(ICollidable entity);

        int TileWidth { get; set; }
        int TileHeight { get; set; }
    }
}
