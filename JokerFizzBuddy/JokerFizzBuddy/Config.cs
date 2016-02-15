using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace JokerFizzBuddy
{
    public static class Config
    {
        private const string MenuName = "Joker Fizz 1.0.0.0";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Welcome to Joker Fizz Addon!");
            Menu.AddLabel("Features:");
            Menu.AddLabel("7 Types of Combos!");
            Menu.AddLabel("Gap Close Combo - R at safe distance!");
            Menu.AddLabel("Before Dash Combo - R when in range of Q, before dashing!");
            Menu.AddLabel("After Dash Combo - R after dashing!");
            Menu.AddLabel("On Dash Combo - R while dashing (cancels R animation)!");
            Menu.AddLabel("Late Game Zhonya Combo - EE > Gapclose > R > W > Q + Zhonya");
            Menu.AddLabel("E Flash Combo - E -> Flash on target -> 2nd E -> R -> W -> Q + Zhonya");
            Menu.AddLabel("Q Minion Combo - Q to minion/monster/champion to gapclose -> R -> W -> Q");
            Menu.AddLabel("E has it's own logic, tries to hit target with first E for AOE dmg and slow");
            Menu.AddLabel("Harass is intelligent, has it's own logic also!");
            Menu.AddLabel("Lane clear Logic!");
            Menu.AddLabel("Jungle clear logic!");
            Menu.AddLabel("Drawings on each spell!");

            Menu.AddLabel("Credits to: Danny - Main Coder");

            Modes.Initialize();
            Drawings.Initialize();
            Misc.Initialize();
        }

        public static void Initialize()
        {

        }

        public static class Drawings
        {
            public static readonly Menu Menu;

            public static bool ShowKillable
            {
                get { return Menu["damageKillable"].Cast<CheckBox>().CurrentValue; }
            }

            public static bool DrawQ
            {
                get { return Menu["drawq"].Cast<CheckBox>().CurrentValue; }
            }
            public static bool DrawW
            {
                get { return Menu["draww"].Cast<CheckBox>().CurrentValue; }
            }
            public static bool DrawE
            {
                get { return Menu["drawe"].Cast<CheckBox>().CurrentValue; }
            }
            public static bool DrawR
            {
                get { return Menu["drawr"].Cast<CheckBox>().CurrentValue; }
            }
            public static Color CurrentColor
            {
                get { return colorlist[Menu["mastercolor"].Cast<Slider>().CurrentValue]; }
            }

            public static CheckBox qdraw = new CheckBox("Draw Q", false);
            public static CheckBox wdraw = new CheckBox("Draw W", false);
            public static CheckBox edraw = new CheckBox("Draw E", false);
            public static CheckBox rdraw = new CheckBox("Draw R", false);
            static Color[] colorlist = { Color.Green, Color.Aqua, Color.Black, Color.Blue, Color.Firebrick, Color.Gold, Color.Pink, Color.Violet, Color.White, Color.Lime, Color.LimeGreen, Color.Yellow, Color.Magenta };
            static Slider masterColorSlider = new Slider("Color Slider", 0, 0, colorlist.Length - 1);

            static Drawings()
            {
                Menu = Config.Menu.AddSubMenu("Drawings");
                Menu.AddGroupLabel("Drawings");
                Menu.Add("damageKillable", new CheckBox("Show text if champion is killable"));
                Menu.Add("mastercolor", masterColorSlider);
                Menu.Add("drawq", qdraw);
                Menu.Add("draww", wdraw);
                Menu.Add("drawe", edraw);
                Menu.Add("drawr", rdraw);
            }

            public static void Initialize()
            {

            }
        }

        public static class Misc
        {
            private static readonly Menu Menu;

            public static int SkinID
            {
                get { return Menu["skinid"].Cast<Slider>().CurrentValue; }
            }

            public static bool enableSkinHack
            {
                get { return Menu["skinhack"].Cast<CheckBox>().CurrentValue; }
            }

            public static Slider SkinSlider = new Slider("SkinID : ({0})", 0, 0, 5);
            public static CheckBox SkinEnable = new CheckBox("Enable");
            public static CheckBox EvolveEnable = new CheckBox("Enable");

            static Misc()
            {
                Menu = Config.Menu.AddSubMenu("Misc");
                Menu.AddGroupLabel("Skin Hack");
                Menu.Add("skinhack", SkinEnable);
                Menu.Add("skinid", SkinSlider);
                SkinSlider.OnValueChange += SkinSlider_OnValueChange;
                SkinEnable.OnValueChange += SkinEnable_OnValueChange;
            }

            private static void SkinEnable_OnValueChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (!Misc.enableSkinHack)
                {
                    Player.SetSkinId(0);
                    return;
                }

                Player.SetSkinId(Misc.SkinID);
            }

            private static void SkinSlider_OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
            {
                if (!Misc.enableSkinHack)
                {
                    Player.SetSkinId(0);
                    return;
                }

                Player.SetSkinId(Misc.SkinID);
            }

            public static void Initialize()
            {

            }

        }

        public static class Modes
        {
            private static readonly Menu Menu;

            static Modes()
            {
                Menu = Config.Menu.AddSubMenu("Modes");

                Combo.Initialize();
                Menu.AddSeparator();

                Harass.Initialize();
                Menu.AddSeparator();

                LaneClear.Initialize();
                Menu.AddSeparator();

                JungleClear.Initialize();
                Menu.AddSeparator();

                Perma.Initialize();

            }

            public static void Initialize()
            {

            }

            public static class Combo
            {
                public static bool UseQ
                {
                    get { return Menu["comboUseQ"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseW
                {
                    get { return Menu["comboUseW"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseE
                {
                    get { return Menu["comboUseE"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseR
                {
                    get { return Menu["comboUseR"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseTiamatHydra
                {
                    get { return Menu["comboUseTiamatHydra"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseCutlassBOTRK
                {
                    get { return Menu["comboUseCutlassBOTRK"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseHexTech
                {
                    get { return Menu["comboUseHexTech"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseZhonya
                {
                    get { return Menu["comboUseZhonya"].Cast<CheckBox>().CurrentValue; }
                }

                public static string ComboSettings
                {
                    get { return Menu["comboSettings"].Cast<ComboBox>().SelectedText; }
                }

                public static bool UseLateGameZhonyasCombo
                {
                    get { return Menu["comboUseLateGameZhonyas"].Cast<KeyBind>().CurrentValue; }
                }

                public static bool UseZhonyasInLateGameZhonyasCombo
                {
                    get { return Menu["comboUseLateGameZhonyasItem"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseQminionREWCombo
                {
                    get { return Menu["comboUseQminionREW"].Cast<KeyBind>().CurrentValue; }
                }

                public static bool UseEFlashZhonyasCombo
                {
                    get { return Menu["comboUseEFlashZhonyas"].Cast<KeyBind>().CurrentValue; }
                }

                public static bool UseZhonyasInEFlashZhonyasCombo
                {
                    get { return Menu["comboUseEFlashZhonyasItem"].Cast<CheckBox>().CurrentValue; }
                }


                static Combo()
                {
                    Menu.AddGroupLabel("Combo");
                    Menu.Add("comboUseQ", new CheckBox("Use Q"));
                    Menu.Add("comboUseW", new CheckBox("Use W"));
                    Menu.Add("comboUseE", new CheckBox("Use E"));
                    Menu.Add("comboUseR", new CheckBox("Use R"));
                    Menu.Add("comboUseTiamatHydra", new CheckBox("Use Tiamat / Hydra"));
                    Menu.Add("comboUseCutlassBOTRK", new CheckBox("Use Bilgewater Cutlass / Blade of the Ruined King"));
                    Menu.Add("comboUseHexTech", new CheckBox("Use Hextech Gunblade"));
                    Menu.Add("comboUseZhonya", new CheckBox("Use Zhonyas"));
                    Menu.AddSeparator();
                    Menu.Add("comboSettings", new ComboBox("Combo Settings:", 0, new string[] { "Gap Closer", "On Dash", "After Dash", "Real On Dash" }));
                    Menu.Add("comboUseLateGameZhonyas", new KeyBind("Use Zhonyas with EE to gapclose R W Q", false, KeyBind.BindTypes.HoldActive, "G".ToCharArray()[0]));
                    Menu.Add("comboUseLateGameZhonyasItem", new CheckBox("Use Zhonyas in Late Game Zhonyas Combo (EE -> Gapclose -> R -> W -> Q -> Zhonyas)"));
                    Menu.Add("comboUseQminionREW", new KeyBind("Q minion/champ/monster to gapclose R E W", false, KeyBind.BindTypes.HoldActive, "H".ToCharArray()[0]));
                    Menu.Add("comboUseEFlashZhonyas", new KeyBind("E Flash on target R W Q and Zhonyas", false, KeyBind.BindTypes.HoldActive, "J".ToCharArray()[0]));
                    Menu.Add("comboUseEFlashZhonyasItem", new CheckBox("Use Zhonyas in E Flash Combo (E -> Flash -> R -> W -> Q -> Zhonyas)"));
                   
                }

                public static void Initialize()
                {

                }
            }

            public static class Harass
            {
                public static bool UseQ
                {
                    get { return Menu["harassUseQ"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseW
                {
                    get { return Menu["harassUseW"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseE
                {
                    get { return Menu["harassUseE"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseTiamatHydra
                {
                    get { return Menu["harassUseTiamatHydra"].Cast<CheckBox>().CurrentValue; }
                }

                static Harass()
                {
                    Menu.AddGroupLabel("Harrass");
                    Menu.Add("harassUseQ", new CheckBox("Use Q"));
                    Menu.Add("harassUseW", new CheckBox("Use W"));
                    Menu.Add("harassUseE", new CheckBox("Use E"));
                    Menu.Add("harassUseTiamatHydra", new CheckBox("Use Tiamat / Hydra"));
                }

                public static void Initialize()
                {

                }
            }

            public static class LaneClear
            {
                public static bool UseQ
                {
                    get { return Menu["lcUseQ"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseW
                {
                    get { return Menu["lcUseW"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseE
                {
                    get { return Menu["lcUseE"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseTiamatHydra
                {
                    get { return Menu["lcUseTiamatHydra"].Cast<CheckBox>().CurrentValue; }
                }

                public static int Mana
                {
                    get { return Menu["lcMana"].Cast<Slider>().CurrentValue; }
                }

                static LaneClear()
                {
                    Menu.AddGroupLabel("Lane Clear");
                    Menu.Add("lcUseQ", new CheckBox("Use Q"));
                    Menu.Add("lcUseW", new CheckBox("Use W"));
                    Menu.Add("lcUseE", new CheckBox("Use E"));
                    Menu.Add("lcUseTiamatHydra", new CheckBox("Use Tiamat / Hydra"));
                    Menu.Add("lcMana", new Slider("Maximum mana usage in percent ({0}%)", 40));
                }

                public static void Initialize()
                {

                }
            }

            public static class JungleClear
            {
                public static bool UseQ
                {
                    get { return Menu["jcUseQ"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseW
                {
                    get { return Menu["jcUseW"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseE
                {
                    get { return Menu["jcUseE"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseTiamatHydra
                {
                    get { return Menu["jcUseTiamatHydra"].Cast<CheckBox>().CurrentValue; }
                }

                public static int Mana
                {
                    get { return Menu["jcMana"].Cast<Slider>().CurrentValue; }
                }

                static JungleClear()
                {
                    Menu.AddGroupLabel("Jungle Clear");
                    Menu.Add("jcUseQ", new CheckBox("Use Q"));
                    Menu.Add("jcUseW", new CheckBox("Use W"));
                    Menu.Add("jcUseE", new CheckBox("Use E"));
                    Menu.Add("jcUseTiamatHydra", new CheckBox("Use Tiamat / Hydra"));
                    Menu.Add("jcMana", new Slider("Maximum mana usage in percent ({0}%)", 40));
                }

                public static void Initialize()
                {

                }
            }

            public static class Perma
            {
                static Slider igniteModeSlider = new Slider("Ignite Mode : Smart", 1, 0, 1);



                public static bool UseIgnite
                {
                    get { return Menu["permaUseIG"].Cast<CheckBox>().CurrentValue; }
                }

                public static int igniteMode
                {
                    get { return Menu["igniteMode"].Cast<Slider>().CurrentValue; }
                }


                static Perma()
                {
                    Menu.AddGroupLabel("Perma Active");
                    Menu.Add("permaUseIG", new CheckBox("Auto-Ignite Champions"));
                    Menu.Add("igniteMode", igniteModeSlider);

                }

                public static void Initialize()
                {
                    igniteModeSlider.OnValueChange += IgniteModeSlider_OnValueChange;
                }

                private static void IgniteModeSlider_OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    if (igniteModeSlider.CurrentValue == 0)
                        igniteModeSlider.DisplayName = "Ignite Mode : Normal Mode";
                    else
                        igniteModeSlider.DisplayName = "Ignite Mode : Smart Mode";
                }
            }
        }
    }
}
