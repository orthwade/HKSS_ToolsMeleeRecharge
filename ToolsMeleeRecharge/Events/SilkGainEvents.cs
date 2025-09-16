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
        // Ensure we only schedule one deferred update per frame
        private static bool updateScheduled = false;
      
        private static List<ToolItem> GetCurrentEquippedTools()
        {
            if (PlayerData.instance == null)
            {
                owd.BepinexPluginLogger.LogError("[GetCurrentEquippedTools] PlayerData.instance is null!");
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
                owd.BepinexPluginLogger.LogInfo($"[AfterSilkGain] AttackType={hit.AttackType}, Damage={hit.DamageDealt}");

                if (PlayerData.instance == null)
                {
                    owd.BepinexPluginLogger.LogError("[AfterSilkGain] PlayerData.instance is null. Aborting.");
                    return;
                }

                var equippedTools = GetCurrentEquippedTools();
                if (equippedTools == null)
                {
                    owd.BepinexPluginLogger.LogError("[AfterSilkGain] equippedTools == null (unexpected).");
                    return;
                }

                foreach (var item in equippedTools)
                {
                    if (item == null)
                    {
                        owd.BepinexPluginLogger.LogWarning("[AfterSilkGain] skipped null ToolItem in equippedTools.");
                        continue;
                    }

                    if (item.Type != ToolItemType.Red) continue; // only attacking tools

                    var toolRecharge = ToolLibrary.GetByInternalName(item.name);
                    if (toolRecharge == null)
                    {
                        owd.BepinexPluginLogger.LogWarning($"[AfterSilkGain] ToolLibrary.GetByInternalName returned null for '{item.name}'.");
                        continue;
                    }

                    if (PlayerData.instance == null)
                    {
                        owd.BepinexPluginLogger.LogError("[AfterSilkGain] PlayerData.instance became null mid-loop. Aborting.");
                        return;
                    }

                    // get current saved data (this is typically a struct copy — we update & write back)
                    ToolItemsData.Data toolData = PlayerData.instance.GetToolData(item.name);

                    int currentCharges = toolData.AmountLeft;
                    int maxCharges = toolRecharge.ResolveStorage(ToolItemManager.GetToolStorageAmount(item), false);
                    int chargePercentPerStrike = GlobalToolConfig.GetGlobalChargePercentPerStrike();

                    if (currentCharges >= maxCharges)
                    {
                        toolRecharge.ResetChargePercentCounter();
                        continue; // already full
                    }

                    toolRecharge.IncreaseChargePercentCounter(chargePercentPerStrike);

                    int charges = maxCharges * toolRecharge.GetChargePercent() / 100;

                    if (charges > 0)
                    {
                        toolData.AmountLeft = Math.Min(toolData.AmountLeft + charges, maxCharges); // cap to max
                        PlayerData.instance.SetToolData(item.name, toolData);
                        owd.BepinexPluginLogger.LogInfo($"[AfterSilkGain] Recharged 1 charge for {toolRecharge.GetDisplayName()}. New charges: {currentCharges + 1}");

                        // schedule a single deferred update (do not call immediately)
                        ScheduleDeferredToolUpdate();

                        toolRecharge.ResetChargePercentCounter();
                    }
                }
            }
            catch (Exception ex)
            {
                owd.BepinexPluginLogger.LogError($"[AfterSilkGain] Exception: {ex}");
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
                owd.BepinexPluginLogger.LogError($"[ScheduleDeferredToolUpdate] Failed to start coroutine: {ex}");
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
                owd.BepinexPluginLogger.LogError($"[DeferredUpdate] ReportAllBoundAttackToolsUpdated threw: {ex}");
            }
            finally
            {
                updateScheduled = false;
            }
        }
    }
}
