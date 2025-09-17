using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine; // for Debug if you prefer Unity's logging, optional

namespace ToolsMeleeRecharge.Patches
{
    [HarmonyPatch(typeof(ToolItemManager), nameof(ToolItemManager.TryReplenishTools),
    new[] { typeof(bool), typeof(ToolItemManager.ReplenishMethod) })]
    internal static class ToolItemManager_TryReplenishTools_Patch
    {
        // Optional: change to your logger
        private static void LogInfo(string s) => owd.BepinexPluginLogger.LogInfo(s);

        private static bool Prefix(bool doReplenish, ToolItemManager.ReplenishMethod method)
        {
            bool run_original = true;

            if (method != ToolItemManager.ReplenishMethod.QuickCraft)
                run_original = GlobalToolConfig.GetBenchRestore() != GlobalToolConfig.BenchRestoreMode.Disabled;

            LogInfo($"[ToolItemManager_TryReplenishTools_Patch] Prefix: method={method}, benchRestoreMode={GlobalToolConfig.GetBenchRestore()}, run_original={run_original}");

            // Return true to run original method, false to skip original entirely.
            return run_original;
        }
    }
}
