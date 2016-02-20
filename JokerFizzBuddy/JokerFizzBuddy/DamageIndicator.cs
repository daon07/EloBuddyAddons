using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System.Drawing;
using SharpDX;

using Settings = JokerFizzBuddy.Config.Drawings;
namespace JokerFizzBuddy
{
    public static class DamageIndicator
    {
        public delegate float DamageToUnitDelegate(AIHeroClient hero);

        private const int xOffset = 5;
        private const int yOffset = 20;

        public static DamageToUnitDelegate DamageToUnit { get; set; }
        public static Text TextKillable { get; private set; }

        static DamageIndicator()
        {
            TextKillable = new Text("", new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold)) { Color = System.Drawing.Color.Red };
            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        static void Drawing_OnEndScene(EventArgs args)
        {
            if (Settings.ShowKillable)
            {
                foreach (var unit in ObjectManager.Get<AIHeroClient>().Where(h => h.IsValid && h.IsHPBarRendered && h.IsEnemy))
                {

                    var barPos = unit.HPBarPosition;
                    var damage = DamageToUnit(unit);
                    var percentHealthAfterDamage = ((unit.Health - damage) > 0 ? (unit.Health - damage) : 0) / unit.MaxHealth;

                    if (damage >= unit.Health)
                    {
                        TextKillable.Position = new Vector2((int)barPos.X - 12, (int)barPos.Y + yOffset + 20);
                        TextKillable.TextValue = "1 Shot - " + Math.Round(Convert.ToDecimal(percentHealthAfterDamage * 100), 2) + "% HP Left!";
                        TextKillable.Color = System.Drawing.Color.LimeGreen;
                    }
                    else if (Math.Round(Convert.ToDecimal(percentHealthAfterDamage * 100), 2) < 50)
                    {
                        TextKillable.Position = new Vector2((int)barPos.X - 35, (int)barPos.Y + yOffset + 20);
                        TextKillable.TextValue = "2 to 5 Shots - " + Math.Round(Convert.ToDecimal(percentHealthAfterDamage * 100), 2) + "% HP Left!";
                        TextKillable.Color = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        TextKillable.Position = new Vector2((int)barPos.X - 35, (int)barPos.Y + yOffset + 20);
                        TextKillable.TextValue = "5 to 10 Shots - " + Math.Round(Convert.ToDecimal(percentHealthAfterDamage * 100), 2) + "% HP Left!";
                        TextKillable.Color = System.Drawing.Color.Red;
                    }

                    TextKillable.Draw();
                }
            }
        }

        public static void Initialize()
        {

        }
    }
}
