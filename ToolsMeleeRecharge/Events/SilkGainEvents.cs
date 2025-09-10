using BepInEx.Logging;

namespace ToolsMeleeRecharge.Events
{
    internal static class SilkGainEvents
    {
        private static ManualLogSource Log => PluginLog.Log;

        /// <summary>
        /// Custom logic executed right after SilkGain would normally be applied.
        /// </summary>
        public static void AfterSilkGain(HitInstance hit)
        {
            Log.LogInfo($"[AfterSilkGain] AttackType={hit.AttackType}, Damage={hit.DamageDealt}");

            // TODO: Hook into recharge logic here
        }
    }
}
