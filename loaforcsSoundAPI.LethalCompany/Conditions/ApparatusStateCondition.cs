using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions;

[SoundAPICondition("LethalCompany:apparatus_state")]
public class ApparatusStateCondition : Condition {
	public static StateType CurrentApparatusState { get; internal set; } = StateType.PLUGGED_IN;

	readonly HashSet<AudioSource> exhaustedSources = [];

	public StateType? Value { get; private set; }

	[CanBeNull]
	public bool? OnceAfterPull { get; private set; }

	/// <inheritdoc/>
	public override void OnRegistered() => SceneManager.sceneUnloaded += ResetApparatusState;

	void ResetApparatusState(Scene scene) {
		CurrentApparatusState = StateType.PLUGGED_IN;
		exhaustedSources.Clear();
	}

	/// <inheritdoc/>
	public override bool Evaluate(IContext context) {
		bool result = Value == CurrentApparatusState;

		if (result && OnceAfterPull.HasValue) {
			result = !exhaustedSources.Contains(context.Source);
			exhaustedSources.Add(context.Source);
		}

		return result;
	}

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		if (!Value.HasValue) {
			return [
				new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one \"LethalCompany:apparatus_state\" condition in SoundPack '{Pack.Name}' is empty or missing!")
			];
		}
		return [];
	}
}

public enum StateType : byte {
	PULLED,
	PLUGGED_IN
}