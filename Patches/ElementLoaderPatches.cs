using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // For Path.Combine
using Klei; // For YamlIO

using Klei.AI;
using MycobrickMod.Elements;



namespace MycobrickMod.Patches
{
    [HarmonyPatch(typeof(LegacyModMain), "ConfigElements")]
    public static class ConfigElements_Patch
    {

        public static void Postfix()
        {
            //Element mycofiber = ElementLoader.FindElementByHash(MycofiberElement.MycofiberSimHash);

            //=: Giving Mycofiber Decor and Temperature modifications :====================================================
            //AttributeModifier mycofiberDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -10f, null, false, false, true);
            //AttributeModifier mycofiberTempModifier = new AttributeModifier(Db.Get().BuildingAttributes.OverheatTemperature.Id, -30f, mycofiber.name);
            //mycofiber.attributeModifiers.Add(mycofiberDecorModifier);
 
        }

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        public static class ElementLoader_CollectElementsFromYAML_Patch
        {
            public static void Postfix(ref List<ElementLoader.ElementEntry> __result) // __result is the return value of the original method
            {
                Debug.Log("[MycobrickMod] ElementLoader_CollectElementsFromYAML_Patch Postfix: Modifying collected elements.");

                // --- Step 1: Construct the path to your elements.yaml ---
                string modPath = null;
                foreach (KMod.Mod mod in Global.Instance.modManager.mods)
                {
                    if (mod.staticID == "MycobrickMod")
                    {
                        modPath = mod.ContentPath; //KMod.Mod.path should be the root directory of your mod
                        break;
                    }
                }

                if (string.IsNullOrEmpty(modPath))
                {
                    Debug.LogWarning("[MycobrickMod] Could not find mod path. Cannot load custom elements.yaml.");
                    return;
                }

                string customElementsFilePath = Path.Combine(modPath, "data", "elements.yaml");

                // --- Step 2: Load and parse your elements.yaml ---
                if (File.Exists(customElementsFilePath))
                {
                    ListPool<YamlIO.Error, ElementLoader>.PooledList errors = ListPool<YamlIO.Error, ElementLoader>.Allocate();
                    ElementLoader.ElementEntryCollection customEntries = null;
                    try
                    {
                        customEntries = YamlIO.LoadFile<ElementLoader.ElementEntryCollection>(customElementsFilePath, delegate (YamlIO.Error error, bool force_log_as_warning)
                        {
                            errors.Add(error);
                            Debug.LogErrorFormat("[MycobrickMod] YAML Parse Error in {0}: {1}", customElementsFilePath, error.message);
                        }, null); // Added the third 'parent_path' argument as null, which is often how it's called.
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogErrorFormat("[MycobrickMod] Exception while loading YAML file {0}: {1}", customElementsFilePath, e.ToString());
                    }


                    if (customEntries != null && customEntries.elements != null)
                    {
                        if (__result == null) // Should not happen if original method ran, but good to check
                        {
                            __result = new List<ElementLoader.ElementEntry>();
                        }
                        __result.AddRange(customEntries.elements);
                        Debug.Log($"[MycobrickMod] Successfully added {customEntries.elements.Length} custom element entries from: {customElementsFilePath}");
                    }
                    else if (errors.Count > 0)
                    {
                        Debug.LogErrorFormat("[MycobrickMod] Failed to parse custom elements file {0} due to {1} YAML errors.", customElementsFilePath, errors.Count);
                    }
                    else
                    {
                        Debug.LogWarningFormat("[MycobrickMod] Custom elements file {0} was loaded but contained no elements or was malformed.", customElementsFilePath);
                    }
                    errors.Recycle();
                }
                else
                {
                    Debug.LogWarning($"[MycobrickMod] Custom elements file not found at: {customElementsFilePath}");
                }
            }
        }
    }
}