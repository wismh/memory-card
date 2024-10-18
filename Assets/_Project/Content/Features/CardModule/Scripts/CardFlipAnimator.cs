using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Features.CardModule
{
    public class CardFlipAnimator
    {
        private Image _image;
        private RectTransform _rectTransform;

        private Tween _tween;
        
        public void SetImage(Image image)
        {
            _image = image;
            _rectTransform = image.rectTransform;
        }
        
        public void Flip(Sprite sprite, float duration)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("CardFlipAnimator: Image not set");
                return;
            }

            _tween.Stop();

            float halfDuration = duration * 0.5f;

            _tween = Tween
                .ScaleX(_rectTransform, 0f, halfDuration, Ease.InQuad)
                .OnComplete(() => 
                {
                    _image.sprite = sprite;
                    _tween = Tween.ScaleX(
                        _rectTransform, 1f, halfDuration, Ease.OutQuad
                    );
                });
        }
    }
}
