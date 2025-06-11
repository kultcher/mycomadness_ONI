using System;
using UnityEngine;
using MycobrickMod.Utils; // Assuming your utils are here

namespace MycobrickMod.Elements
{
    public static class MycofiberElement
    {
        public const string ID = "MycofiberElement";
        public static readonly SimHashes MycofiberSimHash = (SimHashes)Hash.SDBMLower(ID);
        public static readonly Tag TAG = MycofiberSimHash.CreateTag();

        public static readonly Color32 MYCOFIBER_COLOR = new Color32(255, 0, 0, 255); // A more distinct earthy/fungal color
        //public static readonly Color32 MYCOFIBER_COLOR = new Color32(180, 200, 60, 255); // A more distinct earthy/fungal color
        //public static Tag MYCOFIBER_TAG = TagManager.Create(ID);
        //public static Substance substance = new Substance();

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
            Material material = Assets.instance.substanceTable.GetSubstance(SimHashes.SolidMethane).material;
            Substance mycofiber = ElementUtil.CreateRegisteredSubstance(
              name: ID,
              state: Element.State.Solid,
              kanim: ElementUtil.FindAnim("solid_methane_kanim"),
              material: CreateMycofiberMaterial(material),
              colour: MYCOFIBER_COLOR
            );
            Debug.Log("Registered MycofiberElement with ID: " + ID);
        }
    }
}
