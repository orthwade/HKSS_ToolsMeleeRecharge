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
            foreach (var item in equippedTools)
            {
                if (item.Type != ToolItemType.Red) continue; // Only consider attacking tools
                PluginLog.Log.LogInfo($"Equipped Tool: {item.name}, Type={item.Type}");
                // TODO: recharge logic here
                var toolRecharge = ToolLibrary.GetByInternalName(item.name);
                if (toolRecharge == null) continue;

                toolRecharge.IncrementStrikeCounter();
                if (toolRecharge.GetStrikeCounter() >= toolRecharge.GetStrikesPerRecharge())
                {
                    // Recharge one charge
                    ToolItemsData.Data toolData = PlayerData.instance.GetToolData(item.name);

                    int currentCharges = toolData.AmountLeft;
                    int maxCharges = toolRecharge.GetMaxCharges();
                    if (maxCharges < 0)
                        maxCharges = GlobalToolConfig.GetGlobalMaxCharges();

                    if (currentCharges < maxCharges)
                    {
                        toolData.AmountLeft++;
                        PlayerData.instance.SetToolData(item.name, toolData);
                        ToolItemManager.SetToolStorageAmount(item, currentCharges + 1);
                        Log.LogInfo($"Recharged 1 charge for {toolRecharge.GetDisplayName()}. New charges: {currentCharges + 1}");
                    }
                    toolRecharge.ResetStrikeCounter();
                }
            }
        }
    }
}
