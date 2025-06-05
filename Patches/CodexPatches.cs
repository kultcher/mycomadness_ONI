using HarmonyLib;
using STRINGS;
using UnityEngine;
using Klei; // For Tag
using System; // For Type
using System.Collections.Generic;
using MycobrickMod.Elements;
using UnityEngine.UI; // For MycofiberElement and MycobrickElement

namespace MycobrickMod
{
    [HarmonyPatch(typeof(CodexEntryGenerator_Elements), "GenerateEntries")]
    public static class CodexEntryGenerator_Elements_Postfix_MycoCustomization_Patch
    {
        public static void Postfix() // We don't need __result
        {
            CustomizeMycofiberEntry();
            // Call customization for other elements if you add more
        }

        private static void CustomizeMycofiberEntry()
        {
            string simHashId = MycofiberElement.MycofiberSimHash.ToString();
            CodexEntry entry = CodexCache.FindEntry("MYCOFIBERELEMENT");

            Debug.Log($"[MycoCodexCustomizer] Set iconColor for Mycofiber (ID: {simHashId}) to ");

            if (entry != null)
            {
                // 1. Set Icon Color
                var icon_widget = entry.contentContainers[1]?.content[0] as CodexImage;
                if (icon_widget != null)
                {
                    icon_widget.color = Elements.MycofiberElement.MYCOFIBER_COLOR;
                }
                Debug.Log($"[MycoCodexCustomizer] Set iconColor for Mycofiber (ID: {simHashId}) to ");

                // List<ContentContainer> list = entry.contentContainers;
                // foreach (ContentContainer item in list)
                // {

                //     Debug.Log("Content Container: " + item.ToString() + " Content Count: " + item.content.Count + item.GetType().ToString());
                //     List<ICodexWidget> list2 = item.content;
                //     foreach (ICodexWidget item2 in list2)
                //     {
                //         if (item2 != null)
                //         {
                //             Debug.Log("Widget: " + item2.ToString() + " Widget Type: " + item2.GetType().ToString());
                //             if (item2 is CodexImage codexImage)
                //             {
                //                 codexImage.color = Elements.MycofiberElement.MYCOFIBER_COLOR;
                //                 Debug.Log($"[MycoCodexCustomizer] Set iconColor for Mycofiber (ID: {simHashId}) to {codexImage.color}");
                //             }
                //         }
                //     }
                // }
                // Debug.Log($"[MycoCodexCustomizer] Set iconColor for Mycofiber (ID: {simHashId}) to ");
                //<link="MYCOFIBERELEMENT">Mycofiber</link>



                // 3. (Optional) Modify other properties or add/remove content containers
                // For example, if you wanted to ensure a specific main image with the right color:
                // if (entry.contentContainers != null && entry.contentContainers.Count > 0 &&
                //     entry.contentContainers[0]?.content != null && entry.contentContainers[0].content.Count > 0 &&
                //     entry.contentContainers[0].content[0] is CodexImage imageWidget)
                // {
                //     if (imageWidget.sprite == entry.icon) // If the first image is the main icon
                //     {
                //         imageWidget.color = Elements.MycofiberElement.MYCOFIBER_COLOR;
                //         Debug.Log($"[MycoCodexCustomizer] Set color for first CodexImage widget for Mycofiber to {imageWidget.color}");
                //     }
                // }
                // else // Or, if no image container exists or it's not what you want, create/replace it:
                // {
                //     // This part is more involved: you'd need to create a new ContentContainer 
                //     // with a new CodexImage configured with your sprite and color,
                //     // and then decide whether to prepend it, replace an existing one, or append it.
                //     // For just color, entry.iconColor should be sufficient if the UI uses it.
                // }
            }
            else
            {
                Debug.LogWarning($"[MycoCodexCustomizer] Could not find CodexEntry for Mycofiber (ID: {simHashId}) to customize.");
            }
        }
    }
}
