using UnityEngine;
using MycobrickMod.Elements; // For MycofiberElement.Id
using MycobrickMod.Utils; // For ModStrings
using TUNING; // For BUILDINGS constants
using System;
using STRINGS;

namespace MycobrickMod.Buildings
{
    public class MycobrickTileConfig : IBuildingConfig
    {
        public const string Id = "MycobrickTile";

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
                id: Id,
                width: 1,
                height: 1,
                anim: "floor_wood_kanim", // Using "floor_basic_kanim" as placeholder, tinting is complex. "floor_mycofiber_kanim" would require new art.
                hitpoints: TUNING.BUILDINGS.HITPOINTS.TIER1, // e.g., 100
                construction_time: 1.5f,
                construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, // 50kg
                construction_materials: new string[] { MycobrickElement.ID },
                melting_point: TUNING.BUILDINGS.MELTING_POINT_KELVIN.TIER1, // e.g., 1600K
                build_location_rule: BuildLocationRule.Tile,
                decor: TUNING.BUILDINGS.DECOR.NONE, // -10 decor
                noise: NOISE_POLLUTION.NONE
            );

            buildingDef.Floodable = false;
            buildingDef.Overheatable = false;
            buildingDef.Entombable = false;
            buildingDef.UseStructureTemperature = false;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.AudioSize = "small";
            buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
            buildingDef.isKAnimTile = true;
            buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_wood");
            buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_wood_place");
            buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
            buildingDef.IsFoundation = true; // Critical for tiles
            buildingDef.TileLayer = ObjectLayer.FoundationTile; // Correct layer for tiles
            buildingDef.ReplacementLayer = ObjectLayer.ReplacementTile;
            buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
            buildingDef.DragBuild = true; // Allow drag building
            buildingDef.ObjectLayer = ObjectLayer.FoundationTile; // Ensure it's on the foundation layer
            buildingDef.AddSearchTerms(SEARCH_TERMS.TILE);

            string[] construction_materials = new string[]
            {
                MycofiberElement.MycofiberSimHash.ToString()
            };


            //buildingDef.Description = ModStrings.BUILDINGS.MYCOFIBERTILE.DESC;
            //buildingDef.Effect = ModStrings.BUILDINGS.MYCOFIBERTILE.EFFECT;
            // For tooltips and UI, ensure strings are registered:

            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag); // Tiles don't require foundation

            // Add SimCellOccupier to allow replacing elements and occupying the cell
            var simCellOccupier = go.AddOrGet<SimCellOccupier>();
            simCellOccupier.doReplaceElement = true; // This makes it replace the natural tile
            simCellOccupier.notifyOnMelt = true;

            simCellOccupier.movementSpeedMultiplier = 1.33f;

            go.AddOrGet<KAnimGridTileVisualizer>(); // Handles visuals for tiles, connections, etc.
            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;


            // // Ensure it's on the correct physics layer for tiles
            // int tileLayer = LayerMask.NameToLayer("Tile");
            // if (tileLayer != -1) // -1 means layer not found
            // {
            //     go.layer = tileLayer;
            // }
            // else
            // {
            //     Debug.LogWarning($"MycoMod: 'Tile' physics layer not found for {Id}. Using default.");
            // }
        }


        public override void DoPostConfigureComplete(GameObject go)
        {
       		GeneratedBuildings.RemoveLoopingSounds(go);
            go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
            go.GetComponent<KPrefabID>().AddTag(GameTags.Organics); // If Mycofiber is considered organic for game logic
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
		    go.AddOrGet<KAnimGridTileVisualizer>();
        }
    }
}