using System;
using JetBrains.Annotations;
using LethalLevelLoader;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.OtherMods.LethalLevelLoader;

public class LLLTagCondition<T>(Func<T> generator) : Condition where T : ExtendedContent {
	[field: NonSerialized]
	Func<T> _generator = generator;

	[CanBeNull]
	public string Value { get; internal set; } = null;

	public override bool Evaluate(IContext context) {
		T content = _generator();
		return content != null && content.TryGetTag(Value);
	}
}