using UnityEngine;
using UnityEngine.Serialization;

namespace _Fly_Connect.Scripts
{
   public class Finger : MonoBehaviour
   {
      [FormerlySerializedAs("_rectTransform")] [field: SerializeField] public RectTransform RectTransform;
      [FormerlySerializedAs("_animator")] [field: SerializeField] public Animator Animator;
      [field: SerializeField] public RectTransform Arrow;
   }
}
