using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using Project.Features.CardModule;

namespace Project.Features.BoardModule
{
    public class BoardPresenter
    {
        private readonly BoardView _view;
        private readonly BoardPresenterConfig _config;
        private readonly List<CardView> _openedCards = new();
        
        private readonly List<CardView> _currentPair = new(2);
        
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
                _openedCards.AddRange(_currentPair);
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
