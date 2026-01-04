using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using loaforcsSoundAPI.LethalCompany.Compatibility;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.LethalCompany.Reporting;
using loaforcsSoundAPI.Reporting;

namespace loaforcsSoundAPI.LethalCompany;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(SoundAPI.PLUGIN_GUID)]

// Integrations
[BepInDependency(SoundAPI.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(LethalLevelLoader.Plugin.ModGUID, BepInDependency.DependencyFlags.SoftDependency)]
public class loaforcsSoundAPILethalCompany : BaseUnityPlugin {
	internal static new ManualLogSource Logger { get; private set; }
	internal static Harmony Harmony { get; private set; }

	private void Awake() {
		//SoundAPI.RegisterNetworkAdapter(new NGONetworkAdapter());
		Logger = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
		Harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
		Config.SaveOnConfigSet = false;
		SoundAPI.RegisterAll(Assembly.GetExecutingAssembly());

		if (LethalLevelLoaderCompatibility.Enabled) {
			LethalLevelLoaderCompatibility.RegisterLLLConditions();
		}

		if (SoundReportHandler.CurrentReport != null) {
			LethalCompanySoundReport.Init();
		}

		// todo
		// SoundFixesConfig.Bind(Config);

		Harmony.PatchAll(typeof(ApparatusPatch));
		Harmony.PatchAll(typeof(AudioReverbTriggerPatch));
		Harmony.PatchAll(typeof(EnemyAIPatch));
		Harmony.PatchAll(typeof(PlayerControllerPatch));
		Harmony.PatchAll(typeof(RoundManagerPatch));
		Harmony.PatchAll(typeof(StartOfRoundPatch));
		Harmony.PatchAll(typeof(VehicleControllerPatch));

		Config.Save();
		Logger.LogInfo("Done.");
	}

	internal static bool CheckSoftDep(string guid) {
		return BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(guid);
	}
}