using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Scrap;

[SoundAPICondition("LethalCompany:item:name")]
[SoundAPICondition("LethalCompany:scrap:name")]
public class ItemNameCondition : Condition<ItemContext> {
    public ItemsRegistry Value { get; private set; }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(ItemContext context) {
        if (!context.Item) return false;

        return Value.ContainsValue(context.Item.itemProperties);
    }
}