using Project.Features.CardModule;

namespace Project.Features.BoardModule
{
    public class BoardComposer
    {
        private readonly BoardView _boardView;
        private readonly CardFactory _cardFactory;
        
        public BoardComposer(BoardView boardView, CardFactory cardFactory)
        {
            _boardView = boardView;
            _cardFactory = cardFactory;
        }

        public void Compose()
        {
            const int cardCount = 12;
            _boardView.Configure(cardCount);

            for (int i = 0; i < cardCount; ++i)
            {
                var card = _cardFactory.Create();
                _boardView.AddCard(card);
            }
        }
    }
}
