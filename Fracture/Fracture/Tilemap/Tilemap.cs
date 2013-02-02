using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public class Tile
    {
        public bool Collidable { get; set; }
        public int TextureID { get; set; }
        public bool KillOnCollide { get; set; }
    }

    public class Tilemap
    {
        Tile[] tiles;

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Vector2 SpawnTilePosition { get; protected set; }
        public List<Vector2> EnemySpawnPositions { get; protected set; }
        public Vector2 EscapeDoor { get; protected set; }

        public Tile this[int x, int y]
        {
            get
            {
                return tiles[(y * Width) + x];
            }

            set
            {
                tiles[(y * Width) + x] = value;
            }
        }

        public Tilemap(int width, int height)
        {
            Width = width;
            Height = height;

            tiles = new Tile[width * height];
            SpawnTilePosition = -Vector2.One;
            EnemySpawnPositions = new List<Vector2>();
            CollidableTiles = new List<TaggedVector>();
        }

        public List<TaggedVector> CollidableTiles { get; protected set; }

        public static Tilemap Load(string path)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(new StreamReader(File.Open(path, FileMode.Open)));

            XmlNode meta = xDoc.GetElementsByTagName("map")[0];
            int width = int.Parse(meta.Attributes["width"].InnerText);
            int height = int.Parse(meta.Attributes["height"].InnerText);

            XmlNode data = xDoc.GetElementsByTagName("layer")[0].FirstChild;
            byte[] decodedData = Convert.FromBase64String(data.InnerText);

            int[] tileData = new int[decodedData.Length / 4];

            for (int i = 0; i < tileData.Length; i++)
            {
                tileData[i] = (int)BitConverter.ToUInt32(decodedData, 4 * i) - 1;
            }

            Tilemap map = new Tilemap(width, height);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (tileData[(y * width) + x] == 1)
                    {
                        if (map.SpawnTilePosition == -Vector2.One)
                            map.SpawnTilePosition = new Vector2(x, y);
                    }
                    else if (tileData[(y * width) + x] == 2)
                    {
                        map.EscapeDoor = new Vector2(x, y);
                    }
                    else if (tileData[(y * width) + x] == 3)
                    {
                        map.EnemySpawnPositions.Add(new Vector2(x, y));
                    }

                    if (tileData[(y * width) + x] == 0)
                    {
                        map[x, y] = new Tile { Collidable = true, TextureID = 0 };
                        map.CollidableTiles.Add(new TaggedVector { Position = new Vector2(x, y), Identifier = map[x, y] });
                    }
                    else if ((tileData[(y * width) + x] > 8 && tileData[(y * width) + x] < 19) || tileData[(y * width) + x] == 7)
                    {
                        map[x, y] = new Tile { Collidable = true, TextureID = tileData[(y * width) + x], KillOnCollide = true };
                        map.CollidableTiles.Add(new TaggedVector { Position = new Vector2(x, y), Identifier = map[x, y] });
                    }
                    else
                        map[x, y] = new Tile { Collidable = false, TextureID = tileData[(y * width) + x] };
                }

            return map;
        }
    }
}
