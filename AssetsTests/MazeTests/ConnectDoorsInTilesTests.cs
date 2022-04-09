using System;
using Assets.Deeds;
using Assets.Messaging;
using Assets.Tiles;
using AssetsTests.MazeTests.Helpers;
using Utils;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.MazeTests
{
    public class ConnectDoorsInTilesTests
    {
        private readonly ITestOutputHelper _output;

        public ConnectDoorsInTilesTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private void ConnectDoorTestsImpl(ITwoDoorConnectingTests testDefinition)
        {
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dieBuilder = new DieBuilder();

            var dispatchees = dispatchRegistry.Register(actionRegistry, testDefinition.StartingMaze);
            var tilesRegistry = dispatchees.ExtractTilesRegistry();
            var tiles = new Tiles(tilesRegistry, dispatchRegistry, actionRegistry, dieBuilder);

            _output.OutputMazes(testDefinition.StartingMaze);

            tiles = tiles.ConnectDoors(dispatchRegistry, actionRegistry, dieBuilder);
            var actual = tiles.ToString();
            
            _output.OutputMazes(testDefinition.ExpectedMaze, actual);

            Assert.Equal(testDefinition.ExpectedMaze, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ConnectDoorsWithOneLine(int testNumber)
        {
            var testDefinition = OneConnectingLineTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(testDefinition);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ConnectDoorsWithTwoLines(int testNumber)
        {
            var testDefinition = TwoConnectingLinesTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(testDefinition);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ConnectDoorsWithThreeLines(int testNumber)
        {
            var testDefinition = ThreeConnectingLinesTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(testDefinition);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ConnectDoorsWithFourLines(int testNumber)
        {
            var testDefinition = FourConnectingLinesTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(testDefinition);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ConnectDoorsWithFiveLines(int testNumber)
        {
            var testDefinition = FiveConnectingLinesTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(testDefinition);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void ConnectMultipleDoors(int testNumber)
        {
            var testDefinition = MultipleConnectingDoorsTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(testDefinition);
        }
    }
}
