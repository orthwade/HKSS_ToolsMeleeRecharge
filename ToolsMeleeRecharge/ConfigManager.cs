using System.Collections.Generic;
using BepInEx.Configuration;

namespace ToolsMeleeRecharge
{
    internal static class ConfigManager
    {
        private static Dictionary<string, Tool> Tools { get; set; }
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
            
            Tools = new Dictionary<string, Tool>();

            // Add each tool: index, internalName, displayName, config, defaultCharges, defaultStrikes, defaultDamage
            int index = 1; // start numbering from 1
            foreach (var kvp in ToolNameMaps.DisplayToInternal)
            {
                AddTool(index, kvp.Value, kvp.Key, config);
                index++;
            }
        }

        private static void AddTool(int index, string internalName, string displayName, ConfigFile config)
        {
            var tool = new Tool(index, internalName, displayName, config);
            Tools[internalName] = tool;
        }

        public static (int, string, string, int, int, float)? GetToolData(string internalName)
        {
            if (Tools.TryGetValue(internalName, out var tool))
            {
                return tool.GetData();
            }
            return null;
        }
        public static int GetGlobalMaxCharges() => GlobalMaxCharges.Value;
        public static int GetGlobalStrikesPerRecharge() => GlobalStrikesPerRecharge.Value;
        public static float GetGlobalDamageMultiplier() => GlobalDamageMultiplier.Value;
    }
}
