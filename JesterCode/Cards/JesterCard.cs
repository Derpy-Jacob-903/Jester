using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using Jester.JesterCode.Character;
using Jester.JesterCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Jester.JesterCode.Cards;

[Pool(typeof(JesterCardPool))]
public abstract class JesterCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath {
        get
        {
            if (ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            if (ResourceLoader.Exists($"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            return MissingPortraitPath;
        }
    }

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => CustomPortraitPath ?? MissingPortraitPath;
    
    public override string BetaPortraitPath {
        get
        {
            if (ResourceLoader.Exists($"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            if (ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            return MissingPortraitPath;
        }
    }
}