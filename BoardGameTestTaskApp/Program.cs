using System.Collections.Generic;
using System.Linq;
using static BoardGameTestTaskApp.Models;

namespace BoardGameTestTaskApp
{
    public class Program
    {
        static byte ColorsNumber = 3;
        static byte BoardSize = 6;

        static void Main()
        {
            List<Tile> board = Board.Create(BoardSize, ColorsNumber);
            List<IGrouping<byte, Tile>> colorGroups = Board.GroupByColor(board);
            List<IGrouping<byte, Tile>> colorSpotsGroups = ColorSpots.Calculate(colorGroups);
            List<ColorSpot> colorSpots = ColorSpots.Spotify(colorSpotsGroups);
            MoveLogic.MakeBestMove(colorSpots);
        }
    }
}
