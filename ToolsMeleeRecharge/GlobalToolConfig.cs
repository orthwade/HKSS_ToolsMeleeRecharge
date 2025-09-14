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

        private static ConfigEntry<int> StorageModifierPercent;

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
                    "Global charge in percents replenished with one strike.\nIf storage is 10:\n4% == 0.4 charge(3 strikes required for 1 charge)\n10% == 1 charge\n100% == 10 charges",
                    new AcceptableValueRange<int>(1, 20000)
                )
            );

            StorageModifierPercent = config.Bind(
                "00 - Global",
                "StorageModifierPercent",
                100,
                new ConfigDescription(
                    "Modifies size of all storages in percent. 100% = no change, 200% = double size, 50% = half size",
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

        public static int GetStorageModifierPercent() => StorageModifierPercent.Value;

        // public static float GetGlobalDamageMultiplier() => GlobalDamageMultiplier.Value;
    }
}
