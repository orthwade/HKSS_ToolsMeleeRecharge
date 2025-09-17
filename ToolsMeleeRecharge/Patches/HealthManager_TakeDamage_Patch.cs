using HarmonyLib;
using ToolsMeleeRecharge.Events;

namespace ToolsMeleeRecharge.Patches
{
    [HarmonyPatch(typeof(HealthManager))]
    [HarmonyPatch("TakeDamage")]
    internal static class HealthManager_TakeDamage_Patch
    {
        private static void Postfix(HealthManager __instance, HitInstance hitInstance)
        {
            var enemyType = (HealthManager.EnemyTypes)AccessTools
                .Field(typeof(HealthManager), "enemyType")
                .GetValue(__instance);

            bool zeroDamage = hitInstance.DamageDealt <= 0 &&
                              hitInstance.HitEffectsType !=
                              EnemyHitEffectsProfile.EffectsTypes.LagHit;

            if (hitInstance.AttackType == AttackTypes.Nail &&
                enemyType != HealthManager.EnemyTypes.Shade &&
                enemyType != HealthManager.EnemyTypes.Armoured &&
                !__instance.DoNotGiveSilk &&
                !zeroDamage)
            {
                SilkGainEvents.AfterSilkGain(hitInstance);
            }
        }
    }
}
