using System.Collections.Generic;

public static class ToolNameMaps
{
    // 1. Display Name → Internal Name
    public static readonly Dictionary<string, string> DisplayToInternal = new Dictionary<string, string>
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
        { "Needle Phial", "Extractor" },
        { "Flea Brew", "Flea Brew" },
        { "Plasmium Phial", "Lifeblood Syringe" },
        { "Voltvessels", "Lightning Rod" },
    };

    // 2. Internal Name → Display Name
    public static readonly Dictionary<string, string> InternalToDisplay = new Dictionary<string, string>();

    // Static constructor to populate reversed dictionary
    static ToolNameMaps()
    {
        foreach (var kvp in DisplayToInternal)
        {
            // If multiple display names point to the same internal name,
            // only the first will be kept unless you handle duplicates manually
            if (!InternalToDisplay.ContainsKey(kvp.Value))
                InternalToDisplay[kvp.Value] = kvp.Key;
        }
    }
}
