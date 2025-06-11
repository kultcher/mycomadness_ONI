//

using System;
using System.Collections.Generic;
using UnityEngine;
using Klei.AI;
using TUNING;
using Epic.OnlineServices.Presence;

namespace MycobrickMod
{
    public class TarFunnelConfig : IEntityConfig, IHasDlcRestrictions
    {
        public const string ID = "TarFunnel";
        public const string SEED_ID = "TarFunnelSeed";

        public static CellOffset OUTPUT_CONDUIT_CELL_OFFSET = new CellOffset(0, 0);

        public const string MANUAL_HARVEST_PRE_ANIM_NAME = "syrup_harvest_trunk_pre"; // assuming these are the tapping animations, leaving them for now
        public const string MANUAL_HARVEST_LOOP_ANIM_NAME = "syrup_harvest_trunk_loop";
        public const string MANUAL_HARVEST_PST_ANIM_NAME = "syrup_harvest_trunk_pst";
        public const string MANUAL_HARVEST_INTERRUPT_ANIM_NAME = "syrup_harvest_trunk_loop";

        private static readonly List<Storage.StoredItemModifier> storedItemModifiers = new List<Storage.StoredItemModifier>
        {
            Storage.StoredItemModifier.Hide,
            Storage.StoredItemModifier.Preserve,
            Storage.StoredItemModifier.Insulate,
            Storage.StoredItemModifier.Seal
        };

        public GameObject CreatePrefab()      
        {
            string id = ID;
            string name = STRINGS.CREATURES.SPECIES.TARFUNNEL.NAME;
            string desc = STRINGS.CREATURES.SPECIES.TARFUNNEL.DESC;

            float mass = 1f;
            EffectorValues decor = DECOR.PENALTY.TIER1;
            GameObject go = EntityTemplates.CreatePlacedEntity(
                id, name, desc, mass,
                Assets.GetAnim("tarfunnel_kanim"), "idle_empty",
                Grid.SceneLayer.BuildingFront, 1, 2,
                decor, NOISE_POLLUTION.NONE, defaultTemperature: 373.15f
            );

            EntityTemplates.ExtendEntityToBasicPlant
            (
                go,
                temperature_lethal_low: TUNING.PLANTS.TARFUNNEL.MIN_TEMP,
                temperature_warning_low: TUNING.PLANTS.TARFUNNEL.MIN_TEMP + 25f,
                temperature_warning_high: TUNING.PLANTS.TARFUNNEL.MAX_TEMP - 25f,
                temperature_lethal_high: TUNING.PLANTS.TARFUNNEL.MAX_TEMP,
                safe_elements: TUNING.PLANTS.TARFUNNEL.SAFE_ATMOSPHERES,
                pressure_sensitive: true,
                pressure_lethal_low: 0.025f,
                pressure_warning_low: 0.1f,
                crop_id: null,
                can_drown: true,
                can_tinker: false,
                require_solid_tile: true,
                should_grow_old: false, // leaving default, but can set explicitly if you want
                max_age: 2400f,
                min_radiation: 0f,
                max_radiation: 2200f,
                baseTraitId: "TarFunnelTrait",
                baseTraitName: STRINGS.CREATURES.SPECIES.TARFUNNEL.NAME
            );

    		Modifiers component2 = go.GetComponent<Modifiers>();
            if (go.GetComponent<Traits>() == null)
                {
                    go.AddOrGet<Traits>();
                    component2.initialTraits.Add("TarFunnelTrait");
                }

            Debug.Log("[MycobrickMod] Adding crop to " + go.name);
            Crop.CropVal cropval = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == SimHashes.Petroleum.CreateTag());
            Klei.AI.Modifier modifier = Db.Get().traits.Get(component2.initialTraits[0]);

            component2.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
            modifier.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, .75f, STRINGS.CREATURES.SPECIES.TARFUNNEL.NAME, false, false, true)); // *** set growth time here
            go.AddOrGet<Crop>().Configure(cropval);

            KPrefabID component3 = go.GetComponent<KPrefabID>();
            GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component3.PrefabID().ToString());

            Growing growing = go.AddOrGet<Growing>();
            growing.shouldGrowOld = false;
            growing.maxAge = 2400f;
            go.AddOrGet<HarvestDesignatable>();
            go.AddOrGet<LoopingSounds>();

            GameObject plant = go;
            SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;   // may need something else here since this is not harvested traditionally
            string name2 = STRINGS.CREATURES.SPECIES.SEEDS.TARFUNNELSEED.NAME;
            string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.TARFUNNELSEED.DESC;
            KAnimFile anim = Assets.GetAnim("seed_fungusplant_kanim");
            string initialAnim = "object";
            int numberOfSeeds = 1;
            List<Tag> list = new List<Tag>();
            list.Add(GameTags.CropSeed);

            SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
            string domesticatedDescription = STRINGS.CREATURES.SPECIES.TARFUNNEL.DOMESTICATEDDESC;
            GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, SEED_ID, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 1, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false);
            EntityTemplates.ExtendPlantToIrrigated(go, new PlantElementAbsorber.ConsumeInfo[]
            {
                new PlantElementAbsorber.ConsumeInfo
                {
                    tag = SimHashes.CrudeOil.CreateTag(),
                    massConsumptionRate = 0.003333334f
                }
            });
            EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "TarFunnel_preview", Assets.GetAnim("tarfunnel_kanim"), "place", 1, 2);
            SoundEventVolumeCache.instance.AddVolume("fungusplant_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
            SoundEventVolumeCache.instance.AddVolume("fungusplant_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);

            // DirectlyEdiblePlant_StorageElement directlyEdiblePlant_StorageElement = go.AddOrGet<DirectlyEdiblePlant_StorageElement>();
            // directlyEdiblePlant_StorageElement.tagToConsume = SimHashes.Petroleum.CreateTag();      // not sure if this should be crude oil?
            // directlyEdiblePlant_StorageElement.rateProducedPerCycle = TUNING.PLANTS.TARFUNNEL.PETROLEUM_OUTPUT_RATE * 600;
            // directlyEdiblePlant_StorageElement.storageCapacity = TUNING.PLANTS.TARFUNNEL.STORAGE_CAPACITY;
            // directlyEdiblePlant_StorageElement.edibleCellOffsets = new CellOffset[]     // not sure on these, leaving them for now
            // {
            //     new CellOffset(-1, 0),
            //     new CellOffset(1, 0),
            //     new CellOffset(-1, 1),
            //     new CellOffset(1, 1)
            // };

            // internal storage

            Storage storage = go.AddOrGet<Storage>();
            storage.allowItemRemoval = false;
            storage.showInUI = true;
            storage.capacityKg = TUNING.PLANTS.TARFUNNEL.STORAGE_CAPACITY;
            storage.SetDefaultStoredItemModifiers(TarFunnelConfig.storedItemModifiers);

            ElementEmitter elementEmitter = go.AddOrGet<ElementEmitter>();
            ElementConverter.OutputElement steamOutput = new ElementConverter.OutputElement(TUNING.PLANTS.TARFUNNEL.STEAM_OUTPUT_RATE, SimHashes.Steam, TUNING.PLANTS.TARFUNNEL.STEAM_OUTPUT_TEMP, false, true, 0f, 0f, 1f, byte.MaxValue, 0, true);
            //steamOutput.outputElementOffset.y = 1f;
            elementEmitter.outputElement = steamOutput;
            elementEmitter.maxPressure = 3000f;
			elementEmitter.emissionFrequency = 100f;
			elementEmitter.outputElement.massGenerationRate = TUNING.PLANTS.TARFUNNEL.STEAM_OUTPUT_RATE * 100f;
            elementEmitter.SetEmitting(true);

//             ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
//             elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
//             {
//                 new ElementConverter.ConsumedElement(SimHashes.CrudeOil.CreateTag(), TUNING.PLANTS.TARFUNNEL.CRUDE_OIL_INPUT_RATE, true)
//             };
//             elementConverter.outputElements = new ElementConverter.OutputElement[]
//             {
//                 new ElementConverter.OutputElement(TUNING.PLANTS.TARFUNNEL.PETROLEUM_OUTPUT_RATE, SimHashes.Petroleum, TUNING.PLANTS.TARFUNNEL.PETROLEUM_OUTPUT_TEMP, false, true, 0f, 1f, 1f, byte.MaxValue, 0, true),
//                 new ElementConverter.OutputElement(TUNING.PLANTS.TARFUNNEL.STEAM_OUTPUT_RATE, SimHashes.Steam, TUNING.PLANTS.TARFUNNEL.STEAM_OUTPUT_TEMP, false, false, 0f, 1f, 1f, byte.MaxValue, 0, true)
//             };

            // conduit connection
            ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
            conduitDispenser.noBuildingOutputCellOffset = TarFunnelConfig.OUTPUT_CONDUIT_CELL_OFFSET;
            conduitDispenser.conduitType = ConduitType.Liquid;
            conduitDispenser.alwaysDispense = true;
            conduitDispenser.SetOnState(false);
            go.AddOrGet<TarFunnelHarvestWorkable>();
            UnstableEntombDefense.Def def = go.AddOrGetDef<UnstableEntombDefense.Def>();
            def.defaultAnimName = "shake_trunk";
            def.Cooldown = 5f;
           
            TarFunnelPlant.Def def3 = go.AddOrGetDef<TarFunnelPlant.Def>();
            def3.OptimalProductionDuration = 600f;      // not sure how this would convert
            return go;
        }

        public void OnPrefabInit(GameObject prefab){}

        public void OnSpawn(GameObject inst)
        {
            EntityCellVisualizer entityCellVisualizer = inst.AddOrGet<EntityCellVisualizer>();
            entityCellVisualizer.AddPort(EntityCellVisualizer.Ports.LiquidOut, TarFunnelConfig.OUTPUT_CONDUIT_CELL_OFFSET, entityCellVisualizer.Resources.liquidIOColours.output.connected);
        }

        public string[] GetRequiredDlcIds() { return new string[0];}
        public string[] GetForbiddenDlcIds() { return new string[0];}

        public string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }
    }
}