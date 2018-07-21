using System;
using System.Linq;
using System.Text;
using Assets.Rooms;
using AssetsTests.Fakes;
using Utils;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests
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
        //[InlineData(7)]
        public void RoomBlocks_ReduceLayout_ShouldHaveMinimumBlocks(int testNumber)
        {
            var roomBlocks = new RoomBlocks(GetBlockLayout(testNumber));

            var before = roomBlocks.ToString();
            _output.WriteLine("Before");
            _output.WriteLine(before);
            _output.WriteLine('='.ToPaddedString(10));

            roomBlocks.ReduceLayout();

            var actual = roomBlocks.ToString();

            var expected = GetReducedLayout(testNumber);

            _output.WriteLine("Expected");
            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine("Actual");
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        private bool[,] TestMatrix(int testNum)
        {
            switch (testNum)
            {
                case 1:
                    return new[,]
                    {
                        {true,  true,  true },
                        {false, false, false},
                        {true,  false, true }
                    };
                case 2:
                    return new[,]
                    {
                        {true, false, true },
                        {true, false, false},
                        {true, false, true }
                    };

            }

            throw new ArgumentException($"Unknown test number [{testNum}] in TestMatrix");
        }

        [Fact]
        public void SliceRow_ShouldReturnCorrectRows()
        {
            var test = TestMatrix(1);

            var sb = new StringBuilder();
            var row = test.SliceRow(0).ToList();
            foreach (var b in row)
            {
                sb.Append(b.ToString());
                sb.Append(",");
            }
            sb.AppendLine();

            row = test.SliceRow(1).ToList();
            foreach (var b in row)
            {
                sb.Append(b.ToString());
                sb.Append(",");
            }
            sb.AppendLine();

            row = test.SliceRow(2).ToList();
            foreach (var b in row)
            {
                sb.Append(b.ToString());
                sb.Append(",");
            }
            sb.AppendLine();

            var actual = sb.ToString();

            var expected = "True,True,True," + Environment.NewLine +
                           "False,False,False," + Environment.NewLine +
                           "True,False,True," + Environment.NewLine;

            _output.WriteLine(expected);
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SliceColumn_ShouldReturnCorrectColumns()
        {
            var test = TestMatrix(2);

            var sb = new StringBuilder();
            var col = test.SliceColumn(0).ToList();
            foreach (var b in col)
            {
                sb.Append(b.ToString());
                sb.Append(",");
            }
            sb.AppendLine();

            col = test.SliceColumn(1).ToList();
            foreach (var b in col)
            {
                sb.Append(b.ToString());
                sb.Append(",");
            }
            sb.AppendLine();

            col = test.SliceColumn(2).ToList();
            foreach (var b in col)
            {
                sb.Append(b.ToString());
                sb.Append(",");
            }
            sb.AppendLine();

            var actual = sb.ToString();

            var expected = "True,True,True," + Environment.NewLine +
                           "False,False,False," + Environment.NewLine +
                           "True,False,True," + Environment.NewLine;

            _output.WriteLine(expected);
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
