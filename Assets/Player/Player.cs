using Assets.Characters;
using Assets.Deeds;
using Utils;
using Utils.Dispatching;

// https://github.com/dungeons-of-moria/umoria/blob/master/src/player.cpp

namespace Assets.Player;

interface IPlayer : IPlayerStrength, IPlayerDexterity
{

}

internal class Player : Character<Player>, IPlayer
{
    //(int Intelligence, int ManaMultiplier, int FailAdjust, int DeviceUse, int SpellsPerLevel) Intelligence { get; }
    //(int Wisdom, int ManaMultiplier, int FailAdjust, int DeviceUse, int SpellsPerLevel) Wisdom { get; }
    //(int Constitution, int HPBonus, int HPPerTurnPoisonDamage) Constitution { get; }
    //(int Charisma, int Base) Charisma { get; }

    //int NumAttacksPerRound { get; }

    private IPlayerStrength _playerStrength = null!;
    private IPlayerDexterity _playerDexterity = null!;

    internal class PlayerState
    {
        public (int Strength, int Exceptional) StrengthCurrent { get; private set; }
        public (int Strength, int Exceptional) StrengthBase { get; private set; }
        public (int Hit, int Damage) StrengthBonuses { get; private set; }
        public (int Dexterity, int Exceptional) DexterityCurrent { get; private set; }
        public (int Dexterity, int Exceptional) DexterityBase { get; private set; }
        public (int Hit, int Armour, int Disarm) DexterityBonuses { get; private set; }
    }

    public Player(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
    : base(dispatchRegistry, actionRegistry, actor, state)
    {
    }

    public override void UpdateState(Parameters state)
    {
        _playerStrength = new PlayerStrength(state);
        _playerDexterity = new PlayerDexterity(state);

        base.UpdateState(state);
    }

    public (int Strength, int Exceptional) StrengthCurrent => _playerStrength.StrengthCurrent;
    public (int Strength, int Exceptional) StrengthBase => _playerStrength.StrengthBase;
    public (int Hit, int Damage) StrengthBonuses => _playerStrength.StrengthBonuses;
    public void ReduceStrength(int value) => _playerStrength.ReduceStrength(value);

    public (int Dexterity, int Exceptional) DexterityCurrent => _playerDexterity.DexterityCurrent;
    public (int Dexterity, int Exceptional) DexterityBase => _playerDexterity.DexterityBase;
    public (int Hit, int Armour, int Disarm) DexterityBonuses => _playerDexterity.DexterityBonuses;
    public void ReduceDexterity(int value) => _playerDexterity.ReduceDexterity(value);

    public override Parameters CurrentState
    {
        get
        {
            var state = base.CurrentState;

            state.AddRange(_playerStrength.CurrentState);
            state.AddRange(_playerDexterity.CurrentState);

            return state;
        }
    }
}
