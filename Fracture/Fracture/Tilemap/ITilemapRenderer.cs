using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public interface ITilemapRenderer
    {
        Tilemap Map { get; set; }
        Tileset Tileset { get; set; }

        Vector2 GetPlayerSpawn();
        List<Vector2> GetEscapeeSpawns();
        Vector2 EscapeDoor { get; }
        Rectangle EscapeDoorHitbox { get; }
    }
}
