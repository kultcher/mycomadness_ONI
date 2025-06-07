using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

namespace MycobrickMod
{
    public class MycoweaveStrandsConfig : IEntityConfig
    {

        public static string ID = "MycoweaveStrands";
        public static readonly Tag TAG = TagManager.Create("MYCOWEAVESTRANDS");
        private AttributeModifier decorModifier = new AttributeModifier(
            attribute_id: "Decor",
            value: 0.1f,
            description: MycobrickMod.STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MYCOWEAVESTRANDS.NAME,
            is_multiplier: true,
            uiOnly: false,
            is_readonly: true);

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
            MycoweaveStrandsConfig.ID,
            MycobrickMod.STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MYCOWEAVESTRANDS.NAME,
            MycobrickMod.STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MYCOWEAVESTRANDS.DESC,
            1f,
            true,
            Assets.GetAnim("swampreedwool_kanim"), // Temporarily using reed fiber anim
            "object",
            Grid.SceneLayer.BuildingBack,
            EntityTemplates.CollisionShape.RECTANGLE,
            0.8f,
            0.45f,
            true,
            SORTORDER.BUILDINGELEMENTS + 1, // Using same sort as BasicFabric
            SimHashes.Creature,
            new List<Tag>
            {
                GameTags.IndustrialIngredient,
                GameTags.BuildingFiber
            });

            prefab.AddOrGet<EntitySplitter>();
            PrefabAttributeModifiers modifiers = prefab.AddOrGet<PrefabAttributeModifiers>();
            modifiers.AddAttributeDescriptor(this.decorModifier);
            return prefab;
        }

        public void OnPrefabInit(GameObject inst){}

        public void OnSpawn(GameObject inst){}

        public static void RegisterStrings()
        {
                // Register MycoweaveStrands in the global strings
                string id = MycoweaveStrandsConfig.ID.ToUpperInvariant();

                // These strings are what the Codex system uses to auto-generate entries
                Strings.Add($"STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.{id}.NAME", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MYCOWEAVESTRANDS.NAME);
                Strings.Add($"STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.{id}.DESC", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MYCOWEAVESTRANDS.DESC);
                Strings.Add($"STRINGS.CODEX.{id}.TITLE", STRINGS.CODEX.MYCOWEAVESTRANDS.TITLE);
                Strings.Add($"STRINGS.CODEX.{id}.SUBTITLE", STRINGS.CODEX.MYCOWEAVESTRANDS.SUBTITLE);
                Strings.Add($"STRINGS.CODEX.{id}.BODY.CONTAINER1", STRINGS.CODEX.MYCOWEAVESTRANDS.BODY.CONTAINER1);
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }
    }


    
}
