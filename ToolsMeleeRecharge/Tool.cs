using BepInEx.Configuration;
public class Tool
{
    private readonly bool shouldIgnore = false;
    private readonly int index;
    private readonly string internalName;
    private readonly string displayName;
    private int strikeCounter = 0;

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

    public int GetIndex() => index;
    public bool ShouldIgnore() => shouldIgnore;
    public string GetInternalName() => internalName;
    public string GetDisplayName() => displayName;
    public int GetMaxCharges() => maxCharges?.Value ?? -1;
    public int GetStrikesPerRecharge() => strikesPerRecharge?.Value ?? -1;
    public float GetDamageMultiplier() => damageMultiplier?.Value ?? -1f;

    public int GetStrikeCounter() => strikeCounter;
    public void IncrementStrikeCounter() => strikeCounter++;
    public void ResetStrikeCounter() => strikeCounter = 0;
}
