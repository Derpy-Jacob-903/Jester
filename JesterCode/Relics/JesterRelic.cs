using BaseLib.Abstracts;
using BaseLib.Utils;
using Jester.JesterCode.Character;
using Jester.JesterCode.Extensions;

namespace Jester.JesterCode.Relics;

[Pool(typeof(JesterRelicPool))]
public abstract class JesterRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.ToLowerInvariant()}.png".BigRelicImagePath();
}