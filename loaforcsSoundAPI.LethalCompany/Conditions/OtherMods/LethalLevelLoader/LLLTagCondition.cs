using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LethalLevelLoader;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.OtherMods.LethalLevelLoader;

public class LLLTagCondition<T>(Func<T> generator) : Condition where T : ExtendedContent {
	[NonSerialized]
	readonly Func<T> _generator = generator;

	public List<ContentTagReference> Value { get; private set; }

	[CanBeNull]
	public bool? CheckAll { get; private set; }

	public override bool Evaluate(IContext context) {
		T content = _generator();
		if (!content) return false;

		foreach (ContentTagReference reference in Value) {
			if (content.ContentTags.Find(reference.HasTag) != null) {
				if (!CheckAll.GetValueOrDefault()) {
					return true;
				}
			} else if (CheckAll.GetValueOrDefault()) {
				return false;
			}
		}

		return true;
	}

    /// <inheritdoc/>
    public override List<IValidatable.ValidationResult> Validate() {
        if (Value == null || Value.Count == 0) {
            return [
                new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one LLLTagCondition of type '{typeof(T).GetType()}' in SoundPack '{Pack.Name}' is empty or missing!")
            ];
        }

        return [];
    }
}