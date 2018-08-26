using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets
{
    public static class ParameterHelpers
    {
        public const char ParameterStart = '{';
        public const char ParameterEnd = '}';
        private const char TileParameterStart = '[';
        private const char TileParameterEnd = ']';
        private const string EmptyString = "string.Empty";

        public static string FormatParameter(this string name, string value)
        {
            name.ThrowIfEmpty(nameof(name));
            value.ThrowIfEmpty(nameof(value));

            return $"{name} {ParameterStart}{value}{ParameterEnd} ";
        }

        public static string FormatParameter<T>(this string name, T value) where T : struct
        {
            return FormatParameter(name, value.ToString());
        }

        public static ExtractedParameters ToParameters(this string values)
        {
            var parameters = new List<(string name, string parameter)>();

            var matches = values.Split(ParameterStart, ParameterEnd)
                .Where(value => !value.IsNullOrEmptyOrWhiteSpace())
                .Select(value => value.Trim())
                .ToList();

            for (var i = 0; i < matches.Count; i += 2)
            {
                parameters.Add((matches[i], matches[i + 1]));
            }

            return parameters;
        }

        public static (int, int) FromDice(this string dice)
        {
            // 1d10, 1d6, 2d7

            var matches = dice.Split('d')
                .Where(value => !value.IsNullOrEmptyOrWhiteSpace())
                .Select(value => value.Trim())
                .ToList();

            var numRolls = int.Parse(matches[0]);
            var maxDice = int.Parse(matches[1]);

            return (numRolls, maxDice);
        }


        public static (int, int) FromBrackets(this string brackets)
        {
            var matches = brackets.Split('(', ',', ')')
                .Where(value => !value.IsNullOrEmptyOrWhiteSpace())
                .Select(value => value.Trim())
                .ToList();

            var first = int.Parse(matches[0]);
            var second = int.Parse(matches[1]);

            return (first, second);
        }

        public static string ToString(this ExtractedParameters parameters, string name)
        {
            return parameters.Single(param => param.Name == name).Value;
        }

        public static IDispatchee GetDispatchee(this ExtractedParameters parameters, string name, DispatchRegistry registry)
        {
            var value = parameters.ToString(name);

            return registry.GetDispatchee(value);
        }

        public static bool HasValue(this ExtractedParameters parameters, string name)
        {
            var value = parameters.SingleOrDefault(param => param.Name == name);
            return ! value.Name.IsNullOrEmpty() && ! value.Value.IsNullOrEmpty();
        }

        public static T ToValue<T>(this ExtractedParameters parameters, string name) where T : struct
        {
            var value = parameters.ToString(name);

            switch (typeof(T).Name)
            {
                case "Int32": return (T)Convert.ChangeType(int.Parse(value), typeof(T));
                case "Double": return (T)Convert.ChangeType(double.Parse(value), typeof(T));
                case "Compass8Points": return (T)Convert.ChangeType(value.ToEnum<T>(), typeof(T));
                case "Coordinate": return (T)Convert.ChangeType(value.ToCoordinates(), typeof(T));
            }

            throw new ArgumentException($"Unable to convert [{name}:{value}] to type [{typeof(T).Name}]");
        }

        private static string FormatTileParameter(this Coordinate coordinate, string value=null)
        {
            if (value.IsNullOrEmpty())
            {
                value = EmptyString;
            }

            return $"{coordinate} {TileParameterStart}{value}{TileParameterEnd} ";
        }

        private static string FormatTileParameter<T>(this Coordinate coordinate, T value) where T : struct
        {
            return FormatTileParameter(coordinate, value.ToString());
        }

        internal static string ToTilesState(this IList<Coordinate> tiles)
        {
            var sb = new StringBuilder();

            foreach (var coordinates in tiles)
            {
                var tile = FormatTileParameter(coordinates);
                sb.Append(tile);
            }

            return FormatTile(sb.ToString());
        }

        internal static string ToTilesState(this IList<(string Name, Coordinate Coordinates)> tiles)
        {
            var sb = new StringBuilder();

            foreach (var change in tiles)
            {
                var tile = FormatTileParameter(change.Coordinates, change.Name);
                sb.Append(tile);
            }

            return FormatTile(sb.ToString());
        }

        private static string FormatTile(string tiles)
        {
            return FormatParameter("Tiles", tiles);
        }

        internal static string ToTileState(this Coordinate coordinates, string name)
        {
            var state = FormatTileParameter(coordinates, name);

            return FormatTile(state);
        }

        public static ExtractedParameters ToTileParameters(this string values)
        {
            var parameters = new List<(string name, string parameter)>();

            var matches = values.Split(TileParameterStart, TileParameterEnd)
                .Where(value => !value.IsNullOrEmptyOrWhiteSpace())
                .Select(value => value.Trim())
                .ToList();

            for (var i = 0; i < matches.Count; i += 2)
            {
                parameters.Add((matches[i], matches[i + 1]));
            }

            return parameters;
        }

        internal static IList<(string Name, Coordinate Coordinates)> ToTiles(this string tileState)
        {
            var tiles = new List<(string Name, Coordinate Coordinates)>();

            var tilesParameters = tileState.ToTileParameters();

            foreach (var parameter in tilesParameters)
            {
                var name = parameter.Value == EmptyString ? null : parameter.Value;
                var coordinate = parameter.Name.ToCoordinates();
                tiles.Add((name, coordinate));
            }

            return tiles;
        }
    }
}
;