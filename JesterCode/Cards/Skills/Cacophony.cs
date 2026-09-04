using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Jester.JesterCode.Cards;

public class CacophonyJester() : JesterCard(2, CardType.Power,
    CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(33),
        new DamageVar(66M, ValueProp.Unpowered)
    ];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.SingleplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CacophonyPower cacophonyPower = await PowerCmd.Apply<CacophonyPower>(choiceContext, Owner.Creature, DynamicVars.Damage.IntValue, Owner.Creature, this);
    }
    //public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    
    protected override void OnUpgrade() => DynamicVars.Energy.UpgradeValueBy(-1M);
}