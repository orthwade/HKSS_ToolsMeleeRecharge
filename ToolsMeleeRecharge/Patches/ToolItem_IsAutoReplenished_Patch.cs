using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine; // for Debug if you prefer Unity's logging, optional

namespace ToolsMeleeRecharge.Patches
{
    [HarmonyPatch(typeof(ToolItem))]
    [HarmonyPatch(nameof(ToolItem.IsAutoReplenished))]
    internal static class ToolItem_IsAutoReplenished_Patch
    {
        // Optional: change to your logger
        private static void LogInfo(string s) => owd.BepinexPluginLogger.LogInfo(s);

        static bool Prefix(ToolItem __instance, ref bool __result)
        {
            if (__instance == null)
                return true; // let original run, it will handle null appropriately

            if (__instance.Type != ToolItemType.Red)
                return true; // only affect red tools, let others run original

            if(!__instance.IsEquipped)
                return true; // only affect equipped tools, let others run original

            // Only affect when benchRestoreMode == OnlyLiquid
                if (GlobalToolConfig.GetBenchRestore() == GlobalToolConfig.BenchRestoreMode.OnlyLiquid)
                {
                    LogInfo($"[ToolItem_IsAutoReplenished_Patch] Prefix: tool={__instance.name}, type={__instance.Type}");
                    // Liquids keep the original logic, others always return false
                    if (__instance is ToolItemStatesLiquid liquidTool)
                    {
                        LogInfo($"[ToolItem_IsAutoReplenished_Patch] Liquid tool {__instance.name} uses original IsAutoReplenished logic.");
                        // let original logic run
                        return true;
                    }
                    else
                    {
                        LogInfo($"[ToolItem_IsAutoReplenished_Patch] Non-liquid tool {__instance.name} is NOT auto-replenished.");
                        __result = false;
                        return false; // skip original
                    }
                }

            // For other modes, allow original method to run
            return true;
        }
    }
}
