using HarmonyLib;

namespace ToolsMeleeRecharge.Patches
{
    [HarmonyPatch(typeof(ToolItemManager))]
    [HarmonyPatch("TryReplenishTools")]
    internal static class ToolItemManager_TryReplenishTools_Patch
    {
        private static bool Prefix(bool doReplenish, ToolItemManager.ReplenishMethod method)
        {
            if (method != ToolItemManager.ReplenishMethod.QuickCraft)
                return GlobalToolConfig.GetBenchRestore();

                return true; // allow original method to run
        }
    }
}
