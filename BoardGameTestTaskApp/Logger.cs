using System;
using System.Collections.Generic;
using System.Linq;
using static BoardGameTestTaskApp.Models;

namespace BoardGameTestTaskApp
{
    public static class Logger
    {
        private static Array ConsoleColors { get; set; } = Enum.GetValues(typeof(ConsoleColor));

        public static void BoardifyAndLog(List<ColorSpot> colorSpots, byte? newColor)
        {
            List<Tile> newBoard = new List<Tile>();
            colorSpots.ForEach(x => newBoard.AddRange(x.SpotTiles));
            newBoard = newBoard.Distinct().OrderBy(x => x.YCoordinate).ThenBy(x => x.XCoordinate).ToList();
            Log(newBoard, newColor);
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
            if (Program.Moves.Count == 0)
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            }
            Console.ForegroundColor = ConsoleColor.White;
            if (Program.Moves.Count == 1)
            {
                Console.WriteLine(string.Format("START SPOT FIRST TILE: X {0}, Y: {1};", Program.StartSpot.SpotTiles[0].XCoordinate, Program.StartSpot.SpotTiles[0].YCoordinate));
                Console.WriteLine("Coordinates started from 0.0 and left top corner");
            }
            Console.WriteLine(string.Format("MOVE NUMBER: {0},  NEW COLOR: {1}", Program.Moves.Count, result.NewColor));
            TilesConsoleLog(board);
        }

        private static void TilesConsoleLog(List<Tile> board)
        {
            for (int i = Program.BoardSize; i <= board.Count; i+=Program.BoardSize)
            {
                int a = 0;
                if (i > Program.BoardSize)
                {
                    a = i - Program.BoardSize;
                }
                for (int j = a; j < i; j++)
                {
                    var tile = board[j];
                    var color = (ConsoleColor)ConsoleColors.GetValue(tile.Color + 4); // contrast colors from console colors enumeration, for number of colors > 3 should be other code.
                    Console.ForegroundColor = color;
                    Console.Write("◼");
                    if (j == i - 1)
                    {
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
