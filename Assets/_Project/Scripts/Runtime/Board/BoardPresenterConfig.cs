using UnityEngine;

namespace Project.Board
{
    [CreateAssetMenu(menuName = "Configurations/Board/" + nameof(BoardPresenterConfig),
        fileName = nameof(BoardPresenterConfig) + "_Default")]
    public class BoardPresenterConfig : ScriptableObject
    {
        [field: SerializeField] public int DelayBeforeMatch { get; private set; }
    }
}
