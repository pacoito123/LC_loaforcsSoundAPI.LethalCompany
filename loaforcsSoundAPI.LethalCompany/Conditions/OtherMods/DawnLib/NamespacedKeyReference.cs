using Dawn;
using loaforcsSoundAPI.SoundPacks.Data;

namespace loaforcsSoundAPI.LethalCompany.Conditions.OtherMods.DawnLib;

public class NamespacedKeyReference : ContentReference<NamespacedKey> {
    public NamespacedKeyReference(string input) : base(input) => Resolve();

    /// <inheritdoc/>
    protected override bool TryResolve(string input, out NamespacedKey value) => NamespacedKey.TryParse(input, out value);
}