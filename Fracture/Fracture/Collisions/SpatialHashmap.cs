using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fracture
{
    public class SpatialHashmap
    {
        public int CellHeight { get; protected set; }
        public int CellWidth { get; protected set; }

        Dictionary<Point, List<TaggedVector>> cells;
        Dictionary<object, Point> previousEntityPosition;

        public SpatialHashmap(int cellHeight, int cellWidth)
        {
            cells = new Dictionary<Point, List<TaggedVector>>();
            previousEntityPosition = new Dictionary<object, Point>();
            CellHeight = cellHeight;
            CellWidth = cellWidth;
        }

        private Point Hash(TaggedVector point)
        {
            Point p = new Point(((int)point.Position.X) / CellWidth, ((int)point.Position.Y) / CellHeight);

            return p;
        }

        private Point Hash(Vector2 point)
        {
            Point p = new Point(((int)point.X) / CellWidth, ((int)point.Y) / CellHeight);

            return p;
        }

        public void Update(object obj, Vector2 position)
        {
            if (!previousEntityPosition.ContainsKey(obj))
                throw new Exception("The specified object is not being managed with this hashmap");

            Point entityHash = Hash(position);

            if (previousEntityPosition[obj] != entityHash)
            {
                if (!cells.ContainsKey(entityHash))
                    cells.Add(entityHash, new List<TaggedVector>());

                cells[previousEntityPosition[obj]].RemoveAll(new Predicate<TaggedVector>((x) => x.Identifier == obj));
                cells[entityHash].Add(new TaggedVector { Position = position, Identifier = obj });
                previousEntityPosition[obj] = entityHash;
            }
        }

        public void Add(TaggedVector obj)
        {
            if(previousEntityPosition.ContainsKey(obj.Identifier))
                throw new Exception("The specified object is already being managed with this hashmap");

            Point entityHash = Hash(obj);

            if(!cells.ContainsKey(entityHash))
                cells.Add(entityHash, new List<TaggedVector>());

            cells[entityHash].Add(obj);
            previousEntityPosition.Add(obj.Identifier, entityHash);
        }

        public void Remove(object obj)
        {
            if (!previousEntityPosition.ContainsKey(obj))
                throw new Exception("The specified object is not being managed with this hashmap");

            cells[previousEntityPosition[obj]].RemoveAll(new Predicate<TaggedVector>((x) => x.Identifier == obj));
            previousEntityPosition.Remove(obj);
        }

        static readonly Point[] CELL_OFFSETS = new[] { new Point(0, 1), new Point(1, 1), new Point(1, 0), new Point(1, -1), new Point(0, -1), new Point(-1, -1), new Point(-1, 0) };

        public IEnumerable<TaggedVector> GetNeighbouringObjects(Vector2 obj)
        {
            Point p = Hash(obj);

            IEnumerable<TaggedVector> primaryEnumerable = cells[p].Where((x) => x.Position != obj);

            Point currPoint = Point.Zero;
            foreach (Point offset in CELL_OFFSETS)
            {
                currPoint.X = p.X + offset.X;
                currPoint.Y = p.Y + offset.Y;

                if (cells.ContainsKey(currPoint))
                    primaryEnumerable.Concat(cells[currPoint]);
            }

            return primaryEnumerable;
        }
    
    }
}
