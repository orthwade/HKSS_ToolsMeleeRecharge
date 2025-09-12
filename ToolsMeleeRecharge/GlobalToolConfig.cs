using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using BepInEx.Configuration;

namespace ToolsMeleeRecharge
{
    internal static class GlobalToolConfig
    {
        private static ConfigEntry<int> GlobalMaxCharges;
        private static ConfigEntry<int> GlobalChargePercentPerStrike;
        private static ConfigEntry<float> GlobalDamageMultiplier;

        public static void Init(ConfigFile config)
        {
            GlobalMaxCharges = config.Bind(
                "00 - Global",
                "MaxCharges",
                1,
                new ConfigDescription(
                    "Global maximum charges for all tools. Notice that specific tools use Difference, not absolute values.",
                    new AcceptableValueRange<int>(1, 999)
                )
            );

            GlobalChargePercentPerStrike = config.Bind(
                "00 - Global",
                "ChargePercentPerStrike",
                5,
                new ConfigDescription(
                    "Global charge in percents replenished with one strike (if storage is 10, 10% == 1 charge, 100% == 10 charges)",
                    new AcceptableValueRange<int>(1, 20000)
                )
            );

            // GlobalDamageMultiplier = config.Bind(
            //     "00 - Global",
            //     "DamageMultiplier",
            //     1f,
            //     new ConfigDescription(
            //         "Global damage multiplier for all tools",
            //         new AcceptableValueRange<float>(0.1f, 100f)
            //     )
            // );

        }

        public static int GetGlobalMaxCharges() => GlobalMaxCharges.Value;
        public static int GetGlobalChargePercentPerStrike() => GlobalChargePercentPerStrike.Value;
        // public static float GetGlobalDamageMultiplier() => GlobalDamageMultiplier.Value;
    }
}
