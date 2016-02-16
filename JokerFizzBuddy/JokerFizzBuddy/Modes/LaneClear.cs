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
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .Where(
                    m => m.IsValidTarget(E.Range)).ToArray();
                if (minions.Length == 0) return;

                if (E.Name == "FizzJump")
                {
                    var castPos = Prediction.Position.PredictCircularMissileAoe(minions, E.Range, E.Width,
                        E.CastDelay, E.Speed).OrderByDescending(r => r.GetCollisionObjects<Obj_AI_Minion>().Length).FirstOrDefault();

                    if (castPos != null)
                    {
                        var predictMinion = castPos.GetCollisionObjects<Obj_AI_Minion>();

                        if (predictMinion.Length >= 2)
                        {
                            //var castPos = E.GetPrediction(target).CastPosition;
                            E.Cast(castPos.CastPosition);

                            Player.IssueOrder(GameObjectOrder.MoveTo, castPos.CastPosition);
                        }
                    }
                }
            }
        }
    }
}
