using HarmonyLib;
using BepInEx.Logging;

namespace ToolsMeleeRecharge.Patches
{
    [HarmonyPatch(typeof(ToolItemManager), nameof(ToolItemManager.GetToolStorageAmount))]
    internal static class ToolItemManager_GetToolStorageAmount_Patch
    {
        private static void Postfix(ToolItem tool, ref int __result)
        {
            // 🚫 Defensive guard: ignore null tools
            if (tool == null)
                return;

            // 🚫 Ignore non-red tools
            if (tool.Type != ToolItemType.Red)
                return;

            // Defensive: make sure tool has a name
            if (string.IsNullOrEmpty(tool.name))
            {
                owd.BepinexPluginLogger.LogWarning("[ToolItemManager_GetToolStorageAmount_Patch] Skipping unnamed red tool (tool.name was null/empty).");
                return;
            }

            // Try to find tool definition in your library
            var toolRecharge = ToolLibrary.GetByInternalName(tool.name);
            if (toolRecharge == null)
            {
                // Unsupported tool → leave vanilla result intact
                owd.BepinexPluginLogger.LogInfo($"[ToolItemManager_GetToolStorageAmount_Patch] Skipping unsupported red tool \"{tool.name}\" (vanilla storage={__result}).");
                return;
            }

            // Defensive: safeguard SavedData usage
            try
            {
                var maxCharges = toolRecharge.ResolveStorage(__result, true);
                owd.BepinexPluginLogger.LogInfo($"[ToolItemManager_GetToolStorageAmount_Patch] Overriding {tool.name}: vanilla={__result}, patched={maxCharges}");
                __result = maxCharges;
            }
            catch (System.Exception ex)
            {
                owd.BepinexPluginLogger.LogError($"[ToolItemManager_GetToolStorageAmount_Patch] ResolveStorage failed for {tool.name}: {ex}");
                // Fallback: leave __result unchanged
            }
        }
    }
}
