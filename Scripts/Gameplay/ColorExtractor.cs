using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay
{
    public class ColorExtractor : MonoBehaviour
    {
        [SerializeField] private Texture2D _sourceTexture;
        [SerializeField] private int _offset;
        [SerializeField] private int _numberOfColors;
        [SerializeField] private List<Color> _extractedColors = new();
        [SerializeField] private Material _baseMaterial;

        private int _currentColorIndex;

        private List<Material> _extractedMaterials = new();

        private void Awake()
        {
            for (int i = 0; i < _extractedColors.Count; i++)
            {
                Material material = Instantiate(_baseMaterial);
                material.color = _extractedColors[i];
                _extractedMaterials.Add(material);
            }
        }

        [Button]
        private void ExtractColors()
        {
            _extractedColors.Clear();

            for (int i = 0; i < _numberOfColors; i++)
            {
                int x = _offset + i * (_sourceTexture.width / _numberOfColors);

                if (x < _sourceTexture.width)
                {
                    Color pixelColor = _sourceTexture.GetPixel(x, 0);
                    _extractedColors.Add(pixelColor);
                }
                else
                {
                    Debug.LogWarning("Выход за границы текстуры");
                    break;
                }
            }
        }

        //public Color GetColor()
        //{
        //    Color currentColor = _extractedColors[_currentColorIndex];
        //    _currentColorIndex = (_currentColorIndex + 1) % _extractedColors.Count;
        //    return currentColor;
        //}

        public Material GetMaterial()
        {
            Material currentMaterial = _extractedMaterials[_currentColorIndex];
            _currentColorIndex = (_currentColorIndex + 1) % _extractedColors.Count;
            return currentMaterial;
        }
    }
}