using UnityEngine;

namespace Project.Card
{
    [CreateAssetMenu(menuName = "Configurations/Card/" + nameof(CardViewConfig),
        fileName = nameof(CardViewConfig) + "_Default")]
    public class CardViewConfig : ScriptableObject
    {
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public Sprite BackSideSprite { get; private set; }
    }
}