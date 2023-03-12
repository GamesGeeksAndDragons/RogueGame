#nullable enable
using Utils;
using Modifiers = System.Collections.Generic.List<(int Strength, int Exceptional, int HitBonus, int DamageBonus)>;

namespace Assets.Player;

// https://beej.us/moria/mmspoilers/character.html#races

public interface IPlayerStrength
{
    (int Strength, int Exceptional) StrengthCurrent { get; }
    (int Strength, int Exceptional) StrengthBase { get; }
    (int Hit, int Damage) StrengthBonuses { get; }
    void ReduceStrength(int value);
    Parameters CurrentState { get; }
}

class PlayerStrength : IPlayerStrength
{
    private static readonly Modifiers StrengthModifiers = new()
    {
        (1,   0, -3, -2),
        (2,   0, -3, -2),
        (3,   0, -3, -2),
        (4,   0, -2, -1),
        (5,   0, -1, 0),
        (6,   0, -1, 0),
        (7,   0, -1, 0),
        (8,   0,  0, 0),
        (9,   0,  0, 0),
        (10,  0,  0, 0),
        (11,  0,  0, 0),
        (12,  0,  0, 0),
        (13,  0,  0, 0),
        (14,  0,  0, 0),
        (15,  0,  0, 0),
        (16,  0,  0, 1),
        (17,  0,  0, 2),
        (18, 74,  1, 3),
        (18, 90,  2, 4),
        (18, 99,  3, 5),
        (18,100,  6, 6),
    };

    public PlayerStrength(int strength, int hitBonus, int damageBonus)
    {
        BaseStrength = CurrentStrength = strength;
        BaseExceptional = CurrentExceptional = 0;
        HitBonus = hitBonus;
        DamageBonus = damageBonus;
    }

    public PlayerStrength(int baseStrength, int currentStrength, int baseExceptional, int currentExceptional,
        int hitBonus, int damageBonus)
    {
        BaseStrength = baseStrength;
        CurrentStrength = currentStrength;
        BaseExceptional = baseExceptional;
        CurrentExceptional = currentExceptional;
        HitBonus = hitBonus;
        DamageBonus = damageBonus;
    }

    public PlayerStrength(Parameters parameters)
    {
        BaseStrength = parameters.GetStrengthBase();
        CurrentStrength = parameters.GetStrengthCurrent();
        BaseExceptional = parameters.GetStrengthBaseExceptional();
        CurrentExceptional = parameters.GetStrengthCurrentExceptional();
        HitBonus = parameters.GetStrengthHitBonus();
        DamageBonus = parameters.GetStrengthDamageBonus();
    }

    public (int Strength, int Exceptional) StrengthCurrent => (CurrentStrength, CurrentExceptional);
    public (int Strength, int Exceptional) StrengthBase => (BaseStrength, BaseExceptional);
    public (int Hit, int Damage) StrengthBonuses
    {
        get
        {
            var (hit, damage) = GetStrengthRelatedBonuses();
            return (HitBonus + hit, DamageBonus + damage);

            (int hit, int damage) GetStrengthRelatedBonuses()
            {
                var strengthRelatedModifiers = StrengthModifiers
                    .Where(modifier => CurrentStrength == modifier.Strength)
                    .OrderByDescending(modifier => modifier.Exceptional)
                    .ToList();

                var modifier = FilterExceptional(strengthRelatedModifiers);

                return (modifier.HitBonus, modifier.DamageBonus);
            }

            (int Strength, int Exceptional, int HitBonus, int DamageBonus) FilterExceptional(Modifiers modifiers)
            {
                bool UseExceptionalModifiers() => modifiers.Count != 1;

                if (UseExceptionalModifiers())
                {
                    return modifiers.First(modifier => CurrentExceptional <= modifier.Exceptional);
                }

                return modifiers.Single();
            }
        }
    }

    public void ReduceStrength(int value)
    {
        CurrentStrength -= value;
    }

    private int CurrentStrength { get; set; }
    private int BaseStrength { get; init; }

    private int CurrentExceptional { get; set; }
    private int BaseExceptional { get; init; }

    private int HitBonus { get; init; }
    private int DamageBonus { get; init; }

    public Parameters CurrentState => new Parameters()
        .AddStrengthCurrent(CurrentStrength)
        .AddStrengthBase(BaseStrength)
        .AddStrengthCurrentExceptional(CurrentExceptional)
        .AddStrengthBaseExceptional(BaseExceptional)
        .AddStrengthHitBonus(HitBonus)
        .AddStrengthDamageBonus(DamageBonus);
}