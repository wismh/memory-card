using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Lumenwake.UIModule
{
    public class HyperLinkHandler : MonoBehaviour
    {
        [SerializeField] private BaseScreen _screenDestination;
        [SerializeField] private Button _button;

        private BaseScreenManager _screenManager;

        [Inject]
        public void Construct(BaseScreenManager screenManager)
        {
            _screenManager = screenManager;
        }
        
        private void Awake()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }
        
        private void HandleButtonClick()
        {
            _ = _screenManager.OpenScreen(_screenDestination.GetType());
        }
    }
}