using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Project.Features.CardModule 
{
    public class CardView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;

        private CardFlipAnimator _cardFlipAnimator;
        private Sprite _frontSideSprite;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            _image ??= GetComponentInChildren<Image>();
        }
#endif
        
        [Inject]
        public void InjectDependencies(CardFlipAnimator cardFlipAnimator, 
                                       Sprite frontSpriteSide)
        {
            _frontSideSprite = frontSpriteSide;
            _cardFlipAnimator = cardFlipAnimator;
            _cardFlipAnimator.SetImage(_image);
        }
        
        public void SetSize(Vector2 size)
        {
            _image.rectTransform.sizeDelta = size;
        }

        public void Flip()
        {
            _cardFlipAnimator.Flip(_frontSideSprite);   
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Flip();
        }
    }
}