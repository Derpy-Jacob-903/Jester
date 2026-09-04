using BaseLib.Abstracts;
using BaseLib.Extensions;
using Jester.JesterCode.Cards;
using Jester.JesterCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Jester.JesterCode.Powers;

public class ChargePower : TemporaryStrengthPower, ICustomPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override AbstractModel OriginModel => ModelDb.Card<ExoticForm>();
    public string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}