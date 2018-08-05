using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets
{
    public static class ParameterHelpers
    {
        public static ExtractedParameters ToParameters(this string values)
        {
            var parameters = new List<(string name, string parameter)>();

            var matches = values.Split('[', ']')
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
            return parameters.Single(param => param.name == name).value;
        }

        public static IDispatchee GetDispatchee(this ExtractedParameters parameters, string name, DispatchRegistry registry)
        {
            var value = parameters.ToString(name);

            return registry.GetDispatchee(value);
        }

        public static bool HasValue(this ExtractedParameters parameters, string name)
        {
            var value = parameters.SingleOrDefault(param => param.name == name);
            return ! value.name.IsNullOrEmpty() && ! value.value.IsNullOrEmpty();
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
    }
}
