using BepInEx;
using HarmonyLib;
using owd;

namespace ToolsMeleeRecharge
{
    [BepInPlugin("com.orthwade.toolsmeleerecharge", "Tools Melee Recharge", "1.0.6")]
    public class ToolsMeleeRecharge : BaseUnityPlugin
    {
        internal static ToolsMeleeRecharge Instance;
        private void Awake()
        {
            Instance = this;

            BepinexPluginLogger.Init(Config, "ToolsMeleeRecharge");

            GlobalToolConfig.Init(Config);

            ToolLibrary.Init(Config);

            BepinexPluginLogger.LogInfo("Tools Melee Recharge loaded!");

            // Apply Harmony patches
            var harmony = new Harmony("com.orthwade.toolsmeleerecharge");
            harmony.PatchAll();
        }
    }
}
