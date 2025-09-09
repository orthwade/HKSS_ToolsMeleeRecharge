using BepInEx;
using HarmonyLib;

[BepInPlugin("com.yourname.limittools", "Limit Tools Charges", "1.0.0")]
public class LimitToolsPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Create and apply Harmony patches
        var harmony = new Harmony("com.yourname.limittools");
        harmony.PatchAll();
    }
}

// Patch ToolItemManager.GetToolStorageAmount to cap charges
[HarmonyPatch(typeof(ToolItemManager), nameof(ToolItemManager.GetToolStorageAmount))]
class LimitToolStorage
{
    static void Postfix(ToolItem tool, ref int __result)
    {
        if (__result > 2)
            __result = 2; // Limit max charges to 2
    }
}
