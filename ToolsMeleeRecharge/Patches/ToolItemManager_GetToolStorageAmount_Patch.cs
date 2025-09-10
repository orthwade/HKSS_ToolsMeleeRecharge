using HarmonyLib;

namespace ToolsMeleeRecharge.Patches
{
    [HarmonyPatch(typeof(ToolItemManager), nameof(ToolItemManager.GetToolStorageAmount))]
    internal static class ToolItemManager_GetToolStorageAmount_Patch
    {
        private static void Postfix(ToolItem tool, ref int __result)
        {
            if (tool.Type != ToolItemType.Red) return;

            var toolData = ConfigManager.GetToolData(tool.name);
            if (!toolData.HasValue) return;

            var (_, _, _, maxCharges, _, _) = toolData.Value;

            if (maxCharges < 0)
                maxCharges = ConfigManager.GetGlobalMaxCharges();

            if (__result > maxCharges)
                __result = maxCharges; // Cap charges
        }
    }
}
