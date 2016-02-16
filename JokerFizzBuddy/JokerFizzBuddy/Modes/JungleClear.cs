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
                if (E.IsInRange(mob) && E.Name == "FizzJump")
                {
                    var castPos = Player.Instance.Distance(Prediction.Position.PredictUnitPosition(mob, 1)) > E.Range ?
    Player.Instance.Position.Extend(Prediction.Position.PredictUnitPosition(mob, 1), E.Range).To3DWorld() : mob.Position;

                    //var castPos = E.GetPrediction(target).CastPosition;
                    E.Cast(castPos);

                    var pred2 = Prediction.Position.PredictUnitPosition(mob, 1).Distance(Player.Instance.Position) <= (200 + 330 + mob.BoundingRadius);

                    if (pred2)
                        Player.IssueOrder(GameObjectOrder.MoveTo, Prediction.Position.PredictUnitPosition(mob, 1).To3DWorld());
                    else
                        E.Cast(Prediction.Position.PredictUnitPosition(mob, 1).To3DWorld());
                    //E.Cast(minion);
                }
            }
        }
    }
}
