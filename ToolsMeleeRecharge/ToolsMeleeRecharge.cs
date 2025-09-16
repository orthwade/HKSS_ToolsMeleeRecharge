using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using owd;
using ToolsMeleeRecharge.Events;

namespace ToolsMeleeRecharge
{
    [BepInPlugin("com.orthwade.toolsmeleerecharge", "Tools Melee Recharge", "1.0.6")]
    public class ToolsMeleeRecharge : BaseUnityPlugin
    {
        private static ManualLogSource logger;
        internal static ToolsMeleeRecharge Instance;
        private void Awake()
        {
            Instance = this;

            BepinexPluginLogger.Init(Config, "ToolsMeleeRecharge");

            logger = BepinexPluginLogger.GetLogger();
            GlobalToolConfig.Init(Config);

            ToolLibrary.Init(Config);

            logger.LogInfo("Tools Melee Recharge loaded!");

            // Apply Harmony patches
            var harmony = new Harmony("com.orthwade.toolsmeleerecharge");
            harmony.PatchAll();
        }
    }
}
