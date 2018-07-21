using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Utils;

namespace AssetsTests
{
    public class ArrayPrinterTests
    {
        private readonly ITestOutputHelper _output;

        public ArrayPrinterTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void PrintCoordinates_A2x4Array_ShouldHaveTheExpectedOutput()
        {
            var test = new[,]
            {
                {-1, 0, 1, 1},
                {-1, 0, 1, 1},
            };

            var expected =
                " |0    1    2    3    " + Environment.NewLine +
                "----------------------" + Environment.NewLine +
                "0|(0,0)(0,1)(0,2)(0,3)" + Environment.NewLine +
                "1|(1,0)(1,1)(1,2)(1,3)"
                ;

            var actual = test.PrintCoordinates();

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(30));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PrintCoordinates_A12x2Array_ShouldHaveTheExpectedOutput()
        {
            var test = new[,]
            {
                {true, true},
                {true, true},
                {true, true},
                {true, true},
                {true, true},
                {true, true},
                {true, true},
                {true, true},
                {true, true},
                {true, true},
                {true, true},
                {true, true},
            };

            var expected =
                "  |0     1     " + Environment.NewLine +
                "---------------" + Environment.NewLine +
                "0 |(0,0) (0,1) " + Environment.NewLine +
                "1 |(1,0) (1,1) " + Environment.NewLine +
                "2 |(2,0) (2,1) " + Environment.NewLine +
                "3 |(3,0) (3,1) " + Environment.NewLine +
                "4 |(4,0) (4,1) " + Environment.NewLine +
                "5 |(5,0) (5,1) " + Environment.NewLine +
                "6 |(6,0) (6,1) " + Environment.NewLine +
                "7 |(7,0) (7,1) " + Environment.NewLine +
                "8 |(8,0) (8,1) " + Environment.NewLine +
                "9 |(9,0) (9,1) " + Environment.NewLine +
                "10|(10,0)(10,1)" + Environment.NewLine +
                "11|(11,0)(11,1)"
                ;

            var actual = test.PrintCoordinates();

            _output.WriteLine(expected);
            _output.WriteLine(new string('=', 80));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PrintCoordinates_A2x12Array_ShouldHaveTheExpectedOutput()
        {
            var test = new[,]
            {
                {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12},
                {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12},
            };

            var expected =
                " |0     1     2     3     4     5     6     7     8     9     10    11    " + Environment.NewLine +
                "--------------------------------------------------------------------------" + Environment.NewLine +
                "0|(0,0) (0,1) (0,2) (0,3) (0,4) (0,5) (0,6) (0,7) (0,8) (0,9) (0,10)(0,11)" + Environment.NewLine +
                "1|(1,0) (1,1) (1,2) (1,3) (1,4) (1,5) (1,6) (1,7) (1,8) (1,9) (1,10)(1,11)";

            var actual = test.PrintCoordinates();

            _output.WriteLine(expected);
            _output.WriteLine(new string('=', 80));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Print3x3_WithADelegate_ShouldHaveTheExpectedOutput()
        {
            var test = new[,]
            {
                {9,  8,  7},
                {6,  5,  4},
                {3,  2,   1},
            };

            var expected =
                " |012" + Environment.NewLine +
                "-----" + Environment.NewLine +
                "0|987" + Environment.NewLine +
                "1|654" + Environment.NewLine +
                "2|321";

            var actual = test.Print();

            _output.WriteLine(expected);
            _output.WriteLine(new string('-', expected.Length));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        static class SimpleConverter
        {
            private static Dictionary<int, string> _lookup = new Dictionary<int, string>
            {
                {1,  "1"},
                {2,  "2"},
                {3,  "3"},
                {4,  "4"},
                {5,  "5"},
                {6,  "6"},
                {7,  "7"},
                {8,  "8"},
                {9,  "9"},
                {10, "A"},
                {11, "B"},
                {12, "C"},
                {13, "D"},
                {14, "E"},
                {15, "F"},
                {16, "G"},
                {17, "H"},
                {18, "I"},
                {19, "J"},
                {20, "K"},
                {21, "L"},
                {22, "M"},
                {23, "N"},
                {24, "O"},
                {25, "P"},
                {26, "Q"},
                {27, "R"},
                {28, "S"},
                {29, "T"},
                {30, "U"},
                {31, "V"},
                {32, "W"},
                {33, "X"},
                {34, "Y"},
                {35, "Z"},
                {36, "a"},
                {37, "b"},
                {38, "c"},
                {39, "d"},
            };

            public static string Lookup(int value)
            {
                return _lookup[value];
            }
        }

        [Fact]
        public void Print16x3_WithLookupDelegate_ShouldHaveTheExpectedOutput()
        {
            var test = new[,]
            {
                {1,  2,   3,  4,  5,  6,  7,  8,  9, 10, 11, 12},
                {13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24},
                {25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36},
            };

            var expected =
                " |012345678901" + Environment.NewLine +
                "--------------" + Environment.NewLine +
                "0|123456789ABC" + Environment.NewLine +
                "1|DEFGHIJKLMNO" + Environment.NewLine +
                "2|PQRSTUVWXYZa";

            var actual = test.Print(SimpleConverter.Lookup);

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(20));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Print16x3_WithADelegate_ShouldHaveTheExpectedOutput()
        {
            // y = 14, x = 3
            var test = new[,]
            {
                {false,  false,  false}, //  0
                {false,  false,  true }, //  1
                {false,  true,   false}, //  2
                {false,  true,   true }, //  3
                {true,   false,  false}, //  4
                {true,   false,  true }, //  5
                {true,   true,   false}, //  6
                {true,   true,   true }, //  7
                {false,  false,  false}, //  8
                {false,  false,  true }, //  9
                {false,  true,   false}, // 10
                {false,  true,   true }, // 11
                {true,   false,  false}, // 12
                {true,   false,  true }, // 13
                {true,   true,   false}, // 14
                {true,   true,   true }, // 15
            };

            var expected =
                "  |012" + Environment.NewLine +
                "------" + Environment.NewLine +
                "0 |..." + Environment.NewLine +
                "1 |..*" + Environment.NewLine +
                "2 |.*." + Environment.NewLine +
                "3 |.**" + Environment.NewLine +
                "4 |*.." + Environment.NewLine +
                "5 |*.*" + Environment.NewLine +
                "6 |**." + Environment.NewLine +
                "7 |***" + Environment.NewLine +
                "8 |..." + Environment.NewLine +
                "9 |..*" + Environment.NewLine +
                "10|.*." + Environment.NewLine +
                "11|.**" + Environment.NewLine +
                "12|*.." + Environment.NewLine +
                "13|*.*" + Environment.NewLine +
                "14|**." + Environment.NewLine +
                "15|***";

            var actual = test.Print(isSet => isSet ? "*" : ".");

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Print3x16_WithADelegate_ShouldHaveTheExpectedOutput()
        {
            var test = new[,]
            {
                {false, false, false, false, true,  true,  true,  true,  false, false, false, false, true,  true,  true,  true},
                {false, true,  false, true,  false, true,  false, true,  false, true,  false, true,  false, true,  false, true},
                {true,  true,  true,  true,  false, false, false, false, true,  true,  true,  true,  false, false, false, false},
            };

            var expected =
                " |0123456789012345" + Environment.NewLine +
                "------------------" + Environment.NewLine +
                "0|....****....****" + Environment.NewLine +
                "1|.*.*.*.*.*.*.*.*" + Environment.NewLine +
                "2|****....****....";

            var actual = test.Print(isSet => isSet ? "*" : ".");

            _output.WriteLine(expected);
            _output.WriteLine("");
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
