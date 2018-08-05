using System;
using Assets.Mazes;
using Utils;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.MazeTests
{
    public class BlockReducerTests
    {
        private readonly ITestOutputHelper _output;

        public BlockReducerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private string GetReducedLayout(int testNumber)
        {
            switch (testNumber)
            {
                case 1:
                    return
                        " |0" + Environment.NewLine +
                        "---" + Environment.NewLine +
                        "0|*";
                case 2:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|**";
                case 3:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|**" + Environment.NewLine +
                        "1|**";
                case 4:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|**" + Environment.NewLine +
                        "1|*.";
                case 5:
                    return
                        " |012" + Environment.NewLine +
                        "-----" + Environment.NewLine +
                        "0|***" + Environment.NewLine +
                        "1|..*" + Environment.NewLine +
                        "2|.**";
            }

            throw new ArgumentException($"Didn't have Expected Layout for [{testNumber}] blocks");
        }

        private bool[,] GetBlockLayout(int testNumber)
        {
            switch (testNumber)
            {
                case 1:
                    return new [,]
                    {
                        {false, false, false, false},
                        {false, false, true,  false},
                        {false, false, false, false},
                        {false, false, false, false},
                    };
                case 2:
                    return new[,]
                    {
                        {false, false, false, false},
                        {false, true,  true,  false},
                        {false, false, false, false},
                        {false, false, false, false},
                    };
                case 3:
                    return new[,]
                    {
                        {false, false, false, false},
                        {false, true,  true,  false},
                        {false, true,  true, false},
                        {false, false, false, false},
                    };
                case 4:
                    return new[,]
                    {
                        {false, false, false, false},
                        {false, true,  true,  false},
                        {false, true,  false, false},
                        {false, false, false, false},
                    };
                case 5:
                    return new[,]
                    {
                        {false, false, false, false},
                        {false, true,  true,  true },
                        {false, false, false, true },
                        {false, false, true,  true },
                    };
            }

            throw new ArgumentException($"Didn't have Expected Layout for [{testNumber}] blocks");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void MazeBlocks_ReduceLayout_ShouldHaveMinimumBlocks(int testNumber)
        {
            var mazeBlocks = new MazeBlocks(GetBlockLayout(testNumber));

            var before = mazeBlocks.ToString();
            _output.WriteLine("Before");
            _output.WriteLine(before);
            _output.WriteLine('='.ToPaddedString(10));

            mazeBlocks = mazeBlocks.ReduceLayout();

            var actual = mazeBlocks.ToString();

            var expected = GetReducedLayout(testNumber);

            _output.WriteLine("Expected");
            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine("Actual");
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
