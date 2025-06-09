using System;
using System.Collections.Generic;
using Klei.AI;
using MycobrickMod.Elements;
using STRINGS;
using TUNING;
using UnityEngine;

namespace MycobrickMod
{
// Token: 0x0200008C RID: 140
    public class MycoweaveLungsuitConfig : IEquipmentConfig
    {

            public const string ID = "Mycoweave_Lungsuit";
            public const string EFFECT_ID = "Mycoweave_Lungsuit_Effect";
            public static ComplexRecipe recipe;

        // Token: 0x060002BD RID: 701 RVA: 0x00015A2C File Offset: 0x00013C2C
        public EquipmentDef CreateEquipmentDef()
        {

            Dictionary<string, float> ingredients = new Dictionary<string, float>();

            float MYCOWEAVE_LUNGSUIT_MASS = 4f;
            ingredients.Add("Mycoweave Strands", MYCOWEAVE_LUNGSUIT_MASS);       // 4 mycoweave strands


            ClothingWearer.ClothingInfo MYCOWEAVE_LUNGSUIT = new ClothingWearer.ClothingInfo(
                _name: STRINGS.EQUIPMENT.PREFABS.MYCOWEAVE_LUNGSUIT.NAME,
                _decor: -10,
                _temperature: 0.005f,       // insulation thickness, default is .0025
                _homeostasisEfficiencyMultiplier: -1.25f);    // default is -1.25, cool vest is 0

            ClothingWearer.ClothingInfo clothingInfo = MYCOWEAVE_LUNGSUIT;

            List<AttributeModifier> modifiers = new List<AttributeModifier>();
            modifiers.Add(new AttributeModifier(
                Db.Get().Attributes.AirConsumptionRate.Id,
                -0.1f,  // -10% oxygen consumption
                EFFECT_ID,
                true,
                false,
                true
                ));


            EquipmentDef def = EquipmentTemplates.CreateEquipmentDef(
                Id: "Mycoweave_Lungsuit",
                Slot: TUNING.EQUIPMENT.CLOTHING.SLOT,
                OutputElement: MycofiberElement.MycofiberSimHash,
                Mass: MYCOWEAVE_LUNGSUIT_MASS,
                Anim: /*placeholder*/TUNING.EQUIPMENT.VESTS.WARM_VEST_ICON0,
                SnapOn: TUNING.EQUIPMENT.VESTS.SNAPON0,
                BuildOverride: TUNING.EQUIPMENT.VESTS.WARM_VEST_ANIM0,
                BuildOverridePriority: 4,
                AttributeModifiers: modifiers,
                SnapOn1: TUNING.EQUIPMENT.VESTS.SNAPON1,
                IsBody: true,
                CollisionShape: EntityTemplates.CollisionShape.RECTANGLE,
                width: 0.75f,
                height: 0.4f,
                additional_tags: null,
                RecipeTechUnlock: null);

            int decor = MYCOWEAVE_LUNGSUIT.decorMod;
            
            Descriptor conductivity_descriptor = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(MYCOWEAVE_LUNGSUIT.conductivityMod)), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(MYCOWEAVE_LUNGSUIT.conductivityMod)), Descriptor.DescriptorType.Effect, false);
            Descriptor decor_descriptor = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, decor), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, decor), Descriptor.DescriptorType.Effect, false);
            def.additionalDescriptors.Add(conductivity_descriptor);

            //Effect.AddModifierDescriptions(null, def.additionalDescriptors, EFFECT_ID, false);
            
            bool flag = decor != 0;
            if (flag)
            {
                def.additionalDescriptors.Add(decor_descriptor);
            }
            def.OnEquipCallBack = delegate(Equippable eq)
            {
                ClothingWearer.ClothingInfo.OnEquipVest(eq, clothingInfo);
                Effects effects = eq.GetComponent<MinionIdentity>()?.GetComponent<Effects>();
                effects?.Add(EFFECT_ID, true);
            };
            def.OnUnequipCallBack = delegate(Equippable eq)
            {
                ClothingWearer.ClothingInfo.OnUnequipVest(eq);
                Effects effects = eq.GetComponent<MinionIdentity>()?.GetComponent<Effects>();
                effects?.Remove(EFFECT_ID);
            };           
            def.RecipeDescription = STRINGS.EQUIPMENT.PREFABS.MYCOWEAVE_LUNGSUIT.RECIPE_DESC;
            return def;
        }

        public static void SetupVest(GameObject go)
        {
            go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
            Equippable eq = go.GetComponent<Equippable>();
            bool flag = eq == null;
            if (flag)
            {
                eq = go.AddComponent<Equippable>();
            }
            eq.SetQuality(global::QualityLevel.Poor);
            go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
        }

        // Token: 0x060002BF RID: 703 RVA: 0x00015BE4 File Offset: 0x00013DE4
        public void DoPostConfigure(GameObject go)
        {
            MycoweaveLungsuitConfig.SetupVest(go);
            KPrefabID prefab_id = go.GetComponent<KPrefabID>();
            prefab_id.AddTag(GameTags.PedestalDisplayable, false);
        }

        public static void RegisterStrings()
        {
            string id = MycoweaveLungsuitConfig.ID.ToUpperInvariant();
            Strings.Add($"STRINGS.EQUIPMENT.PREFABS.{id}.NAME", STRINGS.EQUIPMENT.PREFABS.MYCOWEAVE_LUNGSUIT.NAME);
            Strings.Add($"STRINGS.EQUIPMENT.PREFABS.{id}.DESC", STRINGS.EQUIPMENT.PREFABS.MYCOWEAVE_LUNGSUIT.DESC);
            Strings.Add($"STRINGS.EQUIPMENT.PREFABS.{id}.EFFECT", STRINGS.EQUIPMENT.PREFABS.MYCOWEAVE_LUNGSUIT.EFFECT);
            Strings.Add($"STRINGS.EQUIPMENT.PREFABS.{id}.RECIPE_DESC", STRINGS.EQUIPMENT.PREFABS.MYCOWEAVE_LUNGSUIT.RECIPE_DESC);

        }

            public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        // Token: 0x040001A1 RID: 417

    }
}   