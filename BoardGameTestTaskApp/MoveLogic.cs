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
            if (Program.Moves.Count == 0)
            {
                int maxSpotId = CalculateBestSpot(colorSpots);
                Program.StartSpotId = maxSpotId;
            }
            ChangeColor(Program.StartSpotId, colorSpots);
            Logger.BoardifyAndLog(colorSpots);
        }

        public static int CalculateBestSpot(List<ColorSpot> colorSpots)
        {
            var maxSpotId = LINQExstensions.MaxIndex(colorSpots, x => BiggestSpotConditions(x));
            return maxSpotId;
        }

        private static int BiggestSpotConditions(ColorSpot x)
        {
            return x.SpotTiles.Count + x.ColorsWeights.Max(y => y.TileNumber);
        }

        public static void ChangeColor(int moveSpotId, List<ColorSpot> colorSpots)
        {
            ColorSpot moveSpot = colorSpots[moveSpotId];
            ColorWeight maxColorWeight = GetMaxColorWeight(moveSpot);
            byte oldColor = moveSpot.Color;
            moveSpot.Color = maxColorWeight.Color;
            foreach (Tile tile in moveSpot.SpotTiles)
            {
                tile.Color = maxColorWeight.Color;
            }
            MergeWithMaxColorWeightNeigbors(colorSpots, moveSpot, maxColorWeight, oldColor);
        }

        private static void MergeWithMaxColorWeightNeigbors(List<ColorSpot> colorSpots, ColorSpot moveSpot, ColorWeight maxColorWeight, byte oldColor)
        {
            List<ColorSpot> maxColorNeighbors = new List<ColorSpot>();
            foreach (int id in maxColorWeight.NeighborsSpotsIds)
            {
                var colorSpot = colorSpots.First(x => x.Id == id);
                maxColorNeighbors.Add(colorSpot);
                colorSpots.Remove(colorSpot);
            }
            foreach (ColorSpot neighbor in maxColorNeighbors)
            {
                MergeColorWeights(moveSpot, oldColor, neighbor);
                moveSpot.SpotTiles.AddRange(neighbor.SpotTiles);
                moveSpot.ColorsWeights.First(x => x.Color == maxColorWeight.Color).NeighborsSpotsIds.Remove(neighbor.Id);
            }
        }

        private static void MergeColorWeights(ColorSpot moveSpot, byte oldColor, ColorSpot neighbor)
        {
            // TODO fix negative TileNumbers, delete ColorWeight without neighbors
            foreach (ColorWeight colorWeight in neighbor.ColorsWeights)
            {
                if (moveSpot.ColorsWeights.Any(x => x.Color == colorWeight.Color))
                {
                    var moveSpotSameColorWeight = moveSpot.ColorsWeights.First(x => x.Color == colorWeight.Color);
                    int tileNumber = colorWeight.TileNumber;
                    if (colorWeight.Color == oldColor)
                    {
                        colorWeight.NeighborsSpotsIds.Remove(moveSpot.Id);
                        tileNumber -= moveSpot.SpotTiles.Count;
                    }
                    moveSpotSameColorWeight.NeighborsSpotsIds = colorWeight.NeighborsSpotsIds;
                    moveSpotSameColorWeight.TileNumber = tileNumber;
                }
                else
                {
                    moveSpot.ColorsWeights.Add(colorWeight);
                }
            }
        }

        private static ColorWeight GetMaxColorWeight(ColorSpot moveSpot)
        {
            int maxColorIndex = LINQExstensions.MaxIndex(moveSpot.ColorsWeights, x => x.TileNumber);
            ColorWeight maxColorWeight = moveSpot.ColorsWeights[maxColorIndex];
            return maxColorWeight;
        }
    }
}
