using BepInEx.Configuration;
public class Tool
{
    private readonly int index;
    private readonly string internalName;
    private readonly string displayName;

    private readonly ConfigEntry<int> maxCharges;
    private readonly ConfigEntry<int> strikesPerRecharge;
    private readonly ConfigEntry<float> damageMultiplier;

    public Tool(int index, string internalName, string displayName, ConfigFile config)
    {
        this.index = index;
        this.internalName = internalName;
        this.displayName = displayName;

        maxCharges = config.Bind(
            $"{index:D2} - {displayName}",
            "MaxCharges",
            -1,
            new ConfigDescription(
                "Maximum charges for this tool (-1 uses global)",
                new AcceptableValueRange<int>(-1, 999)
            )
        );

        strikesPerRecharge = config.Bind(
            $"{index:D2} - {displayName}",
            "StrikesPerRecharge",
            -1,
            new ConfigDescription(
                "Strikes required to replenish one charge (-1 uses global)",
                new AcceptableValueRange<int>(-1, 999)
            )
        );

        damageMultiplier = config.Bind(
            $"{index:D2} - {displayName}",
            "DamageMultiplier",
            -1f,
            new ConfigDescription(
                "Damage multiplier for this tool (-1 uses global)",
                new AcceptableValueRange<float>(-1f, 100f)
            )
        );
    }


    public (int, string, string, int, int, float) GetData()
    {
        return (
            index,
            internalName,
            displayName,
            maxCharges?.Value ?? -1,
            strikesPerRecharge?.Value ?? -1,
            damageMultiplier?.Value ?? -1f
        );
    }
}
