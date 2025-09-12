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

            var maxCharges = toolRecharge.ResolveCharges(__result);

            __result = maxCharges;
        }
    }
}
