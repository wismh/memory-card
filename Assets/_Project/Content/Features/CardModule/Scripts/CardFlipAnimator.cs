using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Features.CardModule
{
    public class CardFlipAnimator
    {
        private readonly CardFlipAnimationConfig _animationConfig;
        
        private Image _image;
        private RectTransform _rectTransform;
        
        private bool _isFlipped;
        private Tween _tween;
        
        public CardFlipAnimator(CardFlipAnimationConfig animationConfig)
        {
            _animationConfig = animationConfig;
        }
        
        public void SetImage(Image image)
        {
            _image = image;
            _rectTransform = image.rectTransform;
        }
        
        public void Flip(Sprite frontSideSprite)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("CardFlipAnimator: Image not set");
                return;
            }

            _tween.Stop();
            _isFlipped = !_isFlipped;

            float halfDuration = _animationConfig.Duration * 0.5f;

            _tween = Tween.LocalRotation(
                _rectTransform,
                new Vector3(0f, 90f, 0f),
                halfDuration,
                Ease.InQuad
            ).OnComplete(() =>
            {
                OnHalfFlip(frontSideSprite);

                _tween = Tween.LocalRotation(
                    _rectTransform,
                    new Vector3(0f, 0f, 0f),
                    halfDuration,
                    Ease.OutQuad
                );
            });
        }
        private void OnHalfFlip(Sprite sprite)
        {
            _image.sprite = _isFlipped ? sprite : _animationConfig.BackSideSprite;
        }
    }
}
