using DG.Tweening;
using Lumenwake;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Card
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
                LoggingSystem.LogError("CardFlipAnimator: Image not set");
                return;
            }

            _tween?.Kill();

            float halfDuration = duration * 0.5f;

            _tween = _rectTransform
                .DOScaleX(0f, halfDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    _image.sprite = sprite;
                    _tween = _rectTransform
                        .DOScaleX(1f, halfDuration)
                        .SetEase(Ease.OutQuad);
                });
        }
    }
}
