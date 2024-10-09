using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Project.Features.CardModule;

namespace Project.Features.BoardModule
{
    public class BoardView : MonoBehaviour
    {
        [Serializable]
        public struct ConfigurationView
        {
            public int MaxCards;
            public Vector2 CardSize;
            public int Spacing;
        }
        
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private List<ConfigurationView> _configurations;
        
        private readonly List<CardView> _cardViews = new();
            
#if UNITY_EDITOR
        private void OnValidate()
        {
            _grid ??= GetComponentInChildren<GridLayoutGroup>();
        }
#endif

        public void Configure(int cardCount)
        {
            var configuration = GetConfiguration(cardCount);
            
            _grid.spacing = new Vector2(configuration.Spacing, configuration.Spacing);
            _grid.cellSize = configuration.CardSize;
            foreach (var card in _cardViews)
                card.SetSize(configuration.CardSize);
        }

        public void AddCard(CardView cardView)
        {
            _cardViews.Add(cardView);
            cardView.transform.SetParent(transform, false);
        }

        private ConfigurationView GetConfiguration(int cardCount)
        {
            foreach (var configuration in _configurations)
                if (cardCount <= configuration.MaxCards)
                    return configuration;
            
            return default(ConfigurationView);
        }
    }
}