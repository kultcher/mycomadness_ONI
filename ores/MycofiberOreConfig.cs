using System.Collections.Generic;
using UnityEngine;
using MycobrickMod.Elements;

namespace MycobrickMod
{
    // MycofiberOreConfig.cs
    public class MycofiberOreConfig : IOreConfig
    {
        public const string ID = "MycofiberOre"; // Explicit string ID
        public static readonly Tag MYCOFIBER_ORE_TAG = TagManager.Create(ID); // Create the tag ONCE

        public SimHashes ElementID => MycofiberElement.MycofiberSimHash;

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
            KPrefabID kpid = prefab.GetComponent<KPrefabID>();

            Tag tagFromSolidOreEntity = kpid.PrefabTag; // Read the initial primary tag
            Debug.Log($"[MycobrickMod] MycofiberOreConfig: Tag from CreateSolidOreEntity: '{tagFromSolidOreEntity.Name}' (Hash: {tagFromSolidOreEntity.GetHash()})");

            kpid.PrefabTag = MYCOFIBER_ORE_TAG; // Change the primary tag
            Debug.Log($"[MycobrickMod] MycofiberOreConfig: Tag changed?: '{tagFromSolidOreEntity.Name}' (Hash: {tagFromSolidOreEntity.GetHash()})");

  

            prefab.name = ID;

            return prefab;
            }
        }
    }   