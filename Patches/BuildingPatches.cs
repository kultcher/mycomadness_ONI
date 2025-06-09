using HarmonyLib;
using System.IO; // For Path.Combine
using Klei; // For YamlIO
using UnityEngine;
using MycobrickMod.Buildings;

namespace MycobrickMod
{
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class ConfigureBuildings
    {
        private static void Prefix()
        {
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MycofiberTileConfig.Id.ToUpperInvariant()}.NAME", MycobrickMod.STRINGS.BUILDINGS.PREFABS.MYCOFIBERTILE.NAME);
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MycofiberTileConfig.Id.ToUpperInvariant()}.DESC", MycobrickMod.STRINGS.BUILDINGS.PREFABS.MYCOFIBERTILE.DESC);
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MycofiberTileConfig.Id.ToUpperInvariant()}.EFFECT", MycobrickMod.STRINGS.BUILDINGS.PREFABS.MYCOFIBERTILE.EFFECT);
            ModUtil.AddBuildingToPlanScreen("Base", MycofiberTileConfig.Id);
        }
    }
}