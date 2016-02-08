using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

namespace JokerFioraBuddy
{
    public static class ItemManager
    {

        public static Item Hydra { get; private set; }
        public static Item BOTRK { get; private set; }
        public static Item Cutl { get; private set; }
        public static Item Tiamat { get; private set; }
        public static Item Yomu { get; private set; }
        public static Item Sheen { get; private set; }
        public static Item TriForce { get; private set; }

        static ItemManager()
        {
            Hydra = new Item((int)ItemId.Ravenous_Hydra_Melee_Only, 400);
            BOTRK = new Item((int)ItemId.Blade_of_the_Ruined_King, 450);
            Cutl = new Item((int)ItemId.Bilgewater_Cutlass, 450);
            Tiamat = new Item((int)ItemId.Tiamat_Melee_Only, 400);
            Yomu = new Item((int)ItemId.Youmuus_Ghostblade);
            Sheen = new Item((int)ItemId.Sheen);
            TriForce = new Item((int)ItemId.Trinity_Force);
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

        public static void useHydraNot(Obj_AI_Base target)
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

        public static void UseYomu()
        {
            if (Yomu.IsOwned() && Yomu.IsReady())
                Yomu.Cast();
        }

        public static void UseCastables()
        {
            if (BOTRK.IsOwned() || Cutl.IsOwned())
            {
                var t = TargetSelector2.GetTarget(BOTRK.Range, DamageType.Physical);
                if (t == null || !t.IsValidTarget()) return;

                if (BOTRK.IsReady() || Cutl.IsReady())
                {
                    BOTRK.Cast(t);
                    Cutl.Cast(t);
                }
            }
        }

        public static void Initialize()
        {
 
        }
    }
}
