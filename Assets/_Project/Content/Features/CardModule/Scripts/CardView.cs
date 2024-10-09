using UnityEngine;
using UnityEngine.UI;

namespace Project.Features.CardModule 
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image _image;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _image ??= GetComponentInChildren<Image>();
        }
#endif
        
        public void SetSize(Vector2 size)
        {
            _image.rectTransform.sizeDelta = size;
        }
    }
}