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
            //TODO fix out of range exception, sometimes moveSpot absent in colorSpots
            ColorSpot moveSpot = colorSpots.First(x => x.Id == moveSpotId);
            if (Program.Moves.Count == 1)
            {
                Program.StartSpotInitialTilesNumber = moveSpot.SpotTiles.Count;
            }
            ColorWeight maxColorWeight = GetMaxColorWeight(moveSpot);
            byte oldColor = moveSpot.Color;
            moveSpot.Color = maxColorWeight.Color;
            newColor = moveSpot.Color = maxColorWeight.Color;
            foreach (Tile tile in moveSpot.SpotTiles)
            {
                tile.Color = maxColorWeight.Color;
            }
            MergeWithMaxColorWeightNeigbors(colorSpots, moveSpot, maxColorWeight, oldColor);
            return newColor;
        }

        private static void MergeWithMaxColorWeightNeigbors(List<ColorSpot> colorSpots, ColorSpot moveSpot, ColorWeight maxColorWeight, byte oldColor)
        {
            List<ColorSpot> maxColorNeighbors = new List<ColorSpot>();

            foreach (int id in maxColorWeight.NeighborsSpotsIds)
            {
                var colorSpot = colorSpots.First(x => x.Id == id);
                maxColorNeighbors.Add(colorSpot);
                Program.MergedIds.Add(new KeyValuePair<int, int>(id, colorSpots.First(x => x.Id == id).SpotTiles.Count));
                colorSpots.Remove(colorSpot);
            }

            foreach (ColorSpot neighbor in maxColorNeighbors)
            {
                MergeColorWeights(moveSpot, oldColor, neighbor, colorSpots);
                moveSpot.SpotTiles.AddRange(neighbor.SpotTiles);
                moveSpot.ColorsWeights.First(x => x.Color == maxColorWeight.Color).NeighborsSpotsIds.Remove(neighbor.Id);
            }
            moveSpot.SpotTiles.ForEach(x => x.Color = moveSpot.Color);
            CleanupColorWeights(moveSpot);
            UpdateColorWeightsDataForAllSpots(colorSpots);

        }

        private static void UpdateColorWeightsDataForAllSpots(List<ColorSpot> colorSpots)
        {
            Program.MergedIds = Program.MergedIds.Distinct().ToList();
            foreach (KeyValuePair<int, int> mergedId in Program.MergedIds)
            {
                List<ColorSpot> spotsForUpdate = colorSpots.Where(x => x.ColorsWeights.Any(y => y.NeighborsSpotsIds.Contains(mergedId.Key) == true)).ToList();
                foreach (ColorSpot spot in spotsForUpdate)
                {
                    foreach (ColorWeight colorWeight in spot.ColorsWeights)
                    {
                        if (colorWeight.NeighborsSpotsIds.Contains(mergedId.Key))
                        {
                            colorWeight.NeighborsSpotsIds.Remove(mergedId.Key);
                            colorWeight.TileNumber -= mergedId.Value;
                        }
                    }
                }
            }
        }

        private static void MergeColorWeights(ColorSpot moveSpot, byte oldColor, ColorSpot neighbor, List<ColorSpot> colorSpots)
        {
            foreach (ColorWeight colorWeight in neighbor.ColorsWeights)
            {
                if (moveSpot.ColorsWeights.Any(x => x.Color == colorWeight.Color))
                {
                    var moveSpotSameColorWeight = moveSpot.ColorsWeights.First(x => x.Color == colorWeight.Color);
                    MergeColorWeightData(moveSpot, oldColor, colorWeight, moveSpotSameColorWeight, colorSpots);
                }
                else
                {
                    ColorWeight newColorWeight = new ColorWeight()
                    {
                        Color = colorWeight.Color
                    };
                    MergeColorWeightData(moveSpot, oldColor, colorWeight, newColorWeight, colorSpots);
                    moveSpot.ColorsWeights.Add(colorWeight);
                }
            }
        }

        private static void MergeColorWeightData(ColorSpot moveSpot, byte oldColor, ColorWeight colorWeight, ColorWeight moveSpotSameColorWeight, List<ColorSpot> colorSpots)
        {
            int tileNumber = colorWeight.TileNumber;
            tileNumber = RemoveMoveSpotDataBeforeMerge(moveSpot, oldColor, colorWeight, tileNumber);
            moveSpotSameColorWeight.NeighborsSpotsIds.AddRange(colorWeight.NeighborsSpotsIds);
            var duplicates = moveSpotSameColorWeight.NeighborsSpotsIds.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => new { id = y.Key, number = y.Count() })
              .ToList();
            foreach (var duplicate in duplicates)
            {
                moveSpotSameColorWeight.NeighborsSpotsIds.RemoveAll(x => x == duplicate.id);
                int length = colorSpots.First(x => x.Id == duplicate.id).SpotTiles.Count;
                tileNumber -= duplicate.number * length;
            }
            moveSpotSameColorWeight.TileNumber += tileNumber;
        }

        private static int RemoveMoveSpotDataBeforeMerge(ColorSpot moveSpot, byte oldColor, ColorWeight colorWeight, int tileNumber)
        {
            if (colorWeight.NeighborsSpotsIds.Contains(moveSpot.Id))
            {
                colorWeight.NeighborsSpotsIds.Remove(moveSpot.Id);
                tileNumber -= Program.StartSpotInitialTilesNumber; //since other spots have outdated data about start spot tiles number after 1st move
            }

            return tileNumber;
        }

        private static void CleanupColorWeights(ColorSpot moveSpot)
        {
            List<ColorWeight> emptyColorWeights = moveSpot.ColorsWeights.Where(x => x.NeighborsSpotsIds.Count == 0).ToList();
            foreach (var emptyColorWeight in emptyColorWeights)
            {
                moveSpot.ColorsWeights.Remove(emptyColorWeight);
            }
            //List<ColorWeight> weightsWithMoveSpot = moveSpot.ColorsWeights.Where(x => x.NeighborsSpotsIds.Contains(moveSpot.Id)).ToList();
            //foreach (var weight in weightsWithMoveSpot)
            //{
            //    weight.NeighborsSpotsIds.Remove(moveSpot.Id);
            //    weight.TileNumber -= Program.StartSpotInitialTilesNumber;
            //}
        }

        private static ColorWeight GetMaxColorWeight(ColorSpot moveSpot)
        {
            int maxColorIndex = LINQExstensions.MaxIndex(moveSpot.ColorsWeights, x => x.TileNumber);
            ColorWeight maxColorWeight = moveSpot.ColorsWeights[maxColorIndex];
            return maxColorWeight;
        }
    }
}
