using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;

using Settings = JokerFizzBuddy.Config.Modes.Harass;

namespace JokerFizzBuddy.Modes
{
    public sealed class Harass : ModeBase
    {
        private void EWQHarass(Obj_AI_Base target)
        {
            var EnoughManaEWQ = false;
            var EnoughManaEQ = false;
            var startPos = Vector3.Zero;
            var UseEWQ = Settings.UseQ && Settings.UseW && Settings.UseE;
            var TotalManaEWQ = Player.GetSpell(Q.Slot).SData.Mana + Player.GetSpell(W.Slot).SData.Mana + Player.GetSpell(E.Slot).SData.Mana;
            var TotalManaEQ = Player.GetSpell(Q.Slot).SData.Mana + Player.GetSpell(E.Slot).SData.Mana;

            if (UseEWQ && Q.IsReady())
            {
                if (Player.Instance.Mana >= TotalManaEWQ || EnoughManaEWQ)
                {
                    if (E.IsReady() && E.Name == "FizzJump" && Player.Instance.Distance(target.Position) <= 530)
                    {
                        EnoughManaEWQ = true;
                        startPos = Player.Instance.Position;
                        Vector3 harassEcastPos = E.GetPrediction(target).CastPosition;
                        E.Cast(harassEcastPos);

                        Core.DelayAction(() =>
                        {
                            E.Cast(E.GetPrediction(target).CastPosition.Extend(startPos, -135).To3DWorld());
                        }, (365 - Game.Ping));
                    }

                    if (W.IsReady() && Player.Instance.Distance(target.Position) <= 175)
                    {
                        W.Cast();
                        EnoughManaEWQ = false;
                    }

                    if (Q.IsReady())
                        Q.Cast(target);
                }

                if (Player.Instance.Mana >= TotalManaEQ || EnoughManaEQ)
                {
                    if (E.IsReady() && E.Name == "FizzJump" && Player.Instance.Distance(target.Position) <= 530)
                    {
                        EnoughManaEQ = true;
                        startPos = Player.Instance.Position;
                        Vector3 harassECastPos = E.GetPrediction(target).CastPosition;

                        E.Cast(harassECastPos);
                        Core.DelayAction(() =>
                        {
                            E.Cast(E.GetPrediction(target).CastPosition.Extend(startPos, -135).To3DWorld());
                            EnoughManaEQ = false;
                        }, (365 - Game.Ping));
                    }

                    if (Q.IsReady())
                        Q.Cast(target);
                }
            }

            else
            {
                if (Settings.UseW && W.IsReady() && Player.Instance.Distance(target.Position) <= Q.Range) W.Cast();
                if (Settings.UseQ && Q.IsReady() && Player.Instance.Distance(target.Position) <= Q.Range)
                    Q.Cast(target);
            }

            if (Settings.UseTiamatHydra)
                ItemManager.useHydra(target);
        }

        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.SelectedTarget;

            if (target == null || !target.IsValidTarget())
                target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (target != null && target.IsValidTarget(Q.Range))
                EWQHarass(target);
        }
    }
}