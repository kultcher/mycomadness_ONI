using System.Collections.Generic;
using UnityEngine;
using MycobrickMod.Elements;

namespace MycobrickMod
{
    // MycofiberOreConfig.cs
    public class MycofiberOreConfig : IOreConfig
    {
        public const string ID = "MycofiberElement"; // Explicit string ID
        public static readonly Tag MYCOFIBER_ORE_TAG = TagManager.Create(ID); // Create the tag ONCE

        public SimHashes ElementID => MycofiberElement.MycofiberSimHash;

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
            prefab.name = ID;

            return prefab;
        }
    }
}