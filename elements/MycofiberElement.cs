using System;
using UnityEngine;
using MycobrickMod.Utils; // Assuming your utils are here

namespace MycobrickMod.Elements
{
    public static class MycofiberElement
    {
        public const string ID = "MycofiberElement";
        public static readonly SimHashes MycofiberSimHash = (SimHashes)Hash.SDBMLower(ID);
        //public static readonly Color32 MYCOFIBER_COLOR = new Color32(180, 150, 110, 255); // A more distinct earthy/fungal color
        public static readonly Color32 MYCOFIBER_COLOR = new Color32(0, 0, 0, 0); // A more distinct earthy/fungal color

        // public static Substance RegisterMycofiberSubstance()
        // {
        //     // Register UI strings for the element
        //     ElementUtil.RegisterElementStrings(
        //         ID,
        //         "Mycofiber", // User-facing name
        //         "A tough, fibrous material harvested from Mycobrick Shrooms. It's surprisingly versatile, though not very durable on its own." // User-facing description
        //     );

        //     // Create and register the substance (visuals)
        //     // For placeholder material, let's try to tint wood to our color
        //     Material woodMaterial = Assets.instance.substanceTable.GetSubstance(SimHashes.WoodLog).material;
        //     Material mycofiberMaterial = new Material(woodMaterial); // Create a new material instance
        //     // A simple tint (more advanced tinting like in DIO is better but this is a start)
        //     // mycofiberMaterial.color = MYCOFIBER_COLOR; // This might not work as expected for all shaders
        //     // For now, we'll just use wood's material directly as a placeholder if tinting is complex
        //     // Or, even simpler, just use an existing material that looks somewhat okay.
        //     // Let's use Dirt's material as a placeholder for now, as it's earthy.
        //     Material placeholderMaterial = Assets.instance.substanceTable.GetSubstance(SimHashes.Dirt).material;


        //     Substance mycofiberSubstance = ElementUtil.CreateRegisteredSubstance(
        //         name: ID,
        //         state: Element.State.Solid,
        //         kanim: Assets.GetAnim("wood_log_kanim"), // Placeholder anim for the item on ground
        //         material: placeholderMaterial,
        //         colour: MYCOFIBER_COLOR
        //     );

        //     return mycofiberSubstance;
        // }

        static Texture2D TintTextureMycofiberColor(Texture sourceTexture, string name)
        {
            Texture2D newTexture = TexUtil.DuplicateTexture(sourceTexture as Texture2D);
            var pixels = newTexture.GetPixels32();
            for (int i = 0; i < pixels.Length; ++i)
            {
                var gray = ((Color)pixels[i]).grayscale * 1.5f;
                pixels[i] = (Color)MYCOFIBER_COLOR * gray;
            }
            newTexture.SetPixels32(pixels);
            newTexture.Apply();
            newTexture.name = name;
            return newTexture;
        }

        static Material CreateMycofiberMaterial(Material source)
        {
            var mycofiberMaterial = new Material(source);

            Texture2D newTexture = TintTextureMycofiberColor(mycofiberMaterial.mainTexture, "mycofiber");

            mycofiberMaterial.mainTexture = newTexture;
            mycofiberMaterial.name = "matMycofiber";

            return mycofiberMaterial;
        }

        public static void RegisterMycofiberSubstance()
        {
            Substance mycofiber = Assets.instance.substanceTable.GetSubstance(SimHashes.SolidOxygen);

            ElementUtil.CreateRegisteredSubstance(
              name: ID,
              state: Element.State.Solid,
              kanim: ElementUtil.FindAnim("gas_tank_kanim"),
              material: CreateMycofiberMaterial(mycofiber.material),
              colour: MYCOFIBER_COLOR
            );

         }
    }
}
