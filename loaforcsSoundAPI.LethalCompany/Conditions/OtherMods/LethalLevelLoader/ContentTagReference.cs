using System.Collections.Generic;
using LethalLevelLoader;
using loaforcsSoundAPI.SoundPacks.Data;

namespace loaforcsSoundAPI.LethalCompany.Conditions.OtherMods.LethalLevelLoader;

public class ContentTagReference : ContentReference<List<ContentTag>> {
    public ContentTagReference(string input) : base(input) => Plugin.onSetupComplete += Resolve;

    /// <inheritdoc/>
    protected override bool TryResolve(string input, out List<ContentTag> value) => ContentTagManager.globalContentTagDictionary.TryGetValue(input, out value);

    public bool HasTag(ContentTag tag) => Value?.Contains(tag) == true;
}