using System.Collections.Generic;
using BepInEx.Configuration;

namespace ToolsMeleeRecharge
{
    internal static class GlobalToolConfig
    {
        private static ConfigEntry<int> GlobalMaxCharges;
        private static ConfigEntry<int> GlobalStrikesPerRecharge;
        private static ConfigEntry<float> GlobalDamageMultiplier;

        public static void Init(ConfigFile config)
        {
            GlobalMaxCharges = config.Bind(
                "00 - Global",
                "MaxCharges",
                1,
                new ConfigDescription(
                    "Global maximum charges for all tools",
                    new AcceptableValueRange<int>(1, 999)
                )
            );

            GlobalStrikesPerRecharge = config.Bind(
                "00 - Global",
                "StrikesPerRecharge",
                1,
                new ConfigDescription(
                    "Global strikes required to replenish one charge for all tools",
                    new AcceptableValueRange<int>(1, 999)
                )
            );

            GlobalDamageMultiplier = config.Bind(
                "00 - Global",
                "DamageMultiplier",
                1f,
                new ConfigDescription(
                    "Global damage multiplier for all tools",
                    new AcceptableValueRange<float>(0.1f, 100f)
                )
            );

        }

        public static int GetGlobalMaxCharges() => GlobalMaxCharges.Value;
        public static int GetGlobalStrikesPerRecharge() => GlobalStrikesPerRecharge.Value;
        public static float GetGlobalDamageMultiplier() => GlobalDamageMultiplier.Value;
    }
}
