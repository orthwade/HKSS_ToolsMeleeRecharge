using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ToolsMeleeRecharge
{
    [BepInPlugin("com.orthwade.toolsmeleerecharge", "Tools Melee Recharge", "1.0.0")]
    public class ToolsMeleeRecharge : BaseUnityPlugin
    {
        internal static ToolsMeleeRecharge Instance;
        private void Awake()
        {
            Instance = this;

            global::ToolsMeleeRecharge.Logger.Init();

            ToolLibrary.Init(Config);

            // Initialize config manager
            GlobalToolConfig.Init(Config);

            global::ToolsMeleeRecharge.Logger.LogInfo("Tools Melee Recharge loaded!");

            // Apply Harmony patches
            var harmony = new Harmony("com.orthwade.toolsmeleerecharge");
            harmony.PatchAll();
        }
    }
}
