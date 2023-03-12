#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Display;
using Utils.Random;

// https://beej.us/moria/weapnarm.txt

namespace Assets.Props
{
    //TODO: Change to have a Weapon base class
    internal class MagicalMeleeWeapon : MeleeWeapon
    {
        public MagicalMeleeWeapon(IDieBuilder randomNumbers, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, string actor)
            : base(randomNumbers, dispatchRegistry, actionRegistry, actor, "", "")
        {
        }
        public MagicalMeleeWeapon(IDieBuilder randomNumbers, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, string actor, string state, string uniqueId)
            : base(randomNumbers, dispatchRegistry, actionRegistry, TilesDisplay.DebugWeapon, state, uniqueId)
        {
        }

        public int AdditionalStrength { get; protected set; }
        public int AdditionalArmourClass { get; protected set; }
        public int SlayAnimalMultiplier { get; protected set; } = 1;
        public int SlayDragonMultiplier { get; protected set; } = 1;
        public int SlayEvilMultiplier { get; protected set; } = 1;
        public int SlayUndeadMultiplier { get; protected set; } = 1;
        public int FlameTongueMultiplier { get; protected set; } = 1;
        public int FrostBrandMultiplier { get; protected set; } = 1;

        public override void UpdateState(Parameters state)
        {
            base.UpdateState(state);
        }

        public override Parameters CurrentState => base.CurrentState;
    }
}
