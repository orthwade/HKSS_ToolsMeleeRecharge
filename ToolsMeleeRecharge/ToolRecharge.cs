using System;
using BepInEx.Configuration;

namespace ToolsMeleeRecharge
{
    public class ToolRecharge
    {
        public enum ChargeMode
        {
            Vanilla,    // use vanilla game value
            Global,     // use global config value
            Difference  // vanilla + difference
        }

        public enum StrikesMode
        {
            Global,     // use global config value
            Custom      // use custom value
        }

        private readonly bool shouldIgnore = false;
        private readonly int index;
        private readonly string internalName;
        private readonly string displayName;
        private int strikeCounter = 0;

        private readonly ConfigEntry<ChargeMode> chargeMode;
        private readonly ConfigEntry<int> chargesDifference;

        private readonly ConfigEntry<StrikesMode> strikesMode;
        private readonly ConfigEntry<int> customChargePercentPerStrike;

        private readonly ConfigEntry<float> damageMultiplier;

        public ToolRecharge(int index, string internalName, string displayName, ConfigFile config)
        {
            this.index = index;
            this.internalName = internalName;
            this.displayName = displayName;

            // Charges settings
            chargeMode = config.Bind(
                $"{index:D2} - {displayName}",
                "ChargesMode",
                ChargeMode.Vanilla,
                new ConfigDescription("How to determine charges for this tool (Vanilla / Global / Difference)")
            );

            chargesDifference = config.Bind(
                $"{index:D2} - {displayName}",
                "ChargesDifference",
                0,
                new ConfigDescription(
                    "Difference to apply to vanilla charges (only used if mode != Vanilla)",
                    new AcceptableValueRange<int>(-50, 200)
                )
            );

            // Strikes settings
            strikesMode = config.Bind(
                $"{index:D2} - {displayName}",
                "StrikesPerRechargeMode",
                StrikesMode.Global,
                new ConfigDescription("How to determine strikes per recharge for this tool (Global / Custom)")
            );

            customChargePercentPerStrike = config.Bind(
                $"{index:D2} - {displayName}",
                "CustomChargePercentPerStrike",
                5,
                new ConfigDescription(
                    "Charge in percents replenished with one strike (100% == 1 charge) (only used if mode = Custom)",
                    new AcceptableValueRange<int>(1, 999)
                )
            );

            // Optional damage multiplier (not hooked up yet)
            // damageMultiplier = config.Bind(
            //     $"{index:D2} - {displayName}",
            //     "DamageMultiplier",
            //     1f,
            //     new ConfigDescription(
            //         "Damage multiplier for this tool",
            //         new AcceptableValueRange<float>(0.1f, 100f)
            //     )
            // );
        }

        public int GetIndex() => index;
        public bool ShouldIgnore() => shouldIgnore;
        public string GetInternalName() => internalName;
        public string GetDisplayName() => displayName;

        /// <summary>
        /// Resolves max charges for this tool, based on mode.
        /// </summary>
        public int ResolveCharges(int vanillaValue)
        {
            switch (chargeMode.Value)
            {
                case ChargeMode.Vanilla:
                    return vanillaValue;
                case ChargeMode.Global:
                    return GlobalToolConfig.GetGlobalMaxCharges();
                case ChargeMode.Difference:
                    return Math.Max(1, vanillaValue + chargesDifference.Value);
                default:
                    return vanillaValue;
            }
        }

        /// <summary>
        /// Resolves strikes per recharge for this tool, based on mode.
        /// </summary>
        public int ResolveChargePercentPerStrike()
        {
            switch (strikesMode.Value)
            {
                case StrikesMode.Global:
                    return GlobalToolConfig.GetGlobalChargePercentPerStrike();
                case StrikesMode.Custom:
                    return customChargePercentPerStrike.Value;
                default:
                    return GlobalToolConfig.GetGlobalChargePercentPerStrike();
            }
        }

        public float GetDamageMultiplier() => damageMultiplier?.Value ?? 1f;

        public int GetChargePercent() => strikeCounter;
        public void IncrementStrikeCounter(int increment) => strikeCounter += increment;
        public void ResetStrikeCounter() => strikeCounter = 0;
    }

}