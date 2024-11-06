using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Project.Features.CardModule;

namespace Project.Features.BoardModule
{
    public class BoardComposer
    {
        private readonly BoardView _boardView;
        private readonly BoardPresenter _boardPresenter;
        private readonly CardFactory _cardFactory;
        private readonly BoardComposerConfig _config;
        
        public BoardComposer(BoardView boardView,
                             CardFactory cardFactory,
                             BoardPresenter boardPresenter,
                             BoardComposerConfig config)
        {
            _config = config;
            _boardView = boardView;
            _cardFactory = cardFactory;
            _boardPresenter = boardPresenter;
        }

        public void Compose(int cardCount)
        {
            int pairsCount = cardCount / 2;

            var shuffledTypes  = _config.Types
                .OrderBy(_ => Random.value)
                .ToList();

            var cards = new List<CardView>();
            
            for (int i = 0; i < pairsCount; ++i)
            {
                var type = shuffledTypes[i % shuffledTypes.Count];
                
                cards.Add(_cardFactory.Create(type));
                cards.Add(_cardFactory.Create(type));
            }

            cards = cards
                .OrderBy(_ => Random.value)
                .ToList();
            
            foreach (var card in cards)
                _boardView.AddCard(card);
            
            _boardPresenter.Configure(cardCount);
        }
    }
}
