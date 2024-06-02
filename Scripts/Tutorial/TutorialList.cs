using System.Collections.Generic;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial
{
    [CreateAssetMenu(
        fileName = "TutorialList",
        menuName = "Tutorial/New TutorialList",
        order = 35
    )]
    public sealed class TutorialList : ScriptableObject
    {
        [SerializeField] 
        private List<TutorialStep> _steps = new();

        public int LastIndex => _steps.Count - 1;

        public TutorialStep this[int index] => _steps[index];

        public int IndexOf(TutorialStep step)
        {
            return _steps.IndexOf(step);
        }

        public bool IsLast(int index)
        {
            return index >= _steps.Count - 1;
        }
    }
}