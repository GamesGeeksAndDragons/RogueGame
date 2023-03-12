#nullable enable
using Utils;
using Modifiers = System.Collections.Generic.List<(int Strength, int Exceptional, int HitBonus, int AcBpnus, int DisarmBonus)>;

namespace Assets.Player;

public interface IPlayerDexterity
{
    (int Dexterity, int Exceptional) DexterityCurrent { get; }
    (int Dexterity, int Exceptional) DexterityBase { get; }
    (int Hit, int Armour, int Disarm) DexterityBonuses { get; }
    void ReduceDexterity(int value);
    Parameters CurrentState { get; }
}

class PlayerDexterity : IPlayerDexterity
{
    private static readonly Modifiers DexterityModifiers = new()
    {
        (1,   0, -3, -4, -8),
        (2,   0, -3, -4, -8),
        (3,   0, -3, -4, -8),
        (4,   0, -2, -3, -6),
        (5,   0, -2, -2, -4),
        (6,   0, -1, -1, -2),
        (7,   0, -1, 0,  -1),
        (8,   0,  0, 0,  0),
        (9,   0,  0, 0,  0),
        (10,  0,  0, 0,  0),
        (11,  0,  0, 0,  0),
        (12,  0,  0, 0,  0),
        (13,  0,  0, 0,  1),
        (14,  0,  0, 0,  1),
        (15,  0,  0, 0,  1),
        (16,  0,  1, 1,  2),
        (17,  0,  2, 1,  2),
        (18, 49,  3, 2,  4),
        (18, 90,  4, 3,  5),
        (18, 99,  4, 6,  6),
        (18,100,  5, 5,  8),
    };

    public PlayerDexterity(int strength, int hitBonus, int acBonus, int disarmBonus)
    {
        BaseDexterity = CurrentDexterity = strength;
        BaseExceptional = CurrentExceptional = 0;
        HitBonus = hitBonus;
        AcBonus = acBonus;
        DisarmBonus = disarmBonus;
    }

    public PlayerDexterity(int baseDexterity, int currentDexterity, int baseExceptional, int currentExceptional,
        int hitBonus, int acBonus, int disarmBonus)
    {
        BaseDexterity = baseDexterity;
        CurrentDexterity = currentDexterity;
        BaseExceptional = baseExceptional;
        CurrentExceptional = currentExceptional;
        HitBonus = hitBonus;
        AcBonus = acBonus;
        DisarmBonus = disarmBonus;
    }

    public PlayerDexterity(Parameters parameters)
    {
        BaseDexterity = parameters.GetDexterityBase();
        CurrentDexterity = parameters.GetDexterityCurrent();
        BaseExceptional = parameters.GetDexterityBaseExceptional();
        CurrentExceptional = parameters.GetDexterityCurrentExceptional();
        HitBonus = parameters.GetDexterityHitBonus();
        AcBonus = parameters.GetDexterityAcBonus();
        DisarmBonus = parameters.GetDexterityDisarmBonus();
    }

    public (int Dexterity, int Exceptional) DexterityCurrent => (CurrentDexterity, CurrentExceptional);
    public (int Dexterity, int Exceptional) DexterityBase => (BaseDexterity, BaseExceptional);
    public (int Hit, int Armour, int Disarm) DexterityBonuses
    {
        get
        {
            var (hit, ac, disarm) = GetDexterityRelatedBonuses();

            return (HitBonus + hit, AcBonus + ac, DisarmBonus + disarm);

            (int hit, int ac, int disarm) GetDexterityRelatedBonuses()
            {
                var strengthRelatedModifiers = DexterityModifiers
                    .Where(modifier => CurrentDexterity == modifier.Strength)
                    .OrderByDescending(modifier => modifier.Exceptional)
                    .ToList();

                var modifier = FilterExceptional(strengthRelatedModifiers);

                return (modifier.HitBonus, modifier.AcBonus, modifier.DisarmBonus);
            }

            (int Strength, int Exceptional, int HitBonus, int AcBonus, int DisarmBonus) FilterExceptional(Modifiers modifiers)
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

    public void ReduceDexterity(int value)
    {
        CurrentDexterity -= value;
    }

    private int CurrentDexterity { get; set; }
    private int BaseDexterity { get; init; }

    private int CurrentExceptional { get; set; }
    private int BaseExceptional { get; init; }

    private int HitBonus { get; init; }
    private int AcBonus { get; init; }
    private int DisarmBonus { get; init; }


    public Parameters CurrentState => new Parameters()
        .AddDexterityCurrent(CurrentDexterity)
        .AddDexterityBase(BaseDexterity)
        .AddDexterityCurrentExceptional(CurrentExceptional)
        .AddDexterityBaseExceptional(BaseExceptional)
        .AddDexterityHitBonus(HitBonus)
        .AddDexterityAcBonus(AcBonus)
        .AddDexterityDisarmBonus(DisarmBonus)
    ;
}