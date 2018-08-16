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
            var maxSpotIndex = LINQExstensions.MaxIndex(colorSpots, x => BiggestSpotConditions(x));
            return maxSpotIndex;
        }

        private static int BiggestSpotConditions(ColorSpot x)
        {
            return x.SpotTiles.Count + x.ColorsWeights.Max(y => y.TileNumber);
        }
    }
}
