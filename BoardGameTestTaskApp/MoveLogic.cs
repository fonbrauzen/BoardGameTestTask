using BoardGameTestTaskApp.Utilities;
using System.Collections.Generic;
using System.Linq;
using static BoardGameTestTaskApp.Models;

namespace BoardGameTestTaskApp
{
    class MoveLogic
    {
        public static void MakeBestMove(List<ColorSpot> colorSpots)
        {
            int maxSpotIndex = CalculateBiggest(colorSpots);

        }

        public static int CalculateBiggest(List<ColorSpot> colorSpots)
        {
            var maxSpotIndex = LINQExstensions.MaxIndex(colorSpots, x => BiggestSpotConditions(x));// colorSpots.Max(x => BiggestSpotConditions(x));
            // var maxSpotIndex = colorSpots.IndexOf(colorSpots.First(x => BiggestSpotConditions(x) == maxSpot));
            return maxSpotIndex;
        }

        private static int BiggestSpotConditions(ColorSpot x)
        {
            return x.SpotTiles.Count + x.ColorsWeights.Max(y => y.TileNumber);
        }

        //static void CalculateMoveCell(List<Tile> board, List<IGrouping<byte, Tile>> colorSpots, int maxSpotIndex)
        //{
        //    IGrouping<byte, Tile> maxSpot = colorSpots[maxSpotIndex];
        //    var maxSpotTiles = maxSpot.ToList();
        //    int maxSpotLength = maxSpotTiles.Count;




        //}

        //static List<IGrouping<byte, Tile>> GetNeighbourSpots(List<IGrouping<byte, Tile>> colorSpots, List<Tile> maxSpotTiles)
        //{
        //    List<IGrouping<byte, Tile>> neighbourSpots = new List<IGrouping<byte, Tile>>();
        //    foreach (Tile tile in maxSpotTiles)
        //    {
        //        neighbourSpots.AddRange(ColorSpots.GetNeighbourSpots(tile, colorSpots));
        //    }
        //    neighbourSpots = neighbourSpots.Distinct().ToList();
        //    return neighbourSpots;
        //}

    }
}
