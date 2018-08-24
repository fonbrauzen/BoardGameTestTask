using System.Collections.Generic;
using static BoardGameTestTaskApp.Models;

namespace BoardGameTestTaskApp
{
    public class Program
    {
        static byte ColorsNumber = 3;
        internal static byte BoardSize = 6;
        internal static ColorSpot StartSpot;
        internal static List<int> MergedIds { get; set; } = new List<int>();
        internal static List<Move> Moves { get; set; } = new List<Move>();

        static void Main()
        {
            List<Tile> board = Board.Create(BoardSize, ColorsNumber);
            Logger.Log(board);
            List<ColorSpot> colorSpots = ColorSpots.Spotify(board);
            while (colorSpots.Count > 1)
            {
                MoveLogic.MakeBestMove(colorSpots);
            }
            System.Console.ReadLine();
        }
    }
}
