﻿using AssetsTests.Helpers;
using AssetsTests.MazeTests.Helpers;

namespace AssetsTests.MazeTests
{
    public class ConnectDoorsInMazeTests : MazeTestHelper
    {
        public ConnectDoorsInMazeTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override void TestAct()
        {
            var changes = GameLevel.Maze.GetTunnelToConnectDoors(DispatchRegistry, ActionRegistry, DieBuilder);
            GameLevel.Maze.ConnectDoorsWithCorridors(changes, DispatchRegistry, ResourceBuilder);
        }

        private void ConnectDoorTestsImpl(IMazeExpectations expectations)
        {
            TestArrange(expectations);
            TestAct();
            TestAssert(expectations);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ConnectDoorsWithOneLine(int testNumber)
        {
            var expectations = OneConnectingLineTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(expectations);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ConnectDoorsWithTwoLines(int testNumber)
        {
            var expectations = TwoConnectingLinesTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(expectations);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ConnectDoorsWithThreeLines(int testNumber)
        {
            var expectations = ThreeConnectingLinesTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(expectations);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ConnectDoorsWithFourLines(int testNumber)
        {
            var expectations = FourConnectingLinesTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(expectations);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ConnectDoorsWithFiveLines(int testNumber)
        {
            var expectations = FiveConnectingLinesTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(expectations);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void ConnectMultipleDoors(int testNumber)
        {
            var expectations = MultipleConnectingDoorsTestDefinitions.GetExpectations(testNumber);

            ConnectDoorTestsImpl(expectations);
        }
    }
}
