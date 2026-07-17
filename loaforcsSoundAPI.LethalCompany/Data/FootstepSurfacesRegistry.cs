using System;
using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;
using Newtonsoft.Json.Linq;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class FootstepSurfacesRegistry : Registry<FootstepSurface, HashSet<FootstepSurface>> {
    public FootstepSurfacesRegistry() : base() => StartOfRoundPatch.StartOfRoundAwake += PopulateRegistry;

    /// <inheritdoc/>
    public override void OnRegistryPopulated() => StartOfRoundPatch.StartOfRoundAwake -= PopulateRegistry;

    /// <inheritdoc/>
    public override bool TryParse(out FootstepSurface value, JToken token) {
        value = null;

        if (!StartOfRound.Instance) return false;
        if (StartOfRound.Instance.footstepSurfaces == null) return false;

        string match = token.ToString();
        if (string.IsNullOrEmpty(match)) return false;

        value = Array.Find(StartOfRound.Instance.footstepSurfaces, surface => surface != null
            && string.Equals(surface.surfaceTag, match, StringComparison.InvariantCultureIgnoreCase));

        return value != null;
    }
}