using UnityEngine;
namespace Project.Card
{
    [CreateAssetMenu(menuName = "Configurations/Card/" + nameof(CardType),
        fileName = nameof(CardType) + "_Default")]
    public class CardType : ScriptableObject
    {
        [field: SerializeField] public string Id { private set; get; }
        [field: SerializeField] public Sprite FrontSprite { private set; get; }
    }
}
