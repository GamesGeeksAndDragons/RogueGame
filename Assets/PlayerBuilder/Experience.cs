using System.Runtime.InteropServices;
using Utils;

namespace Assets.PlayerBuilder;

// https://beej.us/moria/mmspoilers/character.html#races

public interface IExperience
{
    public double PointsToAdvanceForLevel(int level, int racePenalty, int classPenalty);
}

public class Experience : IExperience
{
    private readonly Dictionary<int, double> _baseAdvancementExperience = new Dictionary<int, double>
    {
        {  1, 10 },
        {  2, 25 },
        {  3, 45 },
        {  4, 70 },
        {  5, 100 },
        {  6, 140 },
        {  7, 200 },
        {  8, 280 },
        {  9, 300 },
        { 10, 500 },
        { 11, 650 },
        { 12, 850 },
        { 13, 1_100 },
        { 14, 1_400 },
        { 15, 1_800 },
        { 16, 2_300 },
        { 17, 2_900 },
        { 18, 3_600 },
        { 19, 4_400 },
        { 20, 5_400 },
        { 21, 6_800 },
        { 22, 8_400 },
        { 23, 10_200 },
        { 24, 12_500 },
        { 25, 17_500 },
        { 26, 25_000 },
        { 27, 35_000 },
        { 28, 50_000 },
        { 29, 75_000 },
        { 30, 100_000 },
        { 31, 150_000 },
        { 32, 200_000 },
        { 33, 300_000 },
        { 34, 400_000 },
        { 35, 500_000 },
        { 36, 750_000 },
        { 37, 1_500_000 },
        { 38, 2_500_000 },
        { 39, 5_000_000 },
    };

    public double PointsToAdvanceForLevel(int level, int racePenalty, int classPenalty)
    {
        racePenalty.ThrowIfBelow(0, nameof(racePenalty));
        racePenalty.ThrowIfAbove(1, nameof(racePenalty));
        classPenalty.ThrowIfBelow(0, nameof(classPenalty));
        classPenalty.ThrowIfAbove(1, nameof(classPenalty));
        
        racePenalty += 1;
        classPenalty += 1;

        level = Math.Min(1, level);
        level = Math.Max(level, 39);

        var baseLevel =  _baseAdvancementExperience[level];

        return baseLevel * racePenalty * classPenalty;
    }
}