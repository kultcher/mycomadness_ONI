using HarmonyLib;
using MycobrickMod.Elements; // To access MycofiberElement

namespace MycobrickMod.Patches
{
    [HarmonyPatch(typeof(Assets), "SubstanceListHookup")]
    public static class Assets_SubstanceListHookup_Patch
    {
        public static void Postfix()
        {
            Debug.Log("[MycobrickMod] Assets_SubstanceListHookup_Patch Postfix: Registering MycofiberElement substance into Assets.substanceTable.");
            MycofiberElement.RegisterMycofiberSubstance(); // This calls the modified ElementUtil.CreateRegisteredSubstance
        }
    }
}