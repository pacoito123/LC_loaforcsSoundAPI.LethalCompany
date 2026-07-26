using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions;

[SoundAPICondition("LethalCompany:apparatus_state")]
public class ApparatusStateCondition : Condition {
	internal static bool CurrentApparatusPulled = false;

	readonly HashSet<AudioSource> exhaustedSources = [];

	public StateType? Value { get; private set; }

	public bool? OnceAfterPull { get; private set; }

	/// <inheritdoc/>
	public override void OnRegistered() {
		SceneManager.sceneUnloaded -= ResetApparatusState;
		SceneManager.sceneUnloaded += ResetApparatusState;
	}

	void ResetApparatusState(Scene scene) {
		CurrentApparatusPulled = false;
		exhaustedSources.Clear();
	}

	/// <inheritdoc/>
	public override bool Evaluate(IContext context) {
		bool result = Value == (CurrentApparatusPulled ? StateType.PULLED : StateType.PLUGGED_IN);

		if (OnceAfterPull.HasValue) {
			result = exhaustedSources.Add(context.Source);
		}

		return result;
	}

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		return Value.HasValue ? [] : [new(IValidatable.ResultType.FAIL,
			$"Value field for one \"LethalCompany:apparatus_state\" condition in SoundPack '{Pack.Name}' is empty or missing!")];
	}
}

public enum StateType : byte {
	PULLED,
	PLUGGED_IN
}