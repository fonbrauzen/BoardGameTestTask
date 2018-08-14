using System;
using System.Collections.Generic;
using System.Linq;
using static BoardGameTestTaskApp.Models;

namespace BoardGameTestTaskApp
{
    public class Board
    {
        public static List<Tile> Create(byte boardSize, byte colorsNumber)
        {
            List<Tile> board = new List<Tile>();
            int tilesNumber = boardSize * boardSize;
            Random random = new Random();
            for (byte i = 0; i < boardSize; i++)
            {
                for (byte j = 0; j < boardSize; j++)
                {
                    board.Add(new Tile(j, i, Convert.ToByte(random.Next(0, colorsNumber))));
                }
            }
            return board;
        }

        public static List<IGrouping<byte, Tile>> GroupByColor(List<Tile> board)
        {
            return board.GroupBy(x => x.Color).ToList();
        }
    }
}
