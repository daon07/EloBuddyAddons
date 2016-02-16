using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using Settings = JokerFizzBuddy.Config.Modes.Combo;

namespace JokerFizzBuddy.Modes
{
    public sealed class Combo : ModeBase
    {
        private void ComboMode(Obj_AI_Base target)
        {
            var UseQ = (Q.IsReady() && Settings.UseQ);
            var UseW = (W.IsReady() && Settings.UseW);
            var UseE = (E.IsReady() && Settings.UseE);
            var UseR = (R.IsReady() && Settings.UseR);

            var UseZhonya = (Settings.UseZhonya && ItemManager.Zhonya.IsOwned() && ItemManager.Zhonya.IsReady());

            switch (Settings.ComboSettings)
            {
                case "Gap Closer":
                    if (UseR && !target.IsZombie)
                    {
                        if (!target.IsFacing(Player.Instance))
                        {
                            if (Player.Instance.Distance(target.Position) < (R.Range - target.MoveSpeed) - (165))
                                SpellManager.CastR(target);
                        }
                        else
                        {
                            if (Player.Instance.Distance(target.Position) <= (R.Range - 200))
                                SpellManager.CastR(target);
                        }
                    }
                    if (Settings.UseHexTech || Settings.UseCutlassBOTRK)
                        ItemManager.UseCastables(target);

                    if (UseQ) Q.Cast(target);

                    if (UseW && Player.Instance.Distance(target.Position) <= 540) W.Cast();

                    if (Settings.UseTiamatHydra)
                        ItemManager.useHydra(target);
                    break;

                case "On Dash":
                    if (!target.IsZombie && UseR && Player.Instance.Distance(target.Position) <= 550)
                    {
                        if (Settings.UseHexTech || Settings.UseCutlassBOTRK)
                            ItemManager.UseCastables(target);
                        if (Settings.UseQ && Q.IsReady() && Player.Instance.Distance(target.Position) <= Q.Range)
                        {
                            if (Settings.UseR && R.IsReady())
                                SpellManager.CastR(target);

                            Q.Cast(target);
                        }
                        if (Settings.UseW && W.IsReady() && Player.Instance.Distance(target.Position) <= 540)
                            W.Cast();

                        if (Settings.UseTiamatHydra)
                            ItemManager.useHydra(target);
                    }
                    break;

                case "After Dash":
                    if (!target.IsZombie && UseR)
                    {
                        if (UseW && Player.Instance.Distance(target.Position) <= 540)
                            W.Cast();

                        if (Settings.UseHexTech || Settings.UseCutlassBOTRK)
                            ItemManager.UseCastables(target);

                        if (UseQ && Player.Instance.Distance(target.Position) <= Q.Range)
                        {
                            Q.Cast(target);
                            Core.DelayAction(() =>
                                {
                                    if (UseR)
                                        SpellManager.CastR(target);
                                }, (540 - Game.Ping));
                        }

                        if (Settings.UseTiamatHydra)
                            ItemManager.useHydra(target);
                    }
                    break;

                case "Real On Dash":
                    if (!target.IsZombie && UseR && Player.Instance.Distance(target.Position) <= 550)
                    {
                        if (Settings.UseHexTech || Settings.UseCutlassBOTRK)
                            ItemManager.UseCastables(target);

                        if (UseQ) Q.Cast(target);

                        if (UseR)
                        {
                            if (Player.Instance.Distance(target.Position) <= 380)
                            {
                                Core.DelayAction(() =>
                                    {
                                        SpellManager.CastR(target);
                                    }, (500 - Game.Ping));
                            }
                            else
                                SpellManager.CastR(target);
                        }

                        if (UseW && Player.Instance.Distance(target.Position) <= 540) 
                            W.Cast();

                        if (Settings.UseTiamatHydra) 
                            ItemManager.useHydra(target);
                    }
                    break;
            }
            if (UseW && Player.Instance.Distance(target.Position) <= 540) W.Cast();
            if (UseQ && Player.Instance.Distance(target.Position) <= Q.Range) Q.Cast(target);


            var prediction = Prediction.Position.PredictUnitPosition(target, 1).Distance(Player.Instance.Position) <= (E.Range + 200 + 330);

            if (E.Name == "FizzJump" && UseE && !W.IsReady() && !Q.IsReady() && !R.IsReady() && prediction)
            {

                var castPos = Player.Instance.Distance(Prediction.Position.PredictUnitPosition(target, 1)) > E.Range ?
                    Player.Instance.Position.Extend(Prediction.Position.PredictUnitPosition(target, 1), E.Range).To3DWorld() : target.Position;

                //var castPos = E.GetPrediction(target).CastPosition;
                E.Cast(castPos);

                var pred2 = Prediction.Position.PredictUnitPosition(target, 1).Distance(Player.Instance.Position) <= (200 + 330 + target.BoundingRadius);

                Console.WriteLine("pred2: " + pred2);
                if (pred2)
                    Player.IssueOrder(GameObjectOrder.MoveTo, Prediction.Position.PredictUnitPosition(target, 1).To3DWorld());
                else
                    E.Cast(Prediction.Position.PredictUnitPosition(target, 1).To3DWorld());

                if (UseZhonya && Settings.ComboSettings != "Real On Dash" && Program.CanCastZhonyaOnDash)
                {
                    Core.DelayAction(() =>
                        {
                            ItemManager.UseZhonyas();
                        }, (2150 - Game.Ping));
                }
            }
        }

        public static void LateGameZhonyasCombo()
        {
            Orbwalker.OrbwalkTo(Game.CursorPos);
            Player.IssueOrder(GameObjectOrder.AutoAttack, Game.CursorPos);

             var target = TargetSelector.SelectedTarget;
             if (target == null || !target.IsValidTarget())
                target = TargetSelector.GetTarget(R.Range, DamageType.Magical);

             if (target != null && target.IsValidTarget(R.Range))
             {
                 var distance = Player.Instance.Distance(target.Position);
                 var range = ((E.Range + Q.Range + E.Range) - 50);
                 if (distance <= range)
                 {
                     if (E.IsReady())
                     {
                         var castPos = E.GetPrediction(target).CastPosition.Extend(Player.Instance.Position, 135);
                         E.Cast(castPos.To3DWorld());
                     }

                     if (R.IsReady() && !E.IsReady())
                     {
                         if (!target.IsFacing(Player.Instance))
                         {
                             if (Player.Instance.Distance(target.Position) < (R.Range - target.MoveSpeed) - (165))
                                 SpellManager.CastR(target);
                         }
                         else
                             if (Player.Instance.Distance(target.Position) <= R.Range)
                                 SpellManager.CastR(target);
                     }

                     if (W.IsReady() && !E.IsReady())
                         W.Cast();

                     if (Q.IsReady() && !E.IsReady())
                         Q.Cast(target);

                     if (Player.Instance.LastCastedSpellName() == "FizzPiercingStrike" && Settings.UseZhonyasInLateGameZhonyasCombo)
                         ItemManager.UseZhonyas();
                 }
             }
        }

        public static void EFlashCombo()
        {
            var isEProcessed = false;
            Orbwalker.OrbwalkTo(Game.CursorPos);
            Player.IssueOrder(GameObjectOrder.AttackUnit, Game.CursorPos);

            var target = TargetSelector.SelectedTarget;
             if (target == null || !target.IsValidTarget())
                target = TargetSelector.GetTarget(R.Range, DamageType.Magical);

             if (target != null && target.IsValidTarget(R.Range))
             {
                 var distance = Player.Instance.Distance(target.Position);
                 var range = E.Range + FLASH.Range + 165;
                 if (distance <= range)
                 {
                     if (E.IsReady() && E.Name == "FizzJump")
                     {
                         var castPos = E.GetPrediction(target).CastPosition.Extend(Player.Instance.Position, range - 165);
                         E.Cast(castPos.To3DWorld());
                         Core.DelayAction(() => { isEProcessed = true; }, (990 - Game.Ping));
                     }

                     if (FLASH.IsReady() && !isEProcessed && Player.Instance.LastCastedSpellName() == "FizzJump" &&
                         Player.Instance.Distance(target.Position) <= FLASH.Range + 530 &&
                         Player.Instance.Distance(target.Position) >= 330) 
                     {
                         var endPos = FLASH.GetPrediction(target).CastPosition.Extend(Player.Instance.Position, range - 530);
                         FLASH.Cast(endPos.To3DWorld());
                     }

                     if (R.IsReady() && Settings.UseR && !FLASH.IsReady())
                         SpellManager.CastR(target);

                     if (W.IsReady() && Settings.UseW && !FLASH.IsReady() && Player.Instance.LastCastedSpellName() == "FizzMarinerDoom")
                         W.Cast();

                     if (Q.IsReady() && Settings.UseQ && !E.IsReady() && !FLASH.IsReady() && Player.Instance.LastCastedSpellName() == "FizzSeastonePassive")
                         Q.Cast(target);

                     if (Player.Instance.LastCastedSpellName() == "FizzPiercingStrike" && Settings.UseZhonyasInEFlashZhonyasCombo)
                         ItemManager.UseZhonyas();
                 }
             }
        }

        public static void QminionREWCombo()
        {
             Orbwalker.OrbwalkTo(Game.CursorPos);
             Player.IssueOrder(GameObjectOrder.AttackUnit, Game.CursorPos);

            var target = TargetSelector.SelectedTarget;
             if (target == null || !target.IsValidTarget())
                target = TargetSelector.GetTarget(R.Range, DamageType.Magical);

             if (target != null && target.IsValidTarget(R.Range))
             {
                 var distance = Player.Instance.Distance(target.Position);

                 var range = ((Q.Range + R.Range) - 600);

                 if (distance <= range)
                 {
                     if (Q.IsReady())
                     {
                         if (Player.Instance.Distance(target.Position) <= Q.Range)
                             Q.Cast(target);
                         else
                         {
                             var champions = EntityManager.Heroes.Enemies;

                             foreach (Obj_AI_Base champion in champions)
                             {
                                 if (Program.RRectangle.IsInside(champion.Position) && champion.Distance(target.Position) > 300
                                     && Player.Instance.Distance(champion.Position) <= Q.Range)
                                     Q.Cast(champion);
                             }

                             var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy);

                             foreach (Obj_AI_Base minion in minions)
                             {
                                 if (minion.IsValidTarget(Q.Range))
                                 {
                                     if (Program.RRectangle.IsInside(minion.Position) && minion.Distance(target.Position) > 300)
                                         Q.Cast(minion);
                                 }
                             }
                         }
                     }

                     if (R.IsReady() && !Q.IsReady())
                     {
                         Core.DelayAction(() =>
                             {
                                 if (!target.IsFacing(Player.Instance))
                                 {
                                     if (Player.Instance.Distance(target.Position) < (R.Range - target.MoveSpeed) - (165))
                                         SpellManager.CastR(target);
                                 }
                                 else
                                     if (Player.Instance.Distance(target.Position) <= R.Range)
                                         SpellManager.CastR(target);

                             }, (540 - Game.Ping));
                     }

                     if (E.IsReady() && Player.Instance.LastCastedSpellName() == "FizzMarinerDoom")
                     {
                         if (E.Name == "FizzJump")
                         {
                             var castPos = E.GetPrediction(target).CastPosition.Extend(Player.Instance.Position, -165);
                             E.Cast(castPos.To3DWorld());
                         }

                         if (E.Name == "fizzjumptwo" && Player.Instance.Distance(target.Position) > 330)
                         {
                             var castPos = E.GetPrediction(target).CastPosition.Extend(Player.Instance.Position, -135);
                             E.Cast(castPos.To3DWorld());
                         }
                     }

                     if (W.IsReady() && Player.Instance.LastCastedSpellName() == "FizzJump")
                         W.Cast();
                 }
             }
        }

        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.SelectedTarget;

            if (target == null || !target.IsValidTarget())
                target = TargetSelector.GetTarget(R.Range, DamageType.Magical);

            if (target != null && target.IsValidTarget(R.Range))
                ComboMode(target);
        }
    }
}
