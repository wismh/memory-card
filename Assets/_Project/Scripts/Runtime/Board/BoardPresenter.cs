using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using Project.Card;
using UnityEngine;

namespace Project.Board
{
    public class BoardPresenter
    {
        private readonly BoardView _view;
        private readonly BoardPresenterConfig _config;
        
        private readonly List<CardView> _currentPair = new(2);
        private readonly List<CardView> _openedCards = new();

        private int _cardCount;
        
        public event Action OnBoardComplete;
        
        public BoardPresenter(BoardPresenterConfig config,
                              BoardView view)
        {
            _view = view;
            _config = config;
            
            _view.OnCardClick += HandleCardClick;
        }

        ~BoardPresenter()
        {
            _view.OnCardClick -= HandleCardClick;
        }

        public void Configure(int cardCount)
        {
            _cardCount = cardCount;
            _openedCards.Capacity = cardCount;
            _view.Configure(cardCount);
        }
        
        private void HandleCardClick(CardView view)
        {
            _ = view.CardModel.IsOpened ?
                TryCloseCard(view) : TryOpenCard(view);
        }

        private bool TryOpenCard(CardView cardView)
        {
            if (_openedCards.Contains(cardView))
                return true;

            if (_currentPair.Count >= 2)
                return false;
            
            cardView.CardModel.IsOpened = true;
            cardView.Open();
            
            _currentPair.Add(cardView);

            if (_currentPair.Count == 2)
                _ = ResolveOpenedPair();    
            
            return true;
        }

        private async UniTask ResolveOpenedPair()
        {
            await UniTask.Delay(_config.DelayBeforeMatch);

            if (_currentPair[0].CardType.Id == _currentPair[1].CardType.Id)
            {
                _openedCards.AddRange(_currentPair);
                if (_openedCards.Count == _cardCount)
                    OnBoardComplete?.Invoke();
            }
            else
            {
                _currentPair[0].CardModel.IsOpened = false;
                _currentPair[1].CardModel.IsOpened = false;
                
                _currentPair[0].Close();
                _currentPair[1].Close();
            }
            
            _currentPair.Clear();
        }

        private bool TryCloseCard(CardView cardView)
        {
            if (!_currentPair.Contains(cardView))
                return false;

            _currentPair.Remove(cardView);
            
            cardView.CardModel.IsOpened = false;
            cardView.Close();
            
            return true;
        }
    }
}
