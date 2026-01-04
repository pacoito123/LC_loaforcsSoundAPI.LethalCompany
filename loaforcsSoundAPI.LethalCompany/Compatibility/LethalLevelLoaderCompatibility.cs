using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using LethalLevelLoader;
using loaforcsSoundAPI.LethalCompany.Conditions.OtherMods.LethalLevelLoader;
using loaforcsSoundAPI.Reporting;

namespace loaforcsSoundAPI.LethalCompany.Compatibility;

public static class LethalLevelLoaderCompatibility {
    /// <summary>
    ///     Whether <c>LethalLevelLoader</c> is present in the BepInEx Chainloader or not.
    /// </summary>
    public static bool Enabled {
        get {
            _enabled ??= loaforcsSoundAPILethalCompany.CheckSoftDep(LethalLevelLoader.Plugin.ModGUID);
            return (bool)_enabled;
        }
    }
    private static bool? _enabled;

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static void RegisterLLLConditions() {
        loaforcsSoundAPILethalCompany.Logger.LogInfo("LethalLevelLoader found, registering conditions on SoundAPI side.");
        SoundAPI.RegisterCondition("LethalLevelLoader:dungeon:has_tag", () => new LLLTagCondition<ExtendedDungeonFlow>(() => {
            if (!RoundManager.Instance) return null;
            if (!RoundManager.Instance.dungeonGenerator) return null;
            if (!PatchedContent.TryGetExtendedContent(
                    RoundManager.Instance.dungeonGenerator.Generator.DungeonFlow,
                    out ExtendedDungeonFlow lllDungeon)
               ) return null;
            return lllDungeon;
        }));
        SoundAPI.RegisterCondition("LethalLevelLoader:moon:has_tag", () => new LLLTagCondition<ExtendedLevel>(() => {
            if (!StartOfRound.Instance) return null;
            if (!PatchedContent.TryGetExtendedContent(
                    StartOfRound.Instance.currentLevel,
                    out ExtendedLevel lllMoon)
               ) return null;
            return lllMoon;
        }));
    }


    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static void WriteLLLDataToReport(StreamWriter stream) {
        HashSet<string> tags = [];
        loaforcsSoundAPILethalCompany.Logger.LogDebug("writing lll data!!");
        List<ExtendedMod> mods = [PatchedContent.VanillaMod, .. PatchedContent.ExtendedMods];
        foreach (ExtendedMod mod in mods) {
            foreach (ExtendedContent content in mod.ExtendedContents) {
                tags.UnionWith(content.ContentTagStrings);
            }
        }

        SoundReportHandler.WriteList("Found Lethal Level Loader Tags (CASE-SENSITIVE)", stream, tags);
    }
}