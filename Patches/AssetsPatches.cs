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
            ElementUtil.RegisterElementStrings("MycobrickElement", "Mycobrick", "Tough, fire-resistant bricks made from processed Mycofiber. Excellent for construction.");
            Strings.Add("STRINGS.MISC.TAGS.MYCOFIBERELEMENT", "Mycofiber");     // not sure why this one isn't being added automatically?
            Strings.Add("STRINGS.MISC.TAGS.MYCOBRICKELEMENT", "Mycobrick");
        }
        public static void Postfix()
        {
            Debug.Log("[MycobrickMod] Assets_SubstanceListHookup_Patch Postfix: Registering MycofiberElement substance into Assets.substanceTable.");
            MycofiberElement.RegisterMycofiberSubstance(); // This calls the modified ElementUtil.CreateRegisteredSubstance
            MycobrickElement.RegisterMycobrickSubstance();
        }
    }

    [HarmonyPatch(typeof(Assets), "GetPrefab")]
    public static class MissingAssetDebugPatch
    {
        public static void Prefix(ref Tag tag)
        {
            if (tag == null){
                Debug.Log("[MycobrickMod] Assets.GetPrefab Prefix: tag is null.");
                tag = SimHashes.DirtyWater.CreateTag();
                Debug.Log($"[MycobrickMod] Assets.GetPrefab Prefix: tag set to {tag.ToString()}");
            }
        }
    }
}