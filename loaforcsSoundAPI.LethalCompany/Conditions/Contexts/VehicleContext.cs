using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public class VehicleContext(VehicleController vehicle) : IContext {
	public VehicleController Vehicle => vehicle;
}