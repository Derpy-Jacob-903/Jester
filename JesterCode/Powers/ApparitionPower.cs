using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Jester.JesterCode.Powers;

public class ApparitionPower : JesterPower, IHasSecondAmount
{
    public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;
  
  

  public string GetSecondAmount()
  {
    return Math.Max(0M, (Decimal)this.Amount - this.GetInternalData<ApparitionPower.Data>().damageReceivedThisTurn)
      .ToString();
  }

  protected override object InitInternalData() => new ApparitionPower.Data();

  public override decimal ModifyHpLostBeforeOstyLate(
    Creature target,
    decimal amount,
    ValueProp props,
    Creature dealer,
    CardModel cardSource)
  {
    return target != this.Owner || amount == 0M ? amount : Math.Min(amount, this.Amount - this.GetInternalData<Data>().damageReceivedThisTurn);
  }

  public override Task AfterModifyingHpLostBeforeOsty()
  {
    this.Flash();
    return Task.CompletedTask;
  }

  public override Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature dealer,
    CardModel cardSource)
  {
    if (target != Owner || result.WasFullyBlocked)
      return Task.CompletedTask;
    Data internalData = GetInternalData<Data>();
    internalData.damageReceivedThisTurn += result.UnblockedDamage;
    InvokeDisplayAmountChanged();
    if (internalData.damageReceivedThisTurn >= Amount)
      Owner.HpDisplay = HpDisplay.InfiniteWithNumbers;
    return Task.CompletedTask;
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    PowerCmd.Decrement(this);
    GetInternalData<Data>().damageReceivedThisTurn = 0M;
    InvokeDisplayAmountChanged();
    Owner.HpDisplay = HpDisplay.Normal;
    return Task.CompletedTask;
  }

  private class Data
  {
    public Decimal damageReceivedThisTurn;
  }
}