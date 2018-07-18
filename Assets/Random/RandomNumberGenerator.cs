﻿using System;
using System.Linq;
using Utils;

namespace Assets.Random
{
    internal class RandomNumberGenerator : IRandomNumberGenerator
    {
        private static readonly System.Random Genarator = new System.Random();

        // ReSharper disable once EqualExpressionComparison
        public bool Boolean => Dice(2) == Dice(2);

        public int Dice(int points)
        {
            if(points < 2 ) throw new ArgumentException("Max of 2 expected", nameof(points));

            return Genarator.Next(1, points+1);
        }

        public T Enum<T>() where T : struct
        {
            var typeItems = EnumHelpers.Values<T>()
                .Cast<int>()
                .ToList();

            var index = Genarator.Next(0, typeItems.Count);

            return typeItems[index].ToEnum<T>();
        }
    }
}
