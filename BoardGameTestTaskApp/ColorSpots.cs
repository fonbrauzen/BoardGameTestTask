using System.Collections.Generic;
using System.Linq;
using static BoardGameTestTaskApp.Models;

namespace BoardGameTestTaskApp
{
    public class ColorSpots
    {
        public static List<ColorSpot> Spotify(List<Tile> board)
        {
            List<IGrouping<byte, Tile>> colorGroups = GroupByColor(board);
            List<IGrouping<byte, Tile>> colorSpotsGroups = Calculate(colorGroups);
            List<ColorSpot> colorSpots = SpotGroupsToSpots(colorSpotsGroups);
            return colorSpots;
        }

        private static List<IGrouping<byte, Tile>> GroupByColor(List<Tile> board)
        {
            return board.GroupBy(x => x.Color).ToList();
        }

        public static List<IGrouping<byte, Tile>> Calculate(List<IGrouping<byte, Tile>> colorGroups)
        {
            List<IGrouping<byte, Tile>> colorSpots = new List<IGrouping<byte, Tile>>();
            foreach (IGrouping<byte, Tile> colorGroup in colorGroups)
            {
                FindSpotsInColorGroup(colorGroup, colorSpots);
            }
            
            return colorSpots;
        }

        private static void FindSpotsInColorGroup(IGrouping<byte, Tile> colorGroup, List<IGrouping<byte, Tile>> colorSpots)
        {
            var oneColorTiles = colorGroup.ToList();
            foreach (Tile item in colorGroup)
            {
                if (colorSpots.FindIndex(x => x.Contains(item)) < 0)
                {
                    List<Tile> colorSpot = new List<Tile>() { item };
                    List<Tile> neibours = GetNeighborTiles(oneColorTiles, item);
                    if (neibours.Count > 0)
                    {
                        Queue<Tile> neighboursIteration = new Queue<Tile>(neibours);
                        while (neighboursIteration.Count > 0)
                        {
                            var neibour = neighboursIteration.Dequeue();
                            if (!colorSpot.Contains(neibour))
                            {
                                colorSpot.Add(neibour);
                            }
                            List<Tile> neighboursNeighbours = GetNeighborTiles(oneColorTiles, neibour);
                            foreach (Tile n in neighboursNeighbours)
                            {
                                if (!colorSpot.Contains(n))
                                {
                                    colorSpot.Add(n);
                                    neighboursIteration.Enqueue(n);
                                }
                            }
                        }
                    }
                    colorSpots.AddRange(colorSpot.GroupBy(x => x.Color).ToList());
                }
            }
        }

        public static List<Tile> GetNeighborTiles(List<Tile> Tiles, Tile tile)
        {
            return Tiles.Where(x => (x.YCoordinate == tile.YCoordinate && (x.XCoordinate == tile.XCoordinate + 1 || x.XCoordinate == tile.XCoordinate - 1)) || 
            (x.XCoordinate == tile.XCoordinate && (x.YCoordinate == tile.YCoordinate + 1 || x.YCoordinate == tile.YCoordinate - 1))).ToList();
        }

        private static List<ColorSpot> SpotGroupsToSpots(List<IGrouping<byte, Tile>> colorSpotsGroups)
        {
            List<ColorSpot> spots = new List<ColorSpot>();
            for (int i = 0; i < colorSpotsGroups.Count; i++)
            {
                IGrouping<byte, Tile> group = colorSpotsGroups[i];
                ColorSpot spot = new ColorSpot
                {
                    Color = group.Key,
                    Id = i,
                    SpotTiles = group.ToList()
                };
                CalculateColorWeights(colorSpotsGroups, i, spot);
                spots.Add(spot);

            }
            return spots;
        }

        private static void CalculateColorWeights(List<IGrouping<byte, Tile>> colorSpotsGroups, int spotIndex, ColorSpot spot)
        {
            List<IGrouping<byte, Tile>> neighbors = new List<IGrouping<byte, Tile>>();
            var otherSpots = colorSpotsGroups.Where((v, j) => j != spotIndex).ToList();
            foreach (Tile tile in spot.SpotTiles)
            {
                List<IGrouping<byte, Tile>> tilNeighbours = GetNeighborSpots(tile, otherSpots);
                neighbors.AddRange(tilNeighbours);
            }
            neighbors = neighbors.Distinct().ToList();
            var neighborsGroups = neighbors.GroupBy(x => x.Key).ToList();
            foreach (var color in neighborsGroups)
            {
                GetColorWeight(colorSpotsGroups, spot, color);
            }
        }

        public static List<IGrouping<byte, Tile>> GetNeighborSpots(Tile tile, List<IGrouping<byte, Tile>> spots)
        {
            return spots.Where(x => GetNeighborTiles(x.ToList(), tile).Count() > 0).ToList();
        }

        private static void GetColorWeight(List<IGrouping<byte, Tile>> colorSpotsGroups, ColorSpot spot, IGrouping<byte, IGrouping<byte, Tile>> color)
        {
            ColorWeight colorWeight = new ColorWeight
            {
                Color = color.Key
            };
            var colorNeighbours = color.ToList();
            foreach (IGrouping<byte, Tile> neighbour in colorNeighbours)
            {
                colorWeight.NeighborsSpotsIds.Add(colorSpotsGroups.IndexOf(neighbour));
                colorWeight.TileNumber += neighbour.Count();
            }
            spot.ColorsWeights.Add(colorWeight);
        }
    }
}
