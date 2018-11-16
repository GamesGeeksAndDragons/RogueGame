using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Messaging;
using Utils.Coordinates;
using Utils.Random;
using TilesChange=System.Collections.Generic.List<(string Name, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    internal static class TilesBuilder
    {
        public static Tiles BuildTiles(string inputTiles, DispatchRegistry registry, IRandomNumberGenerator randomNumbers)
        {
            var tiles = BuildTilesRegistry(inputTiles, registry, randomNumbers);

            return tiles;
        }

        private static Tiles BuildTilesRegistry(string inputTiles, DispatchRegistry registry,
            IRandomNumberGenerator randomNumbers)
        {
            string[] ExtractLines()
            {
                var newLineChars = Environment.NewLine.ToCharArray();
                return inputTiles.Split(newLineChars);
            }

            IEnumerable<string> RemoveEmptyStrings(string[] lines)
            {
                return lines.Where(line => !string.IsNullOrWhiteSpace(line));
            }

            (int MaxRows, int MaxColumns) GetUpperBounds(List<string> lines)
            {
                int maxRow = lines.Count;
                var maxColumns = lines.Max(line => line.Length);
                return (maxRow, maxColumns);
            }

            var linesWithBlanks = ExtractLines();
            var linesToProcess = RemoveEmptyStrings(linesWithBlanks).ToList();

            var stateChange = StateChangeForInput(registry, linesToProcess);
            var (rows, columns) = GetUpperBounds(linesToProcess);

            var tiles = new Tiles(rows, columns, registry, randomNumbers);
            var tilesState = stateChange.ToTilesState();
            tiles = tiles.Clone(tilesState);

            return tiles;
        }

        private static TilesChange StateChangeForInput(DispatchRegistry registry, IList<string> lines)
        {
            var tilesChanged = new TilesChange();

            var row = 0;
            foreach (var line in lines)
            {
                var column = 0;
                foreach (var actorChar in line)
                {
                    var coordinates = new Coordinate(row, column);
                    var actor = ActorBuilder.Build(actorChar, coordinates, registry);

                    tilesChanged.Add((actor?.UniqueId, coordinates));

                    column++;
                }

                row++;
            }

            return tilesChanged;
        }
    }
}