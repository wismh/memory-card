using UnityEngine;
using UnityEngine.UI;

using Project.Features.CardModule;

namespace Project.Features.BoardModule
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _grid;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            _grid ??= GetComponentInChildren<GridLayoutGroup>();
        }
#endif

        public void Configure(int cardCount)
        {
            
        }

        public void AddCard(CardView cardView)
        {
            cardView.transform.SetParent(transform, false);
        }
    }
}