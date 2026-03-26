using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions;

[SoundAPICondition("LethalCompany:apparatus_state")]
public class ApparatusStateCondition : Condition {
	public enum StateType : byte {
		PULLED,
		PLUGGED_IN
	}
	internal static bool CurrentApparatusPulled = false;

	private readonly HashSet<AudioSource?> exhaustedSources = [];

	public StateType? Value { get; internal set; }

	public bool? OnceAfterPull { get; internal set; }

	/// <inheritdoc/>
	protected override void OnRegistered() {
		SceneManager.sceneUnloaded += ResetApparatusState;
	}

	private void ResetApparatusState(Scene scene) {
		CurrentApparatusPulled = false;
		exhaustedSources.Clear();
	}

	/// <inheritdoc/>
	public override bool Evaluate(IContext context) {
		return (CurrentApparatusPulled ? StateType.PULLED : StateType.PLUGGED_IN) == (Value ?? StateType.PULLED)
			&& (!OnceAfterPull.GetValueOrDefault() || exhaustedSources.Add(context.Source));
	}

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		return Value.HasValue ? [] : [new(IValidatable.ResultType.FAIL,
			$"Value field for one \"LethalCompany:apparatus_state\" condition in SoundPack '{Pack.Name}' is empty or missing!")];
	}
}