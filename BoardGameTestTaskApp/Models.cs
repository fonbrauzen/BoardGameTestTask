using System.Collections.Generic;

namespace BoardGameTestTaskApp
{
    public class Models
    {
        /// <summary>
        /// x, y == 0,0 top left most corner
        /// </summary>
        public class Tile
        {
            public byte XCoordinate { get; set; }
            public byte YCoordinate { get; set; }
            public byte Color { get; set; }
            internal bool Controlled { get; set; }

            public Tile(byte xCoordinate = 0, byte yCoordinate = 0, byte color = 0, bool controlled = false)
            {
                XCoordinate = xCoordinate;
                YCoordinate = yCoordinate;
                Color = color;
                Controlled = controlled;
            }
        }

        public class ColorSpot
        {
            public byte Color { get; set; }
            public List<Tile> SpotTiles { get; set; }
            public bool IsControlled { get; set; }
            public List<ColorWeight> ColorsWeights { get; set; } = new List<ColorWeight>();
        }

        public class ColorWeight
        {
            public byte Color { get; set; }
            public int TileNumber { get; set; }
            public List<int> NeighboursSpotsIndexes { get; set; } = new List<int>();
        }

        public class Move
        {
            public byte? NewColor { get; set; }
            public List<Tile> Tiles { get; set; }

            public Move(List<Tile> tiles, byte? newColor = null)
            {
                NewColor = newColor;
                Tiles = tiles;
            }
        }
    }
}
