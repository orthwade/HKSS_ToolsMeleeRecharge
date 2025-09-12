using BepInEx.Logging;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using System;
using ToolsMeleeRecharge; // Ensure this imports your plugin class namespace
using UnityEngine;

namespace ToolsMeleeRecharge.Events
{
    internal static class SilkGainEvents
    {
        private static ManualLogSource Log => PluginLog.Log;

        // Ensure we only schedule one deferred update per frame
        private static bool updateScheduled = false;

        private static List<ToolItem> GetCurrentEquippedTools()
        {
            if (PlayerData.instance == null)
            {
                Log.LogError("[GetCurrentEquippedTools] PlayerData.instance is null!");
                return new List<ToolItem>();
            }

            var crestId = PlayerData.instance.CurrentCrestID ?? string.Empty;
            var list = ToolItemManager.GetEquippedToolsForCrest(crestId) 
                    ?? new List<ToolItem>();

            // Remove any null entries up front
            list.RemoveAll(item => item == null);
            return list;
        }

        public static void AfterSilkGain(HitInstance hit)
        {
            try
            {
                Log.LogInfo($"[AfterSilkGain] AttackType={hit.AttackType}, Damage={hit.DamageDealt}");

                if (PlayerData.instance == null)
                {
                    Log.LogError("[AfterSilkGain] PlayerData.instance is null. Aborting.");
                    return;
                }

                var equippedTools = GetCurrentEquippedTools();
                if (equippedTools == null)
                {
                    Log.LogError("[AfterSilkGain] equippedTools == null (unexpected).");
                    return;
                }

                foreach (var item in equippedTools)
                {
                    if (item == null)
                    {
                        Log.LogWarning("[AfterSilkGain] skipped null ToolItem in equippedTools.");
                        continue;
                    }

                    if (item.Type != ToolItemType.Red) continue; // only attacking tools

                    var toolRecharge = ToolLibrary.GetByInternalName(item.name);
                    if (toolRecharge == null)
                    {
                        Log.LogWarning($"[AfterSilkGain] ToolLibrary.GetByInternalName returned null for '{item.name}'.");
                        continue;
                    }

                    if (PlayerData.instance == null)
                    {
                        Log.LogError("[AfterSilkGain] PlayerData.instance became null mid-loop. Aborting.");
                        return;
                    }

                    // get current saved data (this is typically a struct copy — we update & write back)
                    ToolItemsData.Data toolData = PlayerData.instance.GetToolData(item.name);

                    int currentCharges = toolData.AmountLeft;
                    int maxCharges = toolRecharge.GetMaxCharges();
                    int strikesPerRecharge = toolRecharge.GetStrikesPerRecharge();

                    if (strikesPerRecharge < 0)
                        strikesPerRecharge = GlobalToolConfig.GetGlobalStrikesPerRecharge();

                    if (maxCharges < 0)
                        maxCharges = GlobalToolConfig.GetGlobalMaxCharges();

                    if (currentCharges >= maxCharges)
                    {
                        toolRecharge.ResetStrikeCounter();
                        continue; // already full
                    }

                    toolRecharge.IncrementStrikeCounter();

                    if (toolRecharge.GetStrikeCounter() >= strikesPerRecharge)
                    {
                        toolData.AmountLeft++;
                        PlayerData.instance.SetToolData(item.name, toolData);
                        Log.LogInfo($"[AfterSilkGain] Recharged 1 charge for {toolRecharge.GetDisplayName()}. New charges: {currentCharges + 1}");

                        // schedule a single deferred update (do not call immediately)
                        ScheduleDeferredToolUpdate();

                        toolRecharge.ResetStrikeCounter();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"[AfterSilkGain] Exception: {ex}");
            }
        }

        private static void ScheduleDeferredToolUpdate()
        {
            if (updateScheduled) return;
            updateScheduled = true;

            // Use the plugin instance to start coroutine
            try
            {
                ToolsMeleeRecharge.Instance.StartCoroutine(DeferredUpdate());
            }
            catch (Exception ex)
            {
                Log.LogError($"[ScheduleDeferredToolUpdate] Failed to start coroutine: {ex}");
                updateScheduled = false;
            }
        }

        // Wait for physics to finish then one frame so pogo logic should have run
        private static IEnumerator DeferredUpdate()
        {
            // Wait until after the next physics step (damage occurs in fixed updates),
            // then wait one frame to be extra safe.
            yield return new WaitForFixedUpdate();
            yield return null;

            try
            {
                ToolItemManager.ReportAllBoundAttackToolsUpdated();
            }
            catch (Exception ex)
            {
                Log.LogError($"[DeferredUpdate] ReportAllBoundAttackToolsUpdated threw: {ex}");
            }
            finally
            {
                updateScheduled = false;
            }
        }
    }
}
