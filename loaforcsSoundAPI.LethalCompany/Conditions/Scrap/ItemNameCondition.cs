using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Scrap;

[SoundAPICondition("LethalCompany:item:name")]
[SoundAPICondition("LethalCompany:scrap:name")]
public class ItemNameCondition : Condition<ItemContext> {
    public List<ItemContentReference> Value { get; private set; }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(ItemContext context) {
        if (!context.Item) return false;

        return Value.Find(reference => reference.Value == context.Item.itemProperties) != null;
    }
}