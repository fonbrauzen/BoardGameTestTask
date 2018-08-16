using System.Collections.Generic;
using System.Linq;
using static BoardGameTestTaskApp.Models;

namespace BoardGameTestTaskApp
{
    public static class Logger
    {
        public static void BoardifyAndLog(List<ColorSpot> colorSpots)
        {
            List<Tile> newBoard = new List<Tile>();
            colorSpots.ForEach(x => newBoard.AddRange(x.SpotTiles));
            newBoard = newBoard.Distinct().ToList();
            Log(newBoard);
        }

        public static Move Log(List<Tile> board, byte? newColor = null)
        {
            Move result = new Move(board, newColor);
            WriteToConsole(board, result);
            Program.Moves.Add(result);
            return result;
        }

        private static void WriteToConsole(List<Tile> board, Move result)
        {
            string tiles = string.Empty;
            tiles = TilesStringify(board, tiles);
            System.Console.Write(string.Format("MOVE NUMBER: {0},  NEW COLOR: {1}, TILES: {2}", Program.Moves.Count, result.NewColor, tiles));
        }

        private static string TilesStringify(List<Tile> board, string tiles)
        {
            foreach (Tile tile in board)
            {
                tiles += string.Format("color: {0}, x: {1}, y: {2}; ", tile.Color, tile.XCoordinate, tile.YCoordinate);
            }
            return tiles;
        }
    }
}
