// using System;
// using UnityEngine;
// using MycobrickMod.Elements;

// // Token: 0x0200025E RID: 606
// namespace MycobrickMod
// {
//     public class MycofiberOreConfig2 : IOreConfig
//     {
//         // Token: 0x1700001E RID: 30
//         // (get) Token: 0x06000C43 RID: 3139 RVA: 0x0004ED78 File Offset: 0x0004CF78
//         public SimHashes ElementID
//         {
//             get
//             {
//                 return MycofiberElement.MycofiberSimHash;
//             }
//         }
//         public const string ID = "MycofiberOre";

//         // Token: 0x06000C44 RID: 3140 RVA: 0x0004ED80 File Offset: 0x0004CF80
//         public GameObject CreatePrefab()
//         {
//             var prefab = EntityTemplates.CreateLooseEntity(
//                 id: "MycofiberOre",
//                 name: "test",
//                 desc: "test",
//                 mass: 1f,
//                 unitMass: false,
//                 anim: Assets.GetAnim("fungusplant_kanim"),
//                 initialAnim: "object",
//                 sceneLayer: Grid.SceneLayer.Ore,
//                 collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
//                 width: 0.77f,
//                 height: 0.48f,
//                 isPickupable: true);
//             // KPrefabID prefID = prefab.GetComponent<KPrefabID>();
//             // prefID.prefabInitFn += this.OnInit;
//             // prefID.prefabSpawnFn += this.OnSpawn;
//             // prefID.RemoveTag(GameTags.HideFromSpawnTool);
//             return prefab;
//         }
//         public static readonly Tag TAG = TagManager.Create("MycofiberOre");

//         // // Token: 0x06000C45 RID: 3141 RVA: 0x0004EDDC File Offset: 0x0004CFDC
//         // public void OnInit(GameObject inst)
//         // {
//         //     PrimaryElement PE = inst.GetComponent<PrimaryElement>();
//         //     PE.SetElement(this.ElementID, true);
//         //     Element element = PE.Element;
//         // }

//         // // Token: 0x06000C46 RID: 3142 RVA: 0x0004EE08 File Offset: 0x0004D008
//         // public void OnSpawn(GameObject inst)
//         // {
//         //     PrimaryElement PE = inst.GetComponent<PrimaryElement>();
//         //     PE.SetElement(this.ElementID, true);
//         // }
//     }
// }