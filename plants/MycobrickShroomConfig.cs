using System.Collections.Generic;
using Epic.OnlineServices.Presence;
using TUNING;
using UnityEngine;
using MycobrickMod.Elements;

namespace MycobrickMod
{
    public class MycobrickShroomConfig : IEntityConfig, IHasDlcRestrictions
    {
        public const string ID = "MycobrickShroom";
        public GameObject CreatePrefab()
        {
            string id = ID;
            string name = STRINGS.CREATURES.SPECIES.MYCOBRICKSHROOM.NAME;
            string desc = STRINGS.CREATURES.SPECIES.MYCOBRICKSHROOM.DESC;

            float mass = 1f;
            EffectorValues decor = DECOR.PENALTY.TIER1;
            GameObject go = EntityTemplates.CreatePlacedEntity(
                id, name, desc, mass,
                Assets.GetAnim("fungusplant_kanim"), "idle_empty",
                Grid.SceneLayer.BuildingFront, 1, 3,
                decor, NOISE_POLLUTION.NONE, defaultTemperature: 293f
            );

        EntityTemplates.ExtendEntityToBasicPlant(
            go,
            temperature_lethal_low: 233.15f,
            temperature_warning_low: 273.15f,
            temperature_warning_high: 313.15f,
            temperature_lethal_high: 343.15f,
            safe_elements: new SimHashes[] { SimHashes.CarbonDioxide, SimHashes.Oxygen },
            pressure_sensitive: true,
            pressure_lethal_low: 0.025f,
            pressure_warning_low: 0.1f,
            crop_id: "MycofiberElement",
            can_drown: false,
            can_tinker: false,
            require_solid_tile: true,
            should_grow_old: true, // leaving default, but can set explicitly if you want
            max_age: 1200f, // 1200 seconds = 2 cycles
            min_radiation: 0f,
            max_radiation: 2200f,
            baseTraitId: "MycobrickShroom",
            baseTraitName: STRINGS.CREATURES.SPECIES.MYCOBRICKSHROOM.NAME
        );

            // Fertilizer consumption
            PlantElementAbsorber.ConsumeInfo info = new PlantElementAbsorber.ConsumeInfo
            {
                tag = SimHashes.Dirt.CreateTag(),
                massConsumptionRate = 0.0008333334f // 10g/s
            };
            PlantElementAbsorber.ConsumeInfo[] fertilizers = new PlantElementAbsorber.ConsumeInfo[] { info };

            EntityTemplates.ExtendPlantToFertilizable(go, fertilizers);

            go.AddOrGet<StandardCropPlant>();
            go.AddOrGet<SeedProducer>();            

            GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(
                plant: go,
                dlcRestrictions: this,
                productionType: SeedProducer.ProductionType.Harvest,
                id: "MycobrickShroomSeed",
                name: STRINGS.CREATURES.SPECIES.SEEDS.MYCOBRICKSHROOMSEED.NAME,
                desc: STRINGS.CREATURES.SPECIES.SEEDS.MYCOBRICKSHROOMSEED.DESC,
                anim: Assets.GetAnim("seed_fungusplant_kanim"), // This is Duskcap's seed anim
                initialAnim: "object",
                numberOfSeeds: 1,
                additionalTags: new List<Tag> { GameTags.CropSeed },
                domesticatedDescription: STRINGS.CREATURES.SPECIES.MYCOBRICKSHROOM.DOMESTICATEDDESC
            );

            EntityTemplates.CreateAndRegisterPreviewForPlant(
                seed,
                "MycobrickShroom_preview",
                Assets.GetAnim("fungusplant_kanim"),
                "place",
                width: 3,
                height: 1
            );


            Debug.Log("[MycobrickMod] Inside CreatePrefab");

            return go;
        }


        public string[] GetRequiredDlcIds()
        {
            return new string[0];
        }

        public string[] GetForbiddenDlcIds()
        {
            return new string[0];
        }

        public string[] GetDlcIds() // obsolete but required by IEntityConfig
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
        {
            var anim = inst.GetComponent<KBatchedAnimController>();
            if (anim != null)
            {
                KAnimHashedString symbol = new KAnimHashedString("swap_crop01"); // or "drop", etc.
                Color tintColor = new Color32(210, 255, 77, 255); // light greenish for Mycofiber
                anim.SetSymbolTint(symbol, tintColor);
                anim.SetSymbolScale(symbol, 1.8f);
            }      
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}