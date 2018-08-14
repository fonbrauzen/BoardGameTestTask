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
            public List<ColorWeight> ColorsWeights { get; set; }
        }

        public class ColorWeight
        {
            public byte Color { get; set; }
            public int TileNumber { get; set; }
            public List<int> NeighboursSpotsIndexes { get; set; }
        }

        //public class TileComparer : IEqualityComparer<Tile>
        //{

        //    public bool Equals(Tile x, Tile y)
        //    {
        //        //Check whether the objects are the same object. 
        //        if (ReferenceEquals(x, y)) return true;

        //        //Check whether the products' properties are equal. 
        //        return x != null && y != null && x.XCoordinate.Equals(y.XCoordinate) && x.YCoordinate.Equals(y.YCoordinate) && x.Color.Equals(y.Color);
        //    }

        //    public int GetHashCode(Tile obj)
        //    {
        //        int hashCode = obj.XCoordinate.GetHashCode() + obj.YCoordinate.GetHashCode() + obj.Color.GetHashCode();

        //        //Calculate the hash code for the product. 
        //        return hashCode;
        //    }
        //}
    }
}
