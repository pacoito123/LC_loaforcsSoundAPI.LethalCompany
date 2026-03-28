using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Scrap;

[SoundAPICondition("LethalCompany:item:name")]
[SoundAPICondition("LethalCompany:scrap:name")] // TODO: Proooobably a bit gullible.
public class ItemNameCondition : MultipleCondition<Item, ItemContext> {
    /// <inheritdoc/>
    protected override void OnRegistered() {
        if(string.IsNullOrEmpty(Value)) return;
        StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
    }

    /// <inheritdoc/>
    protected override void OnValuesPopulated() {
        StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
    }

    /// <inheritdoc/>
    protected override bool CheckValueWithContext(Item value, ItemContext context) {
        return context.Item != null && context.Item.itemProperties == value;
    }

    /// <inheritdoc/>
    protected override bool TryGetValue(out Item value, string match) {
        value = null!;

        if(string.IsNullOrEmpty(match) || !ItemContext.TryFindItem(match, out value))
            Pack.Logger.LogWarning($"[Debug-SoundReplacementLoader] Item name field '{Value}' for one \"LethalCompany:item:name\" condition in SoundPack '{Pack.Name}' returned no successful matches!");

        return value != null;
    }
}