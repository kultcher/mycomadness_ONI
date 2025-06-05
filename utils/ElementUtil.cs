using HarmonyLib;
using STRINGS;
using UnityEngine;
using Klei;
using System; // For Type
using System.Collections.Generic;


namespace MycobrickMod.Utils // Or your chosen namespace for utilities
{
    public static class ElementUtil
    {
        public static void RegisterElementStrings(string elementId, string name, string description)
        {
            string text = elementId.ToUpperInvariant(); // Use InvariantCulture for consistency
            Debug.Log($"[MycobrickMod] ElementUtil.CreateRegisteredStrings: Registering element key {"STRINGS.ELEMENTS." + text + ".NAME"}: {elementId} with name {name} and description {description}");
            Strings.Add(new string[]
            {
                "STRINGS.ELEMENTS." + text + ".NAME",
                UI.FormatAsLink(name, text) // text here should be the elementId for the link
                
            }
);
            Strings.Add(new string[]
            {
                "STRINGS.ELEMENTS." + text + ".DESC",
                description
            });
        }

        public static Substance CreateSubstance(string name, Element.State state, KAnimFile kanim, Material material, Color32 colour)
        {
            return ModUtil.CreateSubstance(name, state, kanim, material, colour, colour, colour);
        }

        public static void AddSubstance(Substance substance)
        {
            Assets.instance.substanceTable.GetList().Add(substance);
        }

        public static Substance CreateRegisteredSubstance(string name, Element.State state, KAnimFile kanim, Material material, Color32 colour)
        {
            Substance substance = CreateSubstance(name, state, kanim, material, colour);
            SimHashUtil.RegisterSimHash(name);
            ElementUtil.AddSubstance(substance); // Adds to Assets.instance.substanceTable
            Traverse.Create(substance).Field("anims").SetValue(new KAnimFile[] { kanim });

            ElementLoader.FindElementByHash(substance.elementID).substance = substance;
            Debug.Log($"[MycobrickMod] ElementUtil.CreateRegisteredSubstance: Created and registered {substance.name} for {name} with animation: {substance.anim}.");
            return substance;
        }

        public static KAnimFile FindAnim(string name)
        {
            KAnimFile kanimFile = Assets.Anims.Find((KAnimFile anim) => anim.name == name);
            if (kanimFile == null)
            {
                Debug.LogError("Failed to find KAnim: " + name);
            }
            return kanimFile;
        }

        public static void UpdateElementPrefabAnim(Tag tag, Substance substance, string defaultSymbol = "object")
        {
            GameObject OrePrefab = Assets.GetPrefab(tag);

            if (OrePrefab != null)
            {
                Debug.Log($"[MycobrickMod] CustomizeDefaultOrePrefabs: Found default ore prefab for '{tag}'. Customizing...");
                KBatchedAnimController kbac = OrePrefab.GetComponent<KBatchedAnimController>();
                if (kbac != null)
                {
                    KAnimFile baseAnim = substance.anim;
                    kbac.TintColour = substance.colour;
                    kbac.HighlightColour = substance.colour;
                    kbac.OverlayColour = substance.colour;
                    Debug.Log($"[MycobrickMod] kbac {kbac}");

                    if (baseAnim != null)
                    {
                        //Debug.Log($"[MycobrickMod] Customized KBAC {kbac} for existing prefab '{tag}'.");

                        ////kbac.AnimFiles = new KAnimFile[] { baseAnim };
                        //kbac.initialAnim = "object"; // Or "ui"
                        //kbac.TintColour = substance.colour;
                        //var symbol = new KAnimHashedString("object");
                        //var tintColor = substance.colour;
                        //Debug.Log($"[MycobrickMod] Attempting to set symbol '{symbol}' and tint color '{tintColor}' for prefab '{tag}'.");
                        //kbac.SetSymbolTint(symbol, tintColor);
                    }
                }
            }
        }

        // Suggested location: patches/CodexPatches.cs or a new file like Patches/CodexLinkerPatches.cs

        
    }
}