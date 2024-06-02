using System.Collections.Generic;
using System.Linq;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class MeshChanger : MonoBehaviour
{
    [SerializeField] private GameObject _fbxObject;
    [SerializeField] private Country[] _countries;
    [SerializeField] private List<MeshFilter> _oldMeshFilter = new();
    [SerializeField] MeshFilter[] _newMeshFilter;

    [Button]
    private void GetRendererList()
    {
        _oldMeshFilter.Clear();
        _newMeshFilter = _fbxObject.GetComponentsInChildren<MeshFilter>(true);

        foreach (var country in _countries)
        {
            if (country.MeshMarker != null)                                                  
                _oldMeshFilter.Add(country.MeshMarker.GetComponentInChildren<MeshFilter>(true));
        }
    }

    [Button]
    private void ReplaceMeshesInFBX()
    {
        for (int i = 0; i < _oldMeshFilter.Count; i++)
        {
            string[] nameParts = _oldMeshFilter[i].sharedMesh.name.Split(' ');

            string firstWord = nameParts[0];

            MeshFilter matchingMesh = _newMeshFilter.FirstOrDefault(mesh => mesh.sharedMesh.name.StartsWith(firstWord));

            if (matchingMesh != null)
            {
                _oldMeshFilter[i].sharedMesh = matchingMesh.sharedMesh;
            }
            else
            {
                Debug.LogWarning($"Mesh with name {firstWord} not found in the new mesh filters.");
            }
        }
    }
}