using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public class DropshipContext(ItemDropship dropship) : IContext {
    public static ItemDropship? FallbackDropship { get; internal set; }
    public ItemDropship Dropship => dropship;
}