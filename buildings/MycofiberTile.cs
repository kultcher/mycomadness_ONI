using UnityEngine;
using MycobrickMod.Elements; // For MycofiberElement.Id
using MycobrickMod.Utils; // For ModStrings
using TUNING; // For BUILDINGS constants
using System;
using STRINGS;

namespace MycobrickMod.Buildings
{
    public class MycofiberTileConfig : IBuildingConfig
    {
        public const string Id = "MycofiberTile";

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
                construction_materials: new string[] { MycofiberElement.ID },
                melting_point: TUNING.BUILDINGS.MELTING_POINT_KELVIN.TIER1, // e.g., 1600K
                build_location_rule: BuildLocationRule.Tile,
                decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER0, // Example: -5 decor
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
            // Set the building name and description using ModStrings
            // These are typically set on the GameObject after it's created, via strings.Add
            // but BuildingDef has these fields too.
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
            simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.PENALTY_1; // 10% movement penalty
            simCellOccupier.notifyOnMelt = true;

            go.AddOrGet<KAnimGridTileVisualizer>(); // Handles visuals for tiles, connections, etc.

            // Ensure it's on the correct physics layer for tiles
            int tileLayer = LayerMask.NameToLayer("Tile");
            if (tileLayer != -1) // -1 means layer not found
            {
                go.layer = tileLayer;
            }
            else
            {
                Debug.LogWarning($"MycoMod: 'Tile' physics layer not found for {Id}. Using default.");
            }
        }


        public override void DoPostConfigureComplete(GameObject go)
        {
            // Add to game tags
            go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles);
            // If it's made of something specific that needs tagging (e.g. GameTags.Metal for metal tiles)
            go.GetComponent<KPrefabID>().AddTag(GameTags.Organics); // If Mycofiber is considered organic for game logic

            // No specific logic needed here for a basic tile usually.
            // `ConfigureBuildingTemplate` handles most of it.
            // BuildingTemplates.DoPostConfigure(go); // This is often called by the base or game
        }

        // Optional: If you need to do something when the building is planned (ghost placed)
        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            // go.AddOrGet<TileVisualizer>(); // Visualizer might also be needed for the ghost
        }
    }
}