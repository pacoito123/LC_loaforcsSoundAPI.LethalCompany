using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Scrap;

[SoundAPICondition("LethalCompany:item:name")]
[SoundAPICondition("LethalCompany:scrap:name")]
public class ItemNameCondition : MultipleCondition<Item, ItemContext> {
    protected override string ValidateWarnMessage => $"Value field for a MoonNameCondition in SoundPack '{Pack.Name}' is empty or missing!";

    /// <inheritdoc/>
    public override void OnRegistered() {
        StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
        StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
    }

    /// <inheritdoc/>
    protected override void OnValuesPopulated() {
        StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
    }

    /// <inheritdoc/>
    protected override bool TryCacheValue(out Item value, string match) {
        value = null;

        if (string.IsNullOrEmpty(match)) return false;
        if (!ItemContext.TryFindItem(match, out value))
            Pack.Logger.LogWarning($"[Debug-SoundReplacementLoader] Item name field '{Value}' for one \"LethalCompany:item:name\" or \"LethalCompany:scrap:name\" condition in SoundPack '{Pack.Name}' returned no successful matches!");

        return value != null;
    }

    /// <inheritdoc/>
    protected override bool TryObtainValueWithContext(out Item value, ItemContext context) {
        value = null;
        if (!context.Item) return false;
        value = context.Item.itemProperties;
        return value != null;
    }
}