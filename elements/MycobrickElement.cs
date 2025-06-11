using System;
using UnityEngine;
using MycobrickMod.Utils; // Assuming your utils are here

namespace MycobrickMod.Elements
{
    public static class MycobrickElement
    {
        public const string ID = "MycobrickElement";
        public static readonly SimHashes MycobrickSimHash = (SimHashes)Hash.SDBMLower(ID);
        public static readonly Tag TAG = MycobrickSimHash.CreateTag();
        public static Color32 MYCOBRICK_COLOR = new Color32(0, 0, 255, 255); // Your reddish-brown


        static Texture2D TintTextureMycobrickColor(Texture sourceTexture, string name)
        {
            Texture2D newTexture = TexUtil.DuplicateTexture(sourceTexture as Texture2D);
            var pixels = newTexture.GetPixels32();
            for (int i = 0; i < pixels.Length; ++i)
            {
                var gray = ((Color)pixels[i]).grayscale * 1.5f;
                pixels[i] = (Color)MYCOBRICK_COLOR * gray;
            }
            newTexture.SetPixels32(pixels);
            newTexture.Apply();
            newTexture.name = name;
            return newTexture;
        }

        static Material CreateMycobrickMaterial(Material source)
        {
            var mycobrickMaterial = new Material(source);

            Texture2D newTexture = TintTextureMycobrickColor(mycobrickMaterial.mainTexture, "mycobrick");

            mycobrickMaterial.mainTexture = newTexture;
            mycobrickMaterial.name = "matMycobrick";

            return mycobrickMaterial;
        }

        public static void RegisterMycobrickSubstance()
        {
            Material material = Assets.instance.substanceTable.GetSubstance(SimHashes.SolidOxygen).material;

            ElementUtil.CreateRegisteredSubstance(
              name: ID,
              state: Element.State.Solid,
              kanim: ElementUtil.FindAnim("cinnabar_kanim"),
              material: CreateMycobrickMaterial(material),
              colour: MYCOBRICK_COLOR
            );
        }
     
    }
}