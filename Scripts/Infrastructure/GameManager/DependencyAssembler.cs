using _Fly_Connect.Scripts.Infrastructure.Locator;
using UnityEngine;

namespace _Fly_Connect.Scripts.Infrastructure.GameManager
{
    public sealed class DependencyAssembler : MonoBehaviour
    {
        [SerializeField] private ServiceLocator _serviceLocator;
        [SerializeField] private MonoBehaviour[] _providers;
    
        private void Start()
        {
            foreach (var provider in _providers)
            {
                if (provider is IInjectProvider tprovider)
                {
                    tprovider.Inject(_serviceLocator);
                }
            }
        
            GameObject[] rootGameObjects = gameObject.scene.GetRootGameObjects();

            foreach (var go in rootGameObjects)
            {
                Inject(go.transform);
            }
        }

        private void Inject(Transform targetTransform)
        {
            MonoBehaviour[] targets = targetTransform.GetComponents<MonoBehaviour>();

            foreach (var target in targets)
            {
                DependencyInjector.Inject(target, _serviceLocator);
            }

            foreach (Transform child in targetTransform)
            {
                Inject(child);
            }
        }
    }
}