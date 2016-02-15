using System.Linq;
using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;

using Settings = JokerFizzBuddy.Config.Modes.Perma;
using ComboSettings = JokerFizzBuddy.Config.Modes.Combo;

namespace JokerFizzBuddy.Modes
{
    public sealed class PermaActive : ModeBase
    {

        public override bool ShouldBeExecuted()
        {
            
            return true;
        }

        public override void Execute()
        {
            if(Config.Drawings.ShowKillable)
                DamageIndicator.DamageToUnit = GetComboDamage;

            if (Player.Instance.GetSpellSlotFromName("summonerdot") == SpellSlot.Summoner1 ||
               Player.Instance.GetSpellSlotFromName("summonerdot") == SpellSlot.Summoner2)
            {
                if (ObjectManager.Player.IsDead || !IG.IsReady() || !Settings.UseIgnite) return;
                if (ObjectManager.Get<AIHeroClient>().Where(
                    h =>
                        h.IsValidTarget(IG.Range) &&
                        h.Health <
                        ObjectManager.Player.GetSummonerSpellDamage(h, DamageLibrary.SummonerSpells.Ignite)).Count() <=
                    0) return;

                var target = ObjectManager.Get<AIHeroClient>()
                    .Where(
                        h =>
                            h.IsValidTarget(IG.Range) &&
                            h.Health <
                            ObjectManager.Player.GetSummonerSpellDamage(h, DamageLibrary.SummonerSpells.Ignite));
                if (Config.Modes.Perma.igniteMode.Equals("0"))
                    IG.Cast(target.First());
                else
                {
                    if (target.First().Distance(Player.Instance) > 450 || (Player.Instance.HealthPercent < 25))
                    {

                        IG.Cast(target.First());
                    }
                }
            }
        }

        private static float GetComboDamage(AIHeroClient unit)
        {
            var d = 0f;

            if (SpellManager.Q.IsReady() && ComboSettings.UseQ)
                d += Player.Instance.GetSpellDamage(unit, SpellSlot.Q);

            if (SpellManager.W.IsReady() && ComboSettings.UseW)
                d += Player.Instance.GetSpellDamage(unit, SpellSlot.W);

            if (SpellManager.E.IsReady() && ComboSettings.UseE)
                d += Player.Instance.GetSpellDamage(unit, SpellSlot.E);

            if (SpellManager.R.IsReady() && ComboSettings.UseR)
                d += Player.Instance.GetSpellDamage(unit, SpellSlot.R);

            if ((Player.Instance.GetSpellSlotFromName("summonerdot") == SpellSlot.Summoner1 ||
                            Player.Instance.GetSpellSlotFromName("summonerdot") == SpellSlot.Summoner2) && Settings.UseIgnite && SpellManager.IG.IsReady())
                d += Player.Instance.GetSummonerSpellDamage(unit, DamageLibrary.SummonerSpells.Ignite);

            if (ItemManager.BOTRK.IsReady() && ItemManager.BOTRK.IsOwned())
                d += Player.Instance.GetItemDamage(unit, ItemId.Blade_of_the_Ruined_King);

            if (ItemManager.Cutl.IsReady() && ItemManager.Cutl.IsOwned())
                d += Player.Instance.GetItemDamage(unit, ItemId.Bilgewater_Cutlass);

            if (ItemManager.Hydra.IsReady() && ItemManager.Hydra.IsOwned())
                d += Player.Instance.GetItemDamage(unit, ItemId.Ravenous_Hydra_Melee_Only);

            if (ItemManager.Tiamat.IsReady() && ItemManager.Tiamat.IsOwned())
                d += Player.Instance.GetItemDamage(unit, ItemId.Ravenous_Hydra_Melee_Only);

            if (ItemManager.HexTech.IsReady() && ItemManager.HexTech.IsOwned())
                d += Player.Instance.GetItemDamage(unit, ItemId.Hextech_Gunblade);

            d += Player.Instance.GetAutoAttackDamage(unit) * 3;

            return d;
        }
    }
}
