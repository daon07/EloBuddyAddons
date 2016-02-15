using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

namespace JokerFizzBuddy
{
    public static class ItemManager
    {
        public static Item Hydra { get; private set; }
        public static Item BOTRK { get; private set; }
        public static Item Cutl { get; private set; }
        public static Item Tiamat { get; private set; }
        public static Item HexTech { get; private set; }
        public static Item Zhonya { get; private set; }

        static ItemManager()
        {
            Hydra = new Item((int)ItemId.Ravenous_Hydra_Melee_Only, 400);
            BOTRK = new Item((int)ItemId.Blade_of_the_Ruined_King, 450);
            Cutl = new Item((int)ItemId.Bilgewater_Cutlass, 450);
            Tiamat = new Item((int)ItemId.Tiamat_Melee_Only, 400);
            HexTech = new Item((int)ItemId.Hextech_Gunblade, 700);
            Zhonya = new Item((int)ItemId.Zhonyas_Hourglass);
        }

        public static void useHydra(Obj_AI_Base target)
        {
            if (Tiamat.IsOwned() || Hydra.IsOwned())
            {
                if ((Tiamat.IsReady() || Hydra.IsReady()) && Player.Instance.Distance(target) <= Hydra.Range)
                {
                    Tiamat.Cast();
                    Hydra.Cast();
                }
            }
        }

        public static void useHydraNot()
        {
            if (Tiamat.IsOwned() || Hydra.IsOwned())
            {
                if (Tiamat.IsReady() || Hydra.IsReady())
                {
                    Tiamat.Cast();
                    Hydra.Cast();
                }
            }
        }

        public static void UseCastables(Obj_AI_Base t)
        {
            if (HexTech.IsOwned())
            {
                if (HexTech.IsReady())
                    HexTech.Cast(t);
            }
            if (BOTRK.IsOwned() || Cutl.IsOwned())
            {
                if (BOTRK.IsReady() || Cutl.IsReady())
                {
                    BOTRK.Cast(t);
                    Cutl.Cast(t);
                }
            }
        }

        public static void UseZhonyas()
        {
            if (Zhonya.IsOwned() && Zhonya.IsReady())
                Zhonya.Cast();
        }

        public static void Initialize()
        {

        }
    }
}
