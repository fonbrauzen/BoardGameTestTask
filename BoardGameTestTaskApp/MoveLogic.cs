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
            if (Program.Moves.Count == 1)
            {
                int maxSpotId = CalculateBestSpot(colorSpots);
                Program.StartSpotId = maxSpotId;
            }
            byte?newColor = ChangeColor(Program.StartSpotId, colorSpots);
            Logger.BoardifyAndLog(colorSpots, newColor);
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

        public static byte? ChangeColor(int moveSpotId, List<ColorSpot> colorSpots)
        {
            byte? newColor;
            ColorSpot moveSpot = colorSpots.First(x => x.Id == moveSpotId);
            ColorWeight maxColorWeight = GetMaxColorWeight(moveSpot);
            newColor = moveSpot.Color = maxColorWeight.Color;
            MergeWithMaxColorWeightNeigbors(colorSpots, moveSpot, maxColorWeight);
            return newColor;
        }

        private static void MergeWithMaxColorWeightNeigbors(List<ColorSpot> colorSpots, ColorSpot moveSpot, ColorWeight maxColorWeight)
        {
            List<ColorSpot> maxColorNeighbors = new List<ColorSpot>();
            Program.MergedIds.AddRange(maxColorWeight.NeighborsSpotsIds);
            foreach (int id in maxColorWeight.NeighborsSpotsIds)
            {
                var colorSpot = colorSpots.First(x => x.Id == id);
                maxColorNeighbors.Add(colorSpot);
                colorSpots.Remove(colorSpot);
            }

            foreach (ColorSpot neighbor in maxColorNeighbors)
            {
                MergeColorWeights(moveSpot, neighbor);
                moveSpot.SpotTiles.AddRange(neighbor.SpotTiles);
                moveSpot.ColorsWeights.First(x => x.Color == maxColorWeight.Color).NeighborsSpotsIds.Remove(neighbor.Id);
            }
            moveSpot.SpotTiles.ForEach(x => x.Color = moveSpot.Color);
            CleanupEmptyColorWeights(moveSpot);
            TilesNumbersRecalculate(colorSpots, moveSpot);

        }

        private static void TilesNumbersRecalculate(List<ColorSpot> colorSpots, ColorSpot moveSpot)
        {
            foreach (ColorWeight weight in moveSpot.ColorsWeights)
            {
                weight.NeighborsSpotsIds = weight.NeighborsSpotsIds.Except(Program.MergedIds).ToList();
                int tilesNumber = 0;
                foreach (int id in weight.NeighborsSpotsIds)
                {
                    tilesNumber += colorSpots.First(x => x.Id == id).SpotTiles.Count;
                }
                weight.TileNumber = tilesNumber;
            }
        }

        private static void MergeColorWeights(ColorSpot moveSpot, ColorSpot neighbor)
        {
            foreach (ColorWeight colorWeight in neighbor.ColorsWeights)
            {
                if (moveSpot.ColorsWeights.Any(x => x.Color == colorWeight.Color))
                {
                    var moveSpotSameColorWeight = moveSpot.ColorsWeights.First(x => x.Color == colorWeight.Color);
                    MergeColorWeightData(colorWeight, moveSpotSameColorWeight, moveSpot.Id);
                }
                else
                {
                    ColorWeight newColorWeight = new ColorWeight()
                    {
                        Color = colorWeight.Color
                    };
                    MergeColorWeightData(colorWeight, newColorWeight, moveSpot.Id);
                    moveSpot.ColorsWeights.Add(colorWeight);
                }
            }
        }

        private static void MergeColorWeightData(ColorWeight colorWeight, ColorWeight moveSpotSameColorWeight, int moveSpotId)
        {
            moveSpotSameColorWeight.NeighborsSpotsIds.AddRange(colorWeight.NeighborsSpotsIds);
            moveSpotSameColorWeight.NeighborsSpotsIds = moveSpotSameColorWeight.NeighborsSpotsIds.Distinct().ToList();
            if (moveSpotSameColorWeight.NeighborsSpotsIds.Contains(moveSpotId))
            {
                moveSpotSameColorWeight.NeighborsSpotsIds.Remove(moveSpotId);
            }
        }

        private static void CleanupEmptyColorWeights(ColorSpot moveSpot)
        {
            List<ColorWeight> emptyColorWeights = moveSpot.ColorsWeights.Where(x => x.NeighborsSpotsIds.Count == 0).ToList();
            foreach (var emptyColorWeight in emptyColorWeights)
            {
                moveSpot.ColorsWeights.Remove(emptyColorWeight);
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
