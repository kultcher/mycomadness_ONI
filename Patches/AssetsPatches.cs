using HarmonyLib;
using MycobrickMod.Elements;
using MycobrickMod.Utils; // To access MycofiberElement

namespace MycobrickMod.Patches
{
    [HarmonyPatch(typeof(Assets), "SubstanceListHookup")]
    public static class Assets_SubstanceListHookup_Patch
    {
        public static void Prefix()
        {
            ElementUtil.RegisterElementStrings("MycofiberElement", "Mycofiber", "A tough, fibrous material harvested from Mycobrick Shrooms. It's surprisingly versatile, though not very durable on its own.");

        }
        public static void Postfix()
        {
            Debug.Log("[MycobrickMod] Assets_SubstanceListHookup_Patch Postfix: Registering MycofiberElement substance into Assets.substanceTable.");
            MycofiberElement.RegisterMycofiberSubstance(); // This calls the modified ElementUtil.CreateRegisteredSubstance
        }
    }
}