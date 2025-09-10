using BepInEx.Logging;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection;

namespace ToolsMeleeRecharge.Events
{
    internal static class SilkGainEvents
    {
        private static ManualLogSource Log => PluginLog.Log;

        private static List<ToolItem> GetCurrentEquippedTools()
        {
            // Get the tools equipped for the current crest
            return ToolItemManager.GetEquippedToolsForCrest(PlayerData.instance.CurrentCrestID) 
                ?? new List<ToolItem>();
        }

        /// <summary>
        /// Custom logic executed right after SilkGain would normally be applied.
        /// </summary>
        public static void AfterSilkGain(HitInstance hit)
        {
            Log.LogInfo($"[AfterSilkGain] AttackType={hit.AttackType}, Damage={hit.DamageDealt}");

            var equippedTools = GetCurrentEquippedTools();
            foreach (var tool in equippedTools)
            {
                if (tool.Type != ToolItemType.Red) continue; // Only consider attacking tools
                PluginLog.Log.LogInfo($"Equipped Tool: {tool.name}, Type={tool.Type}");
                // TODO: recharge logic here
            }
        }
    }
}
