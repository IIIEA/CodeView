#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial
{
    [CustomEditor(typeof(TutorialManager))]
    public sealed class TutorialManagerEditor : Editor
    {
        private SerializedProperty _stepList;

        private TutorialManager _manager;

        private void Awake()
        {
            _stepList = serializedObject.FindProperty(nameof(_stepList));
            _manager = (TutorialManager) target;
        }

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                GUI.enabled = false;
                EditorGUILayout.Toggle("Completed", _manager.IsCompleted);
                EditorGUILayout.EnumPopup("Current Step", _manager.CurrentStep);
                GUI.enabled = true;
                
                EditorGUILayout.Space(8);
                if (GUILayout.Button("Move Next"))
                {
                    _manager.FinishCurrentStep();
                    _manager.MoveToNextStep();
                }
            }
            
            EditorGUILayout.Space(4.0f);
            EditorGUILayout.PropertyField(_stepList, includeChildren: true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif