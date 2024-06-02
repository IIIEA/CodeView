using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public class MeshMarker : MonoBehaviour
    {
        [SerializeField] private CountryTitle _countryTitle;

        private MeshRenderer _mesh;
        private MeshCollider _meshCollider;
        private Vector3 originalPosition;

        // private void OnValidate()
        // {
        //
        //     // _countryTitle = transform.parent.GetComponentInChildren<CountryTitle>(true);
        //
        //     // if (_countryTitle != null)
        //     //     _countryTitle.transform.parent = transform;
        //
        //     // originalPosition = transform.position;
        //     //
        //     // GameObject model = new GameObject("Model");
        //     // model.transform.SetParent(transform.parent);
        //     // model.transform.localScale = new Vector3(200f, 100f, 1f);
        //     // model.transform.position = Vector3.zero;
        //     // transform.parent = model.transform;
        //     // transform.localScale = Vector3.one;
        //     // transform.position = originalPosition;
        // }

        private void Awake()
        {
            _mesh = GetComponent<MeshRenderer>();
            _meshCollider = GetComponent<MeshCollider>();
        }

        public Vector3 GetBoundCenter()
        {
            Bounds bounds = _mesh.bounds;
            return bounds.center;
        }

        public void SetMaterial(Material material)
        {
            if (_mesh != null)
            {
                _mesh.material = material;
            }
        }

        public void DisableCollider()
        {
            if (_meshCollider != null)
                _meshCollider.enabled = false;
        }

        public void EnableCollider()
        {
            if (_meshCollider != null)
                _meshCollider.enabled = true;
        }
    }
}