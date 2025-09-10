using HarmonyLib;

namespace ToolsMeleeRecharge.Patches
{
    [HarmonyPatch(typeof(ToolItemManager), nameof(ToolItemManager.GetToolStorageAmount))]
    internal static class ToolItemManager_GetToolStorageAmount_Patch
    {
        private static void Postfix(ToolItem tool, ref int __result)
        {
            if (tool.Type != ToolItemType.Red) return;

            var toolRecharge = ToolLibrary.GetByInternalName(tool.name);
            if (toolRecharge == null) return;

            var maxCharges = toolRecharge.GetMaxCharges();

            if (maxCharges < 0)
                maxCharges = GlobalToolConfig.GetGlobalMaxCharges();

            if (__result > maxCharges)
                __result = maxCharges; // Cap charges
        }
    }
}
