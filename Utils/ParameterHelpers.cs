﻿#nullable enable
using System.Text;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Utils
{
    /*
     * Name Value;
     * Name1 Value1;Name2 Value3;
     */
    public static class ParameterHelpers
    {
        private const char NameParameterTerminator = ';';

        public static (string Name, string Value) ToParameter(this string name, string value)
        {
            return (Name: name, Value: value);
        }

        public static string ToParameter(this Coordinate coordinate, string name = StringExtensions.EmptyString)
        {
            return FormatParameter(name, coordinate.ToString());
        }

        public static string ToParameter<T>(this T value, string name)
            where T : struct
        {
            var str = value.ToString().RemoveNullable();
            return FormatParameter(name, str);
        }


        public static string FormatParameter(this string name, string value = StringExtensions.EmptyString)
        {
            name.ThrowIfEmpty(nameof(name));
            if (value.IsNullOrEmpty()) value = StringExtensions.EmptyString;

            return $"{name} {value}{NameParameterTerminator}";
        }

        public static string FormatParameter<T>(this string name, T value) 
            where T : struct
        {
            var str = value.ToString().RemoveNullable();
            return FormatParameter(name, str);
        }

        public static Parameters AppendParameter(this Parameters parameters, string name, string value)
        {
            var appended = ToParameter(name, value);
            parameters.Add(appended);
            return parameters;
        }

        public static Parameters AppendParameter<T>(this Parameters parameters, string name, T value)
            where T : struct
        {
            var str = value.ToString().RemoveNullable();
            return parameters.AppendParameter(name, str);
        }

        public static string AppendParameter(this string parameter, string name, string value)
        {
            var appended = FormatParameter(name, value);
            return parameter + appended;
        }

        public static string AppendParameter<T>(this string parameter, string name, T value)
            where T : struct
        {
            var appended = FormatParameter(name, value);
            return parameter + appended;
        }

        public static Parameters ToParameters(this string values)
        {
            var separatedParameters = values.Split(NameParameterTerminator)
                .Where(value => !value.IsNullOrEmptyOrWhiteSpace());

            var matches = separatedParameters
                .SelectMany(value => value.Split(' '))
                .ToList();

            var parameters = new List<(string name, string parameter)>();
            for (var i = 0; i < matches.Count; i += 2)
            {
                parameters.Add(ToIntParameter(i));
            }

            return parameters;

            (string Name, string Value) ToIntParameter(int i)
            {
                return (matches[i], matches[i + 1]);
            }
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

        public static string ToString(this Parameters parameters, string name)
        {
            return parameters.Single(param => param.Name == name).Value;
        }

        public static IDispatched GetDispatched(this Parameters parameters, string name, IDispatchRegistry registry)
        {
            var value = parameters.ToString(name);

            return registry.GetDispatched(value);
        }

        public static bool HasValue(this Parameters parameters, string name)
        {
            var value = parameters.SingleOrDefault(param => param.Name == name);
            return value != default;
        }

        public static T ToValue<T>(this Parameters parameters, string name) where T : struct
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

        public static string ToState(this string parameter, params string[] parameters)
        {
            var state = new StringBuilder(parameter);

            foreach (var param in parameters)
            {
                state.Append(param);
            }

            return state.ToString();
        }
    }
}
;