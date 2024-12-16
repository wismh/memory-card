using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lumenwake.UIModule
{
    public enum HoverEffectWhileSelectedMode
    {
        IgnoreHoverWhileSelected,
        AllowHoverWhileSelected
    }

    [DisallowMultipleComponent]
    public sealed class HoverEffectView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private bool _enabled = true;

        [SerializeField] private RectTransform _effectRoot;
        [SerializeField] private Button _interactableSource;

        [SerializeField] private float _fadeInDuration = 0.12f;
        [SerializeField] private float _fadeOutDuration = 0.12f;
        [SerializeField] private float _peakAlpha = 1f;
        [SerializeField] private Ease _fadeInEase = Ease.OutQuad;
        [SerializeField] private Ease _fadeOutEase = Ease.InQuad;
        [SerializeField] private HoverEffectWhileSelectedMode _whileSelectedMode = HoverEffectWhileSelectedMode.IgnoreHoverWhileSelected;
        
        private CanvasGroup _canvasGroup;
        private Graphic _graphic;
        private Color _graphicBaseRgb;
        private float _resolvedPeakAlpha = 1f;

        private bool _primarySelected;
        private bool _pointerOver;

        private void Awake()
        {
            KillTweenTarget();
            ResolveTargets();
        }

        private void OnDestroy()
        {
            KillTweenTarget();
        }

        private void OnDisable()
        {
            KillTweenTarget();
            SetAlphaImmediate(0f);
            _pointerOver = false;
        }

        public void SetPrimarySelectionActive(bool isActive)
        {
            _primarySelected = isActive;

            if (!_enabled || !HasFadeTarget())
                return;

            if (_primarySelected && _whileSelectedMode == HoverEffectWhileSelectedMode.IgnoreHoverWhileSelected)
            {
                KillTweenTarget();
                SetAlphaImmediate(0f);
                return;
            }

            if (!_primarySelected && _pointerOver && HoverInputAllowed())
                FadeTo(_resolvedPeakAlpha, _fadeInDuration, _fadeInEase);
            else if (!_pointerOver)
            {
                KillTweenTarget();
                SetAlphaImmediate(0f);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerOver = true;

            if (!_enabled || !HasFadeTarget())
                return;

            if (!HoverInputAllowed())
                return;

            FadeTo(_resolvedPeakAlpha, _fadeInDuration, _fadeInEase);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerOver = false;

            if (!_enabled || !HasFadeTarget())
                return;

            if (!HoverInputAllowed())
                return;

            FadeTo(0f, _fadeOutDuration, _fadeOutEase);
        }

        private bool HoverInputAllowed()
        {
            if (_primarySelected && _whileSelectedMode == HoverEffectWhileSelectedMode.IgnoreHoverWhileSelected)
                return false;

            if (!_interactableSource.IsInteractable())
                return false;

            return true;
        }

        private void ResolveTargets()
        {
            _canvasGroup = null;
            _graphic = null;

            _canvasGroup = _effectRoot.GetComponent<CanvasGroup>();
            if (_canvasGroup != null)
            {
                var initialAlpha = _canvasGroup.alpha;
                _resolvedPeakAlpha = Mathf.Clamp01(_peakAlpha <= 0f ? (initialAlpha > 0f ? initialAlpha : 1f) : _peakAlpha);
                _canvasGroup.alpha = 0f;
                return;
            }

            _graphic = _effectRoot.GetComponent<Graphic>();
            if (_graphic == null)
                return;

            var initial = _graphic.color;
            _resolvedPeakAlpha = Mathf.Clamp01(_peakAlpha <= 0f ? (initial.a > 0f ? initial.a : 1f) : _peakAlpha);
            _graphicBaseRgb = new Color(initial.r, initial.g, initial.b, 1f);
            SetAlphaImmediate(0f);
        }

        private bool HasFadeTarget() =>
            _canvasGroup != null || _graphic != null;

        private void FadeTo(float endAlpha, float duration, Ease ease)
        {
            KillTweenTarget();
            if (duration <= 0f)
            {
                SetAlphaImmediate(endAlpha);
                return;
            }

            if (_canvasGroup != null)
            {
                _canvasGroup.DOFade(endAlpha, duration).SetEase(ease).SetUpdate(true);
                return;
            }

            if (_graphic != null)
                _graphic.DOFade(endAlpha, duration).SetEase(ease).SetUpdate(true);
        }

        private void KillTweenTarget()
        {
            if (_canvasGroup != null)
                DOTween.Kill(_canvasGroup);

            if (_graphic != null)
                DOTween.Kill(_graphic);
        }

        private void SetAlphaImmediate(float alpha)
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = alpha;
                return;
            }

            if (_graphic == null)
                return;

            var c = _graphic.color;
            c.r = _graphicBaseRgb.r;
            c.g = _graphicBaseRgb.g;
            c.b = _graphicBaseRgb.b;
            c.a = alpha;
            _graphic.color = c;
        }
    }
}
