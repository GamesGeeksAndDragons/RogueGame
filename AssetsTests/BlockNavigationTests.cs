using System;
using Assets.Rooms;
using AssetsTests.Fakes;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests
{
    public class BlockNavigationTests
    {
        private readonly ITestOutputHelper _output;
        public enum Test
        {
            TopLeft, TopRight, BottomRight, BottomLeft, Cornered
        }

        public BlockNavigationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(Test.TopLeft)]
        [InlineData(Test.TopRight)]
        [InlineData(Test.BottomRight)]
        [InlineData(Test.BottomLeft)]
        [InlineData(Test.Cornered)]
        public void DecideLayout_ShouldHaveConnectedBlocks(Test test)
        {
            var fakeRandomNumbers = TestDataForNavigationTests.GetGenerator(test);
            var builder = new RandomRoomBuilder(fakeRandomNumbers, new FakeLogger(_output));
            var numBlocks = TestDataForNavigationTests.GetNumBlocks(test);
            var blocks = builder.DecideLayout(numBlocks);

            var expected = TestDataForNavigationTests.GetExpected(test);

            var actual = blocks.ToString();

            _output.WriteLine(expected);
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}