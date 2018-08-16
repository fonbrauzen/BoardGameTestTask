using System.Collections.Generic;
using static BoardGameTestTaskApp.Models;

namespace BoardGameTestTaskApp
{
    public class Program
    {
        static byte ColorsNumber = 3;
        static byte BoardSize = 6;
        internal static List<Move> Moves { get; set; }

        static void Main()
        {
            List<Tile> board = Board.Create(BoardSize, ColorsNumber);
            Logger.Log(board);
            List<ColorSpot> colorSpots = ColorSpots.Spotify(board);
            while (colorSpots.Count > 1)
            {
                MoveLogic.MakeBestMove(colorSpots);
            }
        }
    }
}
