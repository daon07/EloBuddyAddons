using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace JokerFizzBuddy
{
    public static class SpellManager
    {
        public static Spell.Targeted Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }
        public static Spell.Targeted IG { get; private set; }
        public static Spell.Targeted SMITE { get; private set; }
        public static Spell.Skillshot FLASH { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 550);
            W = new Spell.Active(SpellSlot.W, (uint)Player.Instance.GetAutoAttackRange());
            E = new Spell.Skillshot(SpellSlot.E, 400, SkillShotType.Circular, 250, int.MaxValue, 330);
            R = new Spell.Skillshot(SpellSlot.R, 1300, SkillShotType.Linear, 250, 1200, 80);

            E.AllowedCollisionCount = int.MaxValue;
            R.AllowedCollisionCount = 0;

            if (Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown)
                IG = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("summonerdot"), 600);

            if (Player.Instance.GetSpellSlotFromName("summonerflash") != SpellSlot.Unknown)
                FLASH = new Spell.Skillshot(Player.Instance.GetSpellSlotFromName("summonerflash"), 425, SkillShotType.Linear);
        }

        public static void CastR(Obj_AI_Base target, string mode)
        {
            if (R.IsReady())
            {
                Vector3 endPos = R.GetPrediction(target).CastPosition.Extend(Player.Instance.Position, -(600)).To3D();

                if(!target.HasBuff("summonerbarrier") || !target.HasBuff("BlackShield") || !target.HasBuff("SivirShield") || !target.HasBuff("BansheesVeil") || !target.HasBuff("ShroudofDarkness"))
                {
                    switch (mode)
                    {
                        case "Always":
                            R.Cast(endPos);
                            break;
                        case "Only if killable":
                            if (Modes.PermaActive.GetComboDamage(target) >= target.Health)
                                R.Cast(endPos);
                            break;
                    }
                }
            }
        }

        public static void Initialize()
        {

        }
    }
}
