using HarmonyLib;

namespace ToolsMeleeRecharge.Patches
{
    [HarmonyPatch(typeof(ToolItemManager), nameof(ToolItemManager.GetToolStorageAmount))]
    internal static class ToolItemManager_GetToolStorageAmount_Patch
    {
        private static void Postfix(ToolItem tool, ref int __result)
        {
            if (tool == null)
                return;
            if (tool.Type != ToolItemType.Red)
                return;

            var toolRecharge = ToolLibrary.GetByInternalName(tool.name);

            if (toolRecharge == null)
            {
                // 🚫 Not one of your supported tools → leave vanilla result untouched
                UnityEngine.Debug.Log($"[RechargePatch] Skipping unsupported red tool {tool.name} (vanilla storage={__result})");
                return;
            }

            // ✅ Only override if you recognize the tool
            var maxCharges = toolRecharge.ResolveStorage(__result, true);
            __result = maxCharges;
        }
    }
}
