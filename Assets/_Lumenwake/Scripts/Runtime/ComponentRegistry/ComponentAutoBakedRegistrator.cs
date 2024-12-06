using Project.Tools.ComponentRegistry;
using UnityEngine;
using Zenject;

namespace _Lumenwake.ComponentRegistry
{
    public class ComponentAutoBakedRegistrator<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool _nested = false;
        
        private IComponentRegistry<T> _componentRegistry;

        [Inject]
        public void Construct(IComponentRegistry<T> componentRegistry)
        {
            _componentRegistry = componentRegistry;
        }
        
        private void Awake()
        {
            if (_nested) 
                RegisterAllNestedComponents();
            else 
                RegisterFirstLayerComponents();
        }

        private void RegisterFirstLayerComponents()
        {
            foreach (Transform child in transform)
            {
                var component = child.GetComponent<T>();
                if (component)
                    _componentRegistry.Add(component);
            }
        }
        
        private void RegisterAllNestedComponents()
        {
            var components = transform.GetComponentsInChildren<T>();
            foreach (T component in components)
            {
                _componentRegistry.Add(component);
            }
        }
    }
}