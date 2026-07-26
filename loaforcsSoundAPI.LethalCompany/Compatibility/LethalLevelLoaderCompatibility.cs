using System.Runtime.CompilerServices;
using LethalLevelLoader;
using loaforcsSoundAPI.LethalCompany.Conditions.OtherMods.LethalLevelLoader;

namespace loaforcsSoundAPI.LethalCompany.Compatibility;

static class LethalLevelLoaderCompatibility {
    /// <summary>
    /// Whether <c>LethalLevelLoader</c> is present in the BepInEx Chainloader or not.
    /// </summary>
    public static bool Enabled {
        get {
            _enabled ??= BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("imabatby.lethallevelloader");

            return (bool) _enabled;
        }
    }
    static bool? _enabled;

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static void RegisterLLLConditions() {
        loaforcsSoundAPILethalCompany.Logger.LogInfo("LethalLevelLoader found, registering conditions on SoundAPI side.");
        SoundAPI.RegisterCondition("LethalLevelLoader:dungeon:has_tag", static () => new LLLTagCondition<ExtendedDungeonFlow>(static () => DungeonManager.CurrentExtendedDungeonFlow));
        SoundAPI.RegisterCondition("LethalLevelLoader:moon:has_tag", static () => new LLLTagCondition<ExtendedLevel>(static () => LevelManager.CurrentExtendedLevel));
    }
}