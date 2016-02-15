using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using System.Linq;
using Settings = JokerFizzBuddy.Config.Modes.JungleClear;

namespace JokerFizzBuddy.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Player.Instance.ManaPercent <= Settings.Mana)
                return;

            var mob = EntityManager.MinionsAndMonsters.Monsters.FirstOrDefault(m => m.IsValidTarget(Q.Range));

            if (Settings.UseQ && Q.IsReady() && mob.IsValidTarget(Q.Range))
            {
                Q.Cast(mob);
            }

            if (Settings.UseW && W.IsReady() && mob.IsValidTarget(W.Range))
            {
                W.Cast();
            }

            if (Settings.UseTiamatHydra)
                ItemManager.useHydra(mob);

            if (Settings.UseE && E.IsReady() && mob.IsValidTarget(E.Range))
            {
                E.Cast(mob);
            }
        }
    }
}
