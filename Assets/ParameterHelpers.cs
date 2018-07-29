﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Actors;
using Assets.Messaging;
using Utils;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets
{
    public static class ParameterHelpers
    {
        public static ExtractedParameters ToParameters(this string values)
        {
            var parameters = new List<(string name, string parameter)>();

            var matches = values.Split(':', ',', ' ')
                .Where(value => !value.IsNullOrEmptyOrWhiteSpace())
                .ToList();

            for (var i = 0; i < matches.Count; i += 2)
            {
                parameters.Add((matches[i], matches[i + 1]));
            }

            return parameters;
        }

        public static string Value(this ExtractedParameters parameters, string name)
        {
            return parameters.Single(param => param.name == name).value;
        }

        public static T GetParameter<T>(this ExtractedParameters parameters, string name, ActorRegistry registry) where T : class 
        {
            var value = parameters.Value(name);

            if (typeof(IActor).IsAssignableFrom(typeof(T)))
            {
                return (T)registry.GetActor(value);
            }

            switch (typeof(T).Name)
            {
                case "Int32": return (T)Convert.ChangeType(int.Parse(value), typeof(T));
                case "Double": return (T)Convert.ChangeType(double.Parse(value), typeof(T));
            }

            throw new ArgumentException($"Unable to convert [{name}:{value}] to type [{typeof(T).Name}]");
        }
    }
}
