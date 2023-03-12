using Assets.PlayerBuilder;
using WeaponStatistics = System.Collections.Generic.List<(string Title, string Damage, double Weight, int Level, int Cost, int DamageBonus, int HitBonus)>;

namespace Assets.Props;

// https://beej.us/moria/mmspoilers/items.html#weapons
// https://beej.us/moria/items.txt

public class MeleeWeaponBuilder
{
    private static WeaponStatistics Weapons = new()
    {
        ("Broken Sword",                     "1d1",  7.5,  0,  24,  -2, -2),
        ("Broken Dagger",                    "1d1",  1.5,  0,  0,   -2, -2),
        ("Dagger (Misericorde)",             "1d4",  1.5,  0,  10,   0,  0),
        ("Dagger (Stiletto)",                "1d4",  1.2,  0,  10,   0,  0),
        ("Hands (ie No weapon)",             "1d2",  0.0,  0,  0,   -2, -2),
        ("Wooden Club",                      "1d3",  10.0, 0,  10,   0,  0),
        ("Dagger (Bodkin)",                  "1d4",  2.0,  1,  10,   0,  0),
        ("Dagger (Main Gauche)",             "1d5",  3.0,  2,  25,   0,  0),
        ("Cat-o'-Nine Tails",                "1d4",  4.0,  3,  14,   0,  0),
        ("Javelin",                          "1d4",  3.0,  4,  18,   0,  0),
        ("Rapier",                           "1d6",  4.0,  4,  42,   0,  0),
        ("Thrusting Sword (Bilbo)",          "1d6",  8.0,  4,  60,   0,  0),
        ("Sabre",                            "1d7",  5.0,  5,  50,   0,  0),
        ("Small Sword",                      "1d6",  7.5,  5,  48,   0,  0),
        ("Spear",                            "1d6",  5.0,  5,  36,   0,  0),
        ("Thrusting Sword (Baselard)",       "1d7",  10.0, 5,  80,   0,  0),
        ("War Hammer",                       "3d3",  12.0, 5,  225,  0,  0),
        ("Mace",                             "2d4",  12.0, 6,  130,  0,  0),
        ("Backsword",                        "1d9",  9.5,  7,  150,  0,  0),
        ("Cutlass",                          "1d7",  11.0, 7,  85,   0,  0),
        ("Awl-Pike",                         "1d8",  16.0, 8,  200,  0,  0),
        ("Broadsword",                       "2d5",  15.0, 9,  255,  0,  0),
        ("Lance",                            "2d8",  30.0, 10, 230,  0,  0),
        ("Morningstar",                      "2d6",  15.0, 10, 396,  0,  0),
        ("Lucerne Hammer",                   "2d5",  12.0, 11, 376,  0,  0),
        ("Flail",                            "2d6",  15.0, 12, 353,  0,  0),
        ("Longsword",                        "1d10", 13.0, 12, 200,  0,  0),
        ("Battle Axe (European)",            "3d4",  17.0, 13, 334,  0,  0),
        ("Bastard Sword",                    "3d4",  14.0, 14, 350,  0,  0),
        ("Beaked Axe",                       "2d6",  18.0, 15, 408,  0,  0),
        ("Lead-filled Mace",                 "3d4",  18.0, 15, 502,  0,  0),
        ("Pike",                             "2d5",  16.0, 15, 358,  0,  0),
        ("Broad Axe",                        "2d6",  16.0, 17, 304,  0,  0),
        ("Fauchard",                         "1d10", 17.0, 17, 376,  0,  0),
        ("Katana",                           "3d4",  12.0, 18, 400,  0,  0),
        ("Ball and Chain",                   "2d4",  15.0, 20, 200,  0,  0),
        ("Glaive",                           "2d6",  19.0, 20, 363,  0,  0),
        ("Halberd",                          "3d4",  19.0, 22, 430,  0,  0),
        ("Battle Axe (Balestarius)",         "2d8",  18.0, 30, 500,  0,  0),
        ("Two-Handed Sword (Claymore)",      "3d6",  20.0, 30, 775,  0,  0),
        ("Foil",                             "1d5",  3.0,  35, 2,    0,  0),
        ("Two-Handed Sword (Espadon)",       "3d6",  18.0, 35, 655,  0,  0),
        ("Executioner's Sword",              "4d5",  26.0, 40, 850,  0,  0),
        ("Two-Handed Great Flail",           "3d6",  28.0, 45, 590,  0,  0),
        ("Two-Handed Sword (Flamberge)",     "4d5",  24.0, 45, 1000, 0,  0),
        ("Two-Handed Sword (No-Dachi)",      "4d4",  20.0, 45, 675,  0,  0),
        ("Two-Handed Sword (Zweihander)",    "4d6",  28.0, 50, 1500, 0,  0),
    };
}