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
[BepInDependency("imabatby.lethallevelloader", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.github.teamxiaolan.dawnlib", BepInDependency.DependencyFlags.SoftDependency)]
public class loaforcsSoundAPILethalCompany : BaseUnityPlugin {
	internal new static ManualLogSource Logger { get; private set; }

	private void Awake() {
		//SoundAPI.RegisterNetworkAdapter(new NGONetworkAdapter());
		Logger = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
		Config.SaveOnConfigSet = false;
		SoundAPI.RegisterAll(Assembly.GetExecutingAssembly());

		if (LethalLevelLoaderCompatibility.Enabled) {
			LethalLevelLoaderCompatibility.RegisterLLLConditions();
		}

		if (DawnLibCompatibility.Enabled) {
			DawnLibCompatibility.RegisterDawnConditions();
		}

		if (SoundReportHandler.CurrentReport != null) {
			LethalCompanySoundReport.Init();
		}

		// todo
		// SoundFixesConfig.Bind(Config);

		Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);
		harmony.PatchAll(typeof(ApparatusPatch));
		harmony.PatchAll(typeof(AudioReverbTriggerPatch));
		harmony.PatchAll(typeof(EnemyAIPatch));
		harmony.PatchAll(typeof(GrabbableObjectPatch));
		harmony.PatchAll(typeof(HangarShipDoorPatch));
		harmony.PatchAll(typeof(ItemDropshipPatch));
		harmony.PatchAll(typeof(PlayerControllerPatch));
		harmony.PatchAll(typeof(RoundManagerPatch));
		harmony.PatchAll(typeof(StartOfRoundPatch));
		harmony.PatchAll(typeof(VehicleControllerPatch));

		Config.Save();
		Logger.LogInfo("Done.");
	}
}