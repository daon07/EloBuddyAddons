using System;
using System.Drawing;
using System.Runtime.Remoting.Channels;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;
using JokerFizzBuddy.Misc;
using JokerFizzBuddy.Modes;

using ComboSettings = JokerFizzBuddy.Config.Modes.Combo;
using DrawSettings = JokerFizzBuddy.Config.Drawings;
using MiscSettings = JokerFizzBuddy.Config.Misc;

namespace JokerFizzBuddy
{
    public static class Program
    {
        public const string ChampName = "Fizz";
        public static bool doOnce = false;
        public static float RCooldownTimer = 0;
        public static bool CanCastZhonyaOnDash = false;
        public static Geometry.Polygon.Rectangle RRectangle;
        public static Vector3 LastHarassPos { get; set; }
        public static bool JumpBack { get; set; }
        public static int SkinID = 0;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != ChampName)
                return;

            SkinID = Player.Instance.SkinId;

            Config.Initialize();
            ModeManager.Initialize();
            ItemManager.Initialize();
            SpellManager.Initialize();

            RRectangle = new Geometry.Polygon.Rectangle(Player.Instance.Position, Player.Instance.Position, 300);

            //Notification.DrawNotification(new NotificationModel(Game.Time, 5f, 2f, Player.Instance.ChampionName + " Loaded.", Color.Green));
            UpdateChecker.CheckForUpdates();

            Player.SetSkinId(Config.Misc.SkinID);
            Drawing.OnDraw += OnDraw;
            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;

        }

        static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender is Obj_AI_Turret && args.Target.IsMe && SpellManager.E.IsReady() && MiscSettings.UseAutoEOnTurrets)
            {
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Ally).
                    Where(m => m.IsValidTarget(SpellManager.E.Range * 2)).ToArray();
                if (minions.Length != 0 && MiscSettings.UseAutoEOnTurretsMinions)
                {
                    var minionList = minions.OrderBy(m => m.Distance(Player.Instance.Position));

                    var closestMinion = minionList.First();
                    var furthestMinion = minionList.Last();

                    SpellManager.E.Cast(Player.Instance.Position.Extend(closestMinion.Position, SpellManager.E.Range - 1).To3DWorld());
                    Core.DelayAction(() =>
                    {
                        SpellManager.E.Cast(Player.Instance.Position.Extend(furthestMinion.Position, SpellManager.E.Range - 1).To3DWorld());
                    }, (365 - Game.Ping));
                }
                else if (MiscSettings.UseAutoEOnTurretsCursor)
                {
                    SpellManager.E.Cast(Player.Instance.Position.Extend(Game.CursorPos, SpellManager.E.Range - 1).To3DWorld());
                    Core.DelayAction(() =>
                    {
                        SpellManager.E.Cast(Player.Instance.Position.Extend(Game.CursorPos, SpellManager.E.Range - 1).To3DWorld());
                    }, (365 - Game.Ping));
                }
            }
        }

        static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
                return;


            if (args.SData.Name == "FizzPiercingStrike" && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                Core.DelayAction(() =>
                    {
                        JumpBack = true;
                    }, (int)(sender.Spellbook.CastEndTime - Game.Time) + Game.Ping / 2 + 250);

            if (args.SData.Name == "fizzjumptwo" || args.SData.Name == "fizzjumpbuffer")
            {
                LastHarassPos = Vector3.Zero;
                JumpBack = false;
            }
        }

        static void OnDraw(EventArgs args)
        {
            if (DrawSettings.DrawQ)
                Circle.Draw(SpellManager.Q.IsReady() ? DrawSettings.CurrentColor : SharpDX.Color.Red,
                    SpellManager.Q.Range, 3F, Player.Instance.Position);

            if (DrawSettings.DrawW)
                Circle.Draw(SpellManager.W.IsReady() ? DrawSettings.CurrentColor : SharpDX.Color.Red,
                    SpellManager.W.Range, 3F, Player.Instance.Position);

            if (DrawSettings.DrawE)
                Circle.Draw(SpellManager.E.IsReady() ? DrawSettings.CurrentColor : SharpDX.Color.Red,
                    SpellManager.E.Range, 3F, Player.Instance.Position);

            if (DrawSettings.DrawR)
                Circle.Draw(SpellManager.R.IsReady() ? DrawSettings.CurrentColor : SharpDX.Color.Red,
                    SpellManager.R.Range, 3F, Player.Instance.Position);
        }

        static void OnUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.IsRecalling())
                return;

            var target = TargetSelector.SelectedTarget;
            if (target == null || !target.IsValidTarget())
                target = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Magical);

            if (target != null && target.IsValidTarget(SpellManager.R.Range))
            {
                if (Player.Instance.Distance(target) <= SpellManager.R.Range + SpellManager.Q.Range + 100)
                {
                    RRectangle.Start = Player.Instance.Position.Shorten(target.Position, -250).To2D();
                    RRectangle.End = SpellManager.R.GetPrediction(target).CastPosition.Extend(Player.Instance.Position, -330);
                    RRectangle.UpdatePolygon();
                }
            }

            if (SpellManager.R.IsReady())
                doOnce = true;

            if (Player.Instance.LastCastedSpellT() >= 1)
            {
                if (doOnce && Player.Instance.LastCastedspell().Name == "FizzMarinerDoom")
                {
                    RCooldownTimer = Game.Time;
                    doOnce = false;
                }
            }

            if (Game.Time - RCooldownTimer <= 5.0f)
                CanCastZhonyaOnDash = true;
            else
                CanCastZhonyaOnDash = false;

            if (ComboSettings.UseEFlashZhonyasCombo)
                Combo.EFlashCombo();

            if (ComboSettings.UseLateGameZhonyasCombo)
                Combo.LateGameZhonyasCombo();

            if (ComboSettings.UseQminionREWCombo)
                Combo.QminionREWCombo();
        }
    }
}
