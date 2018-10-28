using System;
using System.Linq;
using System.Text;
using Utils;
using Xunit;
using Xunit.Abstractions;

namespace UtilsTests
{
    public class ArrayHelpersTests
    {
        private readonly ITestOutputHelper _output;

        public ArrayHelpersTests(ITestOutputHelper output)
        {
            _output = output;
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
        public void OneDimiensionBounds_AreExpected()
        {
            var array = new string[1,10];

            var row = array.RowUpperBound();
            var column = array.ColumnUpperBound();

            var expected = $"(0,9)";
            var actual   = $"({row},{column})";

            Assert.Equal(expected, actual);
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
