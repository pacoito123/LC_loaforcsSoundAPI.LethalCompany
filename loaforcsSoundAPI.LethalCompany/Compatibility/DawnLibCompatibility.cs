using System.Runtime.CompilerServices;
using Dawn;
using loaforcsSoundAPI.LethalCompany.Conditions.OtherMods.DawnLib;

namespace loaforcsSoundAPI.LethalCompany.Compatibility;

static class DawnLibCompatibility {
    /// <summary>
    /// Whether <c>DawnLib</c> is present in the BepInEx Chainloader or not.
    /// </summary>
    public static bool Enabled {
        get {
            _enabled ??= BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.github.teamxiaolan.dawnlib");

            return (bool) _enabled;
        }
    }
    static bool? _enabled;

	[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static void RegisterDawnConditions() {
        loaforcsSoundAPILethalCompany.Logger.LogInfo("DawnLib found, registering conditions on SoundAPI side.");
        SoundAPI.RegisterCondition("DawnLib:dungeon:has_tag", static () => new DawnTagCondition<DawnDungeonInfo>(static () => {
            if (!RoundManager.Instance) return null;
            if (!RoundManager.Instance.dungeonGenerator) return null;
            if (!RoundManager.Instance.dungeonGenerator.Generator?.DungeonFlow) return null;
            return RoundManager.Instance.dungeonGenerator.Generator?.DungeonFlow.GetDawnInfo();
        }));
        SoundAPI.RegisterCondition("DawnLib:moon:has_tag", static () => new DawnTagCondition<DawnMoonInfo>(static () => {
            if (!StartOfRound.Instance) return null;
            if (!StartOfRound.Instance.currentLevel) return null;
            return StartOfRound.Instance.currentLevel.GetDawnInfo();
        }));
    }
}