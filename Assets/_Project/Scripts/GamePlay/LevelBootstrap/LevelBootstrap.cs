using Project;
using UnityEngine;
using Zenject;

namespace _Project
{
    public class LevelBootstrap : MonoBehaviour
    {
        private BoardComposer _boardComposer;
        
        [Inject]
        public void Construct(BoardComposer boardComposer)
        {
            _boardComposer = boardComposer;
        }

        private void Start()
        {
            _boardComposer.Compose();
        }
    }
}
