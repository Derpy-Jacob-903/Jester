using System.Runtime.InteropServices;
using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Timeline.Scaffolding;

namespace Jester.JesterCode.Character;

public class JesterEpoch
{
    [RegisterStory]
    public sealed class JesterStory1 : ModStoryTemplate
    {
        protected override string StoryKey => "JESTER_UNLOCK_EPOCH";
    }

    [RegisterEpoch]
    [RegisterStoryEpoch(typeof(JesterStory1))]
    [AutoTimelineSlotAfterColumn(EpochEra.Invitation0)]
    public sealed class JesterEpoch1 : ModEpochTemplate
    {
        public override string Id => "JESTER_UNLOCK_EPOCH";
        
        //public static List<CardModel> Cards => [ModelDb.Card<JesterBasic>(), ModelDb.Card<JesterDefend>(), ModelDb.Card<Jes>()];

        public override void QueueUnlocks()
        {
            NTimelineScreen.Instance.QueueCharacterUnlock<Jester>((EpochModel) this);
            SaveManager.Instance.Progress.PendingCharacterUnlock = ModelDb.Character<Jester>().Id;
        }
        public override EpochModel[] GetTimelineExpansion()
        {
            return
            [
                Get(GetId<JesterEpoch2>()),
            ];
        }
    }
    [RegisterStory]
    public sealed class JesterStory2 : ModStoryTemplate
    {
        protected override string StoryKey => "JESTER_CARDS_EPOCH";
    }

    [RegisterEpoch]
    [RegisterStoryEpoch(typeof(JesterStory2))]
    [AutoTimelineSlotAfterColumn(EpochEra.Invitation0)]
    public sealed class JesterEpoch2 : ModEpochTemplate
    {
        public override string Id => "JESTER_CARDS_EPOCH";

        public static List<CardModel> Cards => [ModelDb.Card<SmartMetronome>(), ModelDb.Card<PowerUp>(), ModelDb.Card<RngCannon>()];

        public override string UnlockText => this.CreateCardUnlockText(Cards);

        public override void QueueUnlocks()
        {
            NTimelineScreen.Instance.QueueCardUnlock(JesterEpoch2.Cards);
        }
    }
}