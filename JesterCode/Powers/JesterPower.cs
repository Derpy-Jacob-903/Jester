using BaseLib.Abstracts;
using BaseLib.Extensions;
using Jester.JesterCode.Extensions;

namespace Jester.JesterCode.Powers;

public abstract class JesterPower : CustomPowerModel
{
    //Loads from Jester/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}