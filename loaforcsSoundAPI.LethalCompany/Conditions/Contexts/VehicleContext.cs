using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public class VehicleContext(VehicleController vehicle) : IContext {
	public static VehicleController? FallbackVehicle { get; internal set; }
	public VehicleController Vehicle => vehicle;
}