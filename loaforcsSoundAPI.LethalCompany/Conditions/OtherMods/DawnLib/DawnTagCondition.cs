using System;
using System.Collections.Generic;
using Dawn;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.OtherMods.DawnLib;

public class DawnTagCondition<T>(Func<T> generator) : Condition where T : DawnBaseInfo<T> {
    [NonSerialized]
    readonly Func<T> _generator = generator;

    public List<NamespacedKeyReference> Value { get; private set; }

    [CanBeNull]
    public bool? CheckAll { get; private set; }

    public override bool Evaluate(IContext context) {
        T content = _generator();
        if (content == null) return false;

        foreach (NamespacedKeyReference reference in Value) {
            if (content.HasTag(reference.Value)) {
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
                new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one DawnTagCondition of type '{typeof(T).GetType()}' in SoundPack '{Pack.Name}' is empty or missing!")
            ];
        }

        return [];
    }
}