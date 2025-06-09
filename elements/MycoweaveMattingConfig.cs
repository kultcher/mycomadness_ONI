using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

namespace MycobrickMod
{
    public class MycoweaveMattingConfig : IEntityConfig
    {

        public static string ID = "MycoweaveMatting";
        public static readonly Tag TAG = TagManager.Create("MYCOWEAVEMATTING");

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
            MycoweaveMattingConfig.ID,
            MycobrickMod.STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MYCOWEAVEMATTING.NAME,
            MycobrickMod.STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MYCOWEAVEMATTING.DESC,
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
            return prefab;
        }

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }

        public static void RegisterStrings()
        {
            string id = MycoweaveMattingConfig.ID.ToUpperInvariant();

            // These strings are what the Codex system uses to auto-generate entries
            Strings.Add($"STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.{id}.NAME", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MYCOWEAVEMATTING.NAME);
            Strings.Add($"STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.{id}.DESC", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MYCOWEAVEMATTING.DESC);
            Strings.Add($"STRINGS.CODEX.{id}.TITLE", STRINGS.CODEX.MYCOWEAVEMATTING.TITLE);
            Strings.Add($"STRINGS.CODEX.{id}.SUBTITLE", STRINGS.CODEX.MYCOWEAVEMATTING.SUBTITLE);
            Strings.Add($"STRINGS.CODEX.{id}.BODY.CONTAINER1", STRINGS.CODEX.MYCOWEAVEMATTING.BODY.CONTAINER1);
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }
    }



}
