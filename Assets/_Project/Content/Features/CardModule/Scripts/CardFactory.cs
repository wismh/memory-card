using UnityEngine;
using Zenject;

namespace Project.Features.CardModule
{
    public class CardFactory
    {
        private readonly DiContainer _container;
        private readonly CardView _prefab;

        public CardFactory(DiContainer container, CardView prefab)
        {
            _container = container;
            _prefab = prefab;
        }

        public CardView Create(Sprite frontSideSprite)
        {
            var card = _container.InstantiatePrefabForComponent<CardView>(
                _prefab, new object[]{ frontSideSprite }
            );
            
            return card;
        }
    }
}