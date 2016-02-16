using System;
using System.Drawing;
using System.Runtime.Remoting.Channels;
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

namespace JokerFizzBuddy
{
    public static class Program
    {
        public const string ChampName = "Fizz";
        public static bool doOnce = false;
        public static float RCooldownTimer = 0;
        public static bool CanCastZhonyaOnDash = false;
        public static Geometry.Polygon.Rectangle RRectangle;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != ChampName)
                return;

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
