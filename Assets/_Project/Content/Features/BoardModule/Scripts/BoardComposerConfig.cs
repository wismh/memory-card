using System.Collections.Generic;
using UnityEngine;

using Project.Features.CardModule;

namespace Project.Features.BoardModule
{
    [CreateAssetMenu(menuName = "Configurations/Board/" + nameof(BoardComposerConfig),
        fileName = nameof(BoardComposerConfig) + "_Default")]
    public class BoardComposerConfig : ScriptableObject
    {
        [field: SerializeField] public List<CardType> Types { get; private set; }
    }
}
