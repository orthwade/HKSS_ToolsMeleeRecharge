using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace ToolsMeleeRecharge
{
    internal static class PluginLog
    {
        internal static ManualLogSource Log;
    }

    [BepInPlugin("com.orthwade.toolsmeleerecharge", "Tools Melee Recharge", "1.0.0")]
    public class ToolsMeleeRecharge : BaseUnityPlugin
    {
        private void Awake()
        {
            PluginLog.Log = Logger;
            // Initialize config
            ConfigManager.Init(Config);

            Logger.LogInfo("Tools Melee Recharge loaded!");

            var harmony = new Harmony("com.yourname.limittools");
            harmony.PatchAll();
        }

        // Patch ToolItemManager.GetToolStorageAmount to cap charges
        [HarmonyPatch(typeof(ToolItemManager), nameof(ToolItemManager.GetToolStorageAmount))]
        class LimitToolStorage
        {
            static void Postfix(ToolItem tool, ref int __result)
            {
                if (tool.Type == ToolItemType.Red)
                {
                    var tool_data = ConfigManager.GetToolData(tool.name);
                    if (tool_data.HasValue)
                    {
                        var (_, _, _, maxCharges, _, _) = tool_data.Value;
                        if (maxCharges < 0)
                            maxCharges = ConfigManager.GetGlobalMaxCharges();
                            
                        if (__result > maxCharges)
                            __result = maxCharges; // Limit max charges to tool-specific config
                    }
                }
            }
        }

        private void Update()
        {

        }

        private void TestLogConfig()
        {
           
        }
    }
}
