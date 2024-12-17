using System.Collections.Generic;
using UnityEngine;

using Project.Card;

namespace Project.Board
{
    [CreateAssetMenu(menuName = "Configurations/Board/" + nameof(BoardComposerConfig),
        fileName = nameof(BoardComposerConfig) + "_Default")]
    public class BoardComposerConfig : ScriptableObject
    {
        [field: SerializeField] public List<CardType> Types { get; private set; }
    }
}
