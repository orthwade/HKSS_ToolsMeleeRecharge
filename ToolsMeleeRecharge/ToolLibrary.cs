using System.Collections.Generic;
using BepInEx.Configuration;

static public class ToolLibrary
{
    private static readonly Dictionary<string, string> DisplayToInternal = new Dictionary<string, string>
    {
        { "Straight Pin", "Straight Pin" },
        { "Threefold Pin", "Tri Pin" },
        { "Sting Shard", "Sting Shard" },
        { "Curveclaw", "Curve Claws" },
        { "Longpin", "Harpoon" },
        { "Tacks", "Tack" },
        { "Curvesickle", "Curve Claws Upgraded" },
        { "Throwing Ring", "Shakra Ring" },
        { "Pimpillo", "Pimpilo" },
        { "Silkshot (Twelfth Architect.)", "WebShot Architect" },
        { "Silkshot (Forge Daughter)", "WebShot Forge" },
        { "Conchcutter", "Conch Drill" },
        { "Silkshot", "WebShot Weaver" },
        { "Delvers Drill", "Screw Attack" },
        { "Cogwork Wheel", "Cogwork Saw" },
        { "Snare Setter", "Silk Snare" },
        { "Flintslate", "Flintstone" },
        { "Cogfly", "Cogwork Flier" },
        // { "Needle Phial", "Extractor" },
        { "Flea Brew", "Flea Brew" },
        { "Plasmium Phial", "Lifeblood Syringe" },
        { "Voltvessels", "Lightning Rod" },
    };
    private static Dictionary<string, ToolRecharge> ToolsByInternalName { get; set; }
    private static Dictionary<string, ToolRecharge> ToolsByDisplayName { get; set; }

    private static Dictionary<int, ToolRecharge> ToolsByIndex { get; set; }

    public static void Init(ConfigFile config)
    {
        ToolsByInternalName = new Dictionary<string, ToolRecharge>();
        ToolsByDisplayName = new Dictionary<string, ToolRecharge>();
        ToolsByIndex = new Dictionary<int, ToolRecharge>();

        // Add each tool: index, internalName, displayName, config, defaultCharges, defaultStrikes, defaultDamage
        int index = 1; // start numbering from 1
        foreach (var kvp in DisplayToInternal)
        {
            AddTool(index, kvp.Value, kvp.Key, config);
            index++;
        }
    }

    private static void AddTool(int index, string internalName, string displayName, ConfigFile config)
    {
        var tool = new ToolRecharge(index, internalName, displayName, config);
        ToolsByInternalName[internalName] = tool;
        ToolsByDisplayName[displayName] = tool;
        ToolsByIndex[index] = tool;
    }

    public static ToolRecharge GetByInternalName(string internalName)
    {
        ToolsByInternalName.TryGetValue(internalName, out var tool);
        return tool;
    }
    public static ToolRecharge GetByDisplayName(string displayName)
    {
        ToolsByDisplayName.TryGetValue(displayName, out var tool);
        return tool;
    }
    public static ToolRecharge GetByIndex(int index)
    {
        ToolsByIndex.TryGetValue(index, out var tool);
        return tool;
    }
}