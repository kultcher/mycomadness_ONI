using HarmonyLib;

using Klei; // For Tag
using System; // For Type
using System.Collections.Generic;

using System.IO;


namespace MycobrickMod
{
    [HarmonyPatch(typeof(Db), "Initialize")]
    public static class Db_Initialize_Patch
    {
        public static void Postfix()
        {
            MycoweaveStrandsConfig.RegisterStrings();
        }
    }

    [HarmonyPatch(typeof(CodexScreen), "ChangeArticle")]
    public static class ChangeArticle_Debuf
    {
        public static void Prefix(string id) // We don't need __result
        {
            Debug.Log($"[MycoCodexCustomizer] ChangeArticle called with id: {id}");
            // Call customization for other elements if you add more
        }
    }

    [HarmonyPatch(typeof(CodexCache), "CollectYAMLEntries")]
    public static class CodexCache_CollectYAMLEntries_Patch
    {
        public static void Postfix(List<CategoryEntry> categories)
        {
            CollectModYAMLEntries(categories);
        }

        private static void CollectModYAMLEntries(List<CategoryEntry> categories)
        {
            // Get all loaded mods
            Debug.Log($"[CodexCache] Getting mods...");


            var modManager = Global.Instance.modManager;
            if (modManager?.mods == null) return;

            Debug.Log($"[CodexCache] Found mod...");


            foreach (var mod in modManager.mods)
            {
                //if (!mod.IsEnabledForActiveDlc()) continue;

                string modCodexPath = Path.Combine(mod.ContentPath, "codex");
                Debug.Log($"[CodexCache] Searching {mod.ContentPath}...");

                if (!Directory.Exists(modCodexPath)) continue;

                Debug.Log($"[CodexCache] Scanning mod codex directory: {modCodexPath}");

                // Collect entries from subdirectories
                foreach (string directory in Directory.GetDirectories(modCodexPath))
                {
                    string categoryName = Path.GetFileNameWithoutExtension(directory);
                    List<CodexEntry> categoryEntries = CollectModEntries(modCodexPath, categoryName);
                    Debug.Log($"[CodexCache] Found codex entries in {directory}: {categoryEntries.Count}");

                    foreach (CodexEntry entry in categoryEntries)
                    {
                        if (entry?.id == null || entry.contentContainers == null || !Game.IsCorrectDlcActiveForCurrentSave(entry))
                            continue;
                        Debug.Log($"[CodexCache] Looking at codex entry: {entry.id}");

                        string formattedId = CodexCache.FormatLinkID(entry.id);
                        if (CodexCache.entries.ContainsKey(formattedId))
                        {
                            var mergeMethod = typeof(CodexCache).GetMethod("MergeEntry",
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                            mergeMethod?.Invoke(null, new object[] { entry.id, entry });
                        }
                        else
                        {
                            CodexCache.AddEntry(entry.id, entry, categories);
                            entry.customContentLength = entry.contentContainers.Count;
                        }
                    }
                }
            }
        }

        private static List<CodexEntry> CollectModEntries(string basePath, string folder)
        {
            List<CodexEntry> assets = new List<CodexEntry>();
            string fullFolderPath = string.IsNullOrEmpty(folder) ? basePath : Path.Combine(basePath, folder);
            
            if (!Directory.Exists(fullFolderPath)) return assets;

            string[] filePaths;
            try
            {
                filePaths = Directory.GetFiles(fullFolderPath, "*.yaml");
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.LogWarning($"[CodexCache] Access denied to {fullFolderPath}: {e}");
                return assets;
            }

            string category = folder.ToUpper();
            
            // Get widget tag mappings using reflection
            var widgetTagMappingsField = typeof(CodexCache).GetField("widgetTagMappings", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var widgetTagMappings = widgetTagMappingsField?.GetValue(null) as List<global::Tuple<string, Type>>;

            foreach (string path in filePaths)
            {
                // Skip sub-entries (they're handled separately)
                if (Path.GetFileName(path).Contains("SubEntry")) continue;

                try
                {
                    // Use reflection to call private YamlParseErrorCB method
                    var yamlErrorMethod = typeof(CodexCache).GetMethod("YamlParseErrorCB", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                    var errorHandler = yamlErrorMethod != null ? 
                        (YamlIO.ErrorHandler)Delegate.CreateDelegate(typeof(YamlIO.ErrorHandler), yamlErrorMethod) : null;

                    CodexEntry asset = YamlIO.LoadFile<CodexEntry>(path, errorHandler, widgetTagMappings);
                    if (asset != null)
                    {
                        asset.category = category;
                        assets.Add(asset);
                        Debug.Log($"[CodexCache] Loaded mod codex entry: {asset.id} from {path}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[CodexCache] Failed to load mod codex entry from {path}: {ex}");
                }
            }

            // Sort entries
            foreach (CodexEntry asset in assets)
            {
                if (string.IsNullOrEmpty(asset.sortString))
                {
                    asset.sortString = Strings.Get(asset.title);
                }
            }
            assets.Sort((x, y) => x.sortString.CompareTo(y.sortString));

            return assets;
        }
    }


}
