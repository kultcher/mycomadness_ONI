using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine; // For Debug.Log

namespace MycobrickMod.Utils
{
    public static class SimHashUtil
    {
        public static Dictionary<SimHashes, string> SimHashNameLookup = new Dictionary<SimHashes, string>();
        public static readonly Dictionary<string, object> ReverseSimHashNameLookup = new Dictionary<string, object>();

        public static void RegisterSimHash(string name)
        {
            SimHashes simHash = (SimHashes)Hash.SDBMLower(name);
            if (!SimHashNameLookup.ContainsKey(simHash))
            {
                SimHashNameLookup.Add(simHash, name);
            }
            else
            {
                Debug.LogWarning($"[MycobrickMod] SimHashUtil.RegisterSimHash: SimHash for '{name}' already registered in SimHashNameLookup.");
            }

            if (!ReverseSimHashNameLookup.ContainsKey(name))
            {
                ReverseSimHashNameLookup.Add(name, simHash);
            }
            else
            {
                Debug.LogWarning($"[MycobrickMod] SimHashUtil.RegisterSimHash: Name '{name}' already registered in ReverseSimHashNameLookup.");
            }
        }
    }
    // ... rest of the file (Harmony patches for Enum.ToString/Parse)
}