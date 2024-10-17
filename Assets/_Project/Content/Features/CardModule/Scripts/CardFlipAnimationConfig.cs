using UnityEngine;

namespace Project.Features.CardModule
{
    [CreateAssetMenu(menuName = "Configurations/Card/" + nameof(CardFlipAnimationConfig),
        fileName = nameof(CardFlipAnimationConfig) + "_Default")]
    public class CardFlipAnimationConfig : ScriptableObject
    {
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public Sprite BackSideSprite { get; private set; }
    }
}
