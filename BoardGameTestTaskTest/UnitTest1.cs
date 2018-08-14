using BoardGameTestTaskApp;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using static BoardGameTestTaskApp.Models;

namespace BoardGameTestTaskTest
{
    public class ProgramTest
    {
        private readonly Program _program;

        public ProgramTest() => _program = new Program();

        [Theory]
        [InlineData(6,3)]
        public void GetGameBoardTest(byte boardsSize, byte colorNumber)
        {
            var board = Board.Create(boardsSize, colorNumber);
            Assert.True(board.Count == boardsSize*boardsSize && board.Where(x => x.Color == colorNumber - 1).ToList().Count > 0);
        }

        [Theory]
        [InlineData(2,5,0,1)]
        public void SpotNeigbourTest(byte color, byte XCoordinate, byte YCoordinateA, byte YCoordinateB)
        {
            Tile a = new Tile(XCoordinate, YCoordinateA, color);
            Tile b = new Tile(XCoordinate, YCoordinateB, color);
            List<Tile> neighbours = new List<Tile>() { a, b };
            var result =  ColorSpots.GetNeighbourTiles(neighbours, a);
            Assert.True(result.Count == 1);
        }

        [Theory]
        [InlineData(2, 5, 0, 1)]
        public void SpotNCalculationTest(byte color, byte XCoordinate, byte YCoordinateA, byte YCoordinateB)
        {
            Tile a = new Tile(XCoordinate, YCoordinateA, color);
            Tile b = new Tile(XCoordinate, YCoordinateB, color);
            List<Tile> neighbours = new List<Tile>() { a, b };
            var grouped = neighbours.GroupBy(x => x.Color).ToList();
            var result = ColorSpots.Calculate(grouped);
            Assert.True(result[0].Count() == 1);
        }
    }
}
