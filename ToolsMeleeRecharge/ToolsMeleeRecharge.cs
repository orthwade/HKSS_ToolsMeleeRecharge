using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
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

            ToolLibrary.Init(Config);

            // Initialize config manager
            GlobalToolConfig.Init(Config);

            Logger.LogInfo("Tools Melee Recharge loaded!");

            // Apply Harmony patches
            var harmony = new Harmony("com.orthwade.toolsmeleerecharge");
            harmony.PatchAll();
        }
    }
}
