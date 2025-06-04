using System;
using UnityEngine;
using MycobrickMod.Utils; // Assuming your utils are here

namespace MycobrickMod.Elements
{
    public static class MycofiberElement
    {
        public const string ID = "MycofiberElement";
        public static readonly SimHashes MycofiberSimHash = (SimHashes)Hash.SDBMLower(ID);
        public static Tag mycofiber_element_tag = TagManager.Create(ID);
        public static readonly Color32 MYCOFIBER_COLOR = new Color32(255, 255, 0, 255); // A more distinct earthy/fungal color
        public static Substance substance;
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
            Substance temp = Assets.instance.substanceTable.GetSubstance(SimHashes.SolidOxygen);
            Substance mycofiber = ElementUtil.CreateRegisteredSubstance(
              name: ID,
              state: Element.State.Solid,
              kanim: ElementUtil.FindAnim("seed_saltplant_kanim"),
              material: CreateMycofiberMaterial(temp.material),
              colour: MYCOFIBER_COLOR
            );
            substance = mycofiber;
        }
    }
}
