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
            // GetBestInitialTile(board, ColorsNumber);
            // MoveLogic.MakeBestMove(colorSpots);
        }

        

        //public static Tile GetBestInitialTile(List<List<int>> board, int colorsNumber)
        //{
        //    Tile result = new Tile();
        //    List<KeyValuePair<int, List<Tile>>> tilesBestResults = new List<KeyValuePair<int, List<Tile>>>();
        //    for (int i = 0; i < board.Count; i++)
        //    {
        //        tilesBestResults.Add(GetMaxTilesColor4Tile(new Tile(0, i, board[i][0]), board, colorsNumber));
        //    }
        //    // TODO logic for best result
        //    return result;
        //}

    //    public static KeyValuePair<int, List<Tile>> GetMaxTilesColor4Tile(Tile currentTile, List<List<int>> board, int colorsNumber)
    //    {
    //        KeyValuePair<int, List<Tile>> result = new KeyValuePair<int, List<Tile>>();
    //        int currentTileRow = currentTile.YCoordinate;
    //        int currentTileColumn = currentTile.XCoordinate;
    //        List<KeyValuePair<int, List<Tile>>> colorsScores = CreateColorsList(colorsNumber);
    //        GetRightTilesColors(currentTile, board, currentTileRow, colorsScores);
    //        GetLeftTilesColors(currentTile, board, currentTileRow, colorsScores);
    //        GetDownTilesColors(currentTile, board, currentTileColumn, colorsScores);
    //        GetUpTilesColors(currentTile, board, currentTileColumn, colorsScores);
    //        result = CountMaxTilesColor(result, colorsScores);
    //        return result;
    //    }

    //    private static KeyValuePair<int, List<Tile>> CountMaxTilesColor(KeyValuePair<int, List<Tile>> result, List<KeyValuePair<int, List<Tile>>> colorsScores)
    //    {
    //        int maxTiles = colorsScores.Max(x => x.Value.Count);
    //        for (int i = 0; i < colorsScores.Count; i++)
    //        {
    //            KeyValuePair<int, List<Tile>> colorScore = colorsScores[i];
    //            if (colorScore.Value.Count == maxTiles)
    //            {
    //                result = colorsScores[i];
    //            }

    //        }

    //        return result;
    //    }

    //    private static List<KeyValuePair<int, List<Tile>>> CreateColorsList(int colorsNumber)
    //    {
    //        List<KeyValuePair<int, List<Tile>>> colorsScores = new List<KeyValuePair<int, List<Tile>>>();
    //        for (int i = 0; i < colorsNumber - 1; i++)
    //        {
    //            colorsScores.Add(new KeyValuePair<int, List<Tile>>(i, new List<Tile>()));
    //        }

    //        return colorsScores;
    //    }

    //    private static void GetLeftTilesColors(Tile currentTile, List<List<int>> board, int currentTileRow, List<KeyValuePair<int, List<Tile>>> result)
    //    {
    //        if (currentTile.XCoordinate > 0)
    //        {
    //            int xColumnLeft = currentTile.XCoordinate - 1;
    //            while (xColumnLeft >= 0)
    //            {
    //                int tileColor = board[currentTileRow][xColumnLeft];
    //                if (xColumnLeft == currentTile.XCoordinate - 1 || tileColor == board[currentTileRow][xColumnLeft + 1])
    //                {
    //                    result[tileColor].Value.Add(new Tile(xColumnLeft, currentTileRow, tileColor));
    //                    xColumnLeft--;
    //                }
    //                else
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //    }

    //    private static void GetRightTilesColors(Tile currentTile, List<List<int>> board, int currentTileRow, List<KeyValuePair<int, List<Tile>>> result)
    //    {
    //        if (currentTile.XCoordinate < board[currentTileRow].Count - 1)
    //        {
    //            int xColumnRight = currentTile.XCoordinate + 1;
    //            while (xColumnRight < board[currentTileRow].Count)
    //            {
    //                int tileColor = board[currentTileRow][xColumnRight];
    //                if (xColumnRight == currentTile.XCoordinate + 1 || tileColor == board[currentTileRow][xColumnRight - 1])
    //                {
    //                    result[tileColor].Value.Add(new Tile(xColumnRight, currentTileRow, tileColor)); ;
    //                    xColumnRight++;
    //                }
    //                else
    //                {
    //                    break;
    //                }
                    
    //            }
    //        }
    //    }

    //    private static void GetDownTilesColors(Tile currentTile, List<List<int>> board, int currentTileColumn, List<KeyValuePair<int, List<Tile>>> result)
    //    {
    //        if (currentTile.YCoordinate < board.Count - 1)
    //        {
    //            int yRowDown = currentTile.YCoordinate + 1;
    //            while (yRowDown < board.Count)
    //            {
    //                int tileColor = board[yRowDown][currentTileColumn];
    //                if (yRowDown == currentTile.YCoordinate + 1 || tileColor == board[yRowDown - 1][currentTileColumn])
    //                {
    //                    result[tileColor].Value.Add(new Tile(currentTileColumn, yRowDown, tileColor));
    //                    yRowDown++;
    //                }
    //                else
    //                {
    //                    break;
    //                }

    //            }
    //        }
    //    }

    //    private static void GetUpTilesColors(Tile currentTile, List<List<int>> board, int currentTileColumn, List<KeyValuePair<int, List<Tile>>> result)
    //    {
    //        if (currentTile.YCoordinate < board.Count - 1)
    //        {
    //            int yRowUp = currentTile.YCoordinate - 1;
    //            while (yRowUp < board.Count)
    //            {
    //                int tileColor = board[yRowUp][currentTileColumn];
    //                if (yRowUp == currentTile.YCoordinate - 1 || tileColor == board[yRowUp + 1][currentTileColumn])
    //                {
    //                    result[tileColor].Value.Add(new Tile(currentTileColumn, yRowUp, tileColor));
    //                    yRowUp++;
    //                }
    //                else
    //                {
    //                    break;
    //                }

    //            }
    //        }
    //    }
    }
}
