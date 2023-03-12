using Assets.Deeds;
using Assets.Messaging;
using Utils.Display;
using Utils.Random;

namespace Assets.Props;

// https://beej.us/moria/mmspoilers/items.html#weapons
// https://beej.us/moria/items.txt

internal class MeleeWeapon : Prop<MeleeWeapon>
{
    public MeleeWeapon(IDieBuilder randomNumbers, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, string actor)
        : base(randomNumbers, dispatchRegistry, actionRegistry, actor, "", "")
    {
    }
    public MeleeWeapon(IDieBuilder randomNumbers, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, string actor, string state, string uniqueId)
        : base(randomNumbers, dispatchRegistry, actionRegistry, TilesDisplay.DebugWeapon, state, uniqueId)
    {
    }

    private int Hit { get; set; }
    private int HitBonus { get; set; }
    private int Damage { get; set; }
    private int DamageBonus { get; set; }
}