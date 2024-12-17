using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Project.Card 
{
    public class CardView : MonoBehaviour, IPointerClickHandler
    {
        public delegate void CardClickHandler();
        
        [SerializeField] private Image _image;

        private CardModel _cardModel;
        private CardFlipAnimator _cardFlipAnimator;
        private CardType _cardType;
        private CardViewConfig _config;
        
        public CardType CardType => _cardType;
        public CardModel CardModel => _cardModel;
        
        private event CardClickHandler OnClick;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            _image ??= GetComponentInChildren<Image>();
        }
#endif
        
        [Inject]
        public void InjectDependencies(CardFlipAnimator cardFlipAnimator, 
                                       CardViewConfig config,
                                       CardModel cardModel,
                                       CardType cardType)
        {
            _config = config;
            _cardType = cardType;
            _cardModel = cardModel;
            _cardFlipAnimator = cardFlipAnimator;
            _cardFlipAnimator.SetImage(_image);
        }
        
        public void SetSize(Vector2 size)
        {
            _image.rectTransform.sizeDelta = size;
        }

        public void Open()
        {
            _cardFlipAnimator.Flip(_cardType.FrontSprite, _config.Duration); 
        }

        public void Close()
        {
            _cardFlipAnimator.Flip(_config.BackSideSprite, _config.Duration); 
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();   
        }

        public void AddListener(CardClickHandler handler)
        {
            OnClick += handler;
        }
        
        private void OnDestroy()
        {
            OnClick = null;
        }
    }
}