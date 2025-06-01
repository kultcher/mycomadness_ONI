using HarmonyLib;
using STRINGS;
using UnityEngine;

namespace MycobrickMod.Utils // Or your chosen namespace for utilities
{
    public static class ElementUtil
    {
        public static void RegisterElementStrings(string elementId, string name, string description)
        {
            string text = elementId.ToUpperInvariant(); // Use InvariantCulture for consistency
            Strings.Add(new string[]
            {
                "STRINGS.ELEMENTS." + text + ".NAME",
                UI.FormatAsLink(name, text) // text here should be the elementId for the link
            });
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
            AddSubstance(substance); // Adds to Assets.instance.substanceTable

            // REMOVED THE PART THAT TRIES TO FIND ELEMENT AND LINK IT HERE
            Debug.Log($"[MycobrickMod] ElementUtil.CreateRegisteredSubstance: Created and registered substance for {name} into Assets.substanceTable. Linking will occur in ElementLoader.Load.");
            return substance;
        }
    }
}