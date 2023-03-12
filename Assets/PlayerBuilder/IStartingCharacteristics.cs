namespace Assets.PlayerBuilder;

internal interface IStartingCharacteristics
{
    (int Start, string Die) Age { get; }
    (int Start, string Die) MaleHeight { get; }
    (int Start, string Die) MaleWeight { get; }
    (int Start, string Die) FemaleHeight { get; }
    (int Start, string Die) FemaleWeight { get; }
}