namespace Assets.PlayerBuilder;

// https://github.com/jhirschberg70/browser-based-umoria/blob/f9fcf9ce217922be4941c7397007f5635ff2f838/src/player.h#L100
// taken from flags in an attempt to give meaning

public class PlayerSpells
{
    public int NumSpellsCanLearn;      // Number of spells can learn. new_spells_to_learn
    public int SpellsLearned;           // bit mask of spells learned
    public int SpellsWorked;           // bit mask of spells tried and worked
    public int SpellsForgotten;        // bit mask of spells learned but forgotten
    public int[] OrderSpellsLearnedIn = new int[32]; // order spells learned/remembered/forgotten
}