using UnityEngine;
using UnityEngine.Serialization;

namespace _Fly_Connect.Scripts
{
    public class WorldFinger : MonoBehaviour
    {
        [FormerlySerializedAs("_rectTransform")] [field: SerializeField] public RectTransform RectTransform;
        [FormerlySerializedAs("_animator")] [field: SerializeField] public Animator Animator;
    }
}