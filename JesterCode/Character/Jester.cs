using BaseLib.Abstracts;
using BaseLib.Utils;
using BaseLib.Utils.NodeFactories;
using Jester.JesterCode.Extensions;
using Godot;
using Jester.JesterCode.Cards;
using Jester.JesterCode.Relics;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Jester.JesterCode.Character;

public class Jester : PlaceholderCharacterModel
{
    public const string CharacterId = "Jester";

    public override string PlaceholderID => "necrobinder";
    public static readonly Color Color = new("CD4EED");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 80;
    
    
    /*public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<MetronomeJester>(),
        ModelDb.Card<MetronomeJester>(),
        ModelDb.Card<MetronomeJester>(),
        ModelDb.Card<MetronomeJester>(),
        ModelDb.Card<MetronomeJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<Bodyguard>(),
        ModelDb.Card<HatTrick>()
    ];*/
    /*public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<MetronomeJester>(),
        ModelDb.Card<HatTrick>(),
    ];*/
    
    /*public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<MetronomeJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<Survivor>(),
        ModelDb.Card<Venerate>(),
        ModelDb.Card<Bodyguard>(),
        ModelDb.Card<Zap>(),
    ];*/
    
    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<MetronomeJester>(),
        ModelDb.Card<JesterSkill>(),
        ModelDb.Card<JesterSkill>(),
        ModelDb.Card<JesterSkill>(),
        ModelDb.Card<JesterSkill>(),
        ModelDb.Card<HatTrick>(),
    ];
    /*public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<Metronome>(),
        ModelDb.Card<Metronome>(),
        ModelDb.Card<Metronome>(),
        ModelDb.Card<Metronome>(),
        ModelDb.Card<Metronome>()
    ];*/
    /*public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<JesterBasic>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<DefendJester>(),
        ModelDb.Card<Metronome>()
    ];*/

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<JestersHat>()
    ];
    
    public override NCreatureVisuals CreateCustomVisuals()
    {
        return NodeFactory<NCreatureVisuals>.CreateFromResource(PreloadManager.Cache.GetTexture2D("res://" + "MINIBOSS_Arthur_Idle.png".CharacterUiPath()));
    }

    public override CardPoolModel CardPool => ModelDb.CardPool<JesterCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<JesterRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<JesterPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();

    private static readonly Random rng = new Random();

    public override string CustomMapMarkerPath
    {
        get
        {
            var characterModels = ModelDb.AllCharacters.Where(c => c is not Jester).ToList();
            if (characterModels.Count == 0)
                return null;
            var chosen = characterModels[rng.Next(characterModels.Count)];
            return chosen.MapMarker.LoadPath;
        }
    }
    //"map_marker_char_name.png".CharacterUiPath();
    //"Passively"
    
    /*public override CardCreationOptions ModifyCardRewardCreationOptions(
        Player player,
        CardCreationOptions options)
    {
        if (player.Character is not Jester || options.Flags.HasFlag((Enum) CardCreationFlags.NoCardPoolModifications) || options.CustomCardPool != null || options.CardPools.All<CardPoolModel>((Func<CardPoolModel, bool>) (p => p.IsColorless)))
            return options;
        //IEnumerable<CardPoolModel> pools = player.UnlockState.CharacterCardPools.Union<CardPoolModel>((IEnumerable<CardPoolModel>) options.CardPools);
        return options.WithCustomPool(ModelDb.AllCards, options.RarityOdds);
    }*/
}

/// <summary>
/// 
/// </summary>
interface IMetronomeBlacklist
{
    
}

