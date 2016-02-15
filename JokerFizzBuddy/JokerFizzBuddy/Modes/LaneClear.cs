using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using System.Linq;
using Settings = JokerFizzBuddy.Config.Modes.LaneClear;

namespace JokerFizzBuddy.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Player.Instance.ManaPercent <= Settings.Mana)
                return;

            if (Settings.UseW && W.IsReady())
            {
                var minion = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(W.Range));
                if (minion == null) return;

                W.Cast();

                if (Settings.UseTiamatHydra)
                    ItemManager.useHydra(minion);
            }

            if (Settings.UseQ && Q.IsReady())
            {
                var minion = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Q.Range));
                if (minion == null) return;

                if(Q.IsInRange(minion) && minion.Health <= Player.Instance.GetSpellDamage(minion, Q.Slot))
                    Q.Cast(minion);

            }

            if (Settings.UseE && E.IsReady())
            {
                var minion = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(E.Range));
                if (minion == null) return;

                if (E.IsInRange(minion))
                    E.Cast(minion);
            }
        }
    }
}
