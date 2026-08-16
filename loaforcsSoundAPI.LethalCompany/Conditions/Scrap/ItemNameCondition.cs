using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
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

        return Value.Find(it => it.Value == context.Item.itemProperties) != null;
    }

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		if (Value == null || Value.Count == 0) {
			return [
				new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one \"LethalCompany:item:name\" or \"LethalCompany:scrap:name\" condition in SoundPack '{Pack.Name}' is empty or missing!")
			];
		}

		return [];
	}
}