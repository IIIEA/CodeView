#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    [CustomEditor(typeof(CityPoint))]
    [CanEditMultipleObjects]
    public class CityPointEditor : OdinEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CityPoint city = (CityPoint)target;

            bool isSelectedCity = Selection.activeObject == city.gameObject;

            if (GUILayout.Button("Add city"))
            {
                GameObject obj1 = (GameObject)Selection.objects[0];
                CityPoint city1 = obj1.GetComponent<CityPoint>();
                city1.Country.AddGameCity(city1);
                city1.SwitchIsGameCity(true);
            }

            if (GUILayout.Button("Remove City"))
            {
                GameObject obj1 = (GameObject)Selection.objects[0];
                CityPoint city1 = obj1.GetComponent<CityPoint>();
                city1.Country.RemoveGameCity(city1);
                city1.SwitchIsGameCity(false);
            }

            if (GUILayout.Button((isSelectedCity && city.ShowCitiesGizmo ? "Disable" : "Enable") + " Mesh Renderer"))
            {
                ToggleMeshRenderer(city, !city.ShowCitiesGizmo);
                SceneView.RepaintAll();
            }
            
            if (EditorApplication.isPlaying)
            {
                city.Country.ShowCities = false;
                SceneView.RepaintAll();
            }

            SceneView.RepaintAll();
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        private static void OnDrawGizmos(CityPoint cityPoint, GizmoType gizmoType)
        {
            if (cityPoint.ShowCitiesGizmo)
            {
                Handles.color = !cityPoint.IsGameCity ? Color.white : Color.red;

                // float size = Remap.DoRemap(cityPoint.Country.CountryData.MinPopulation,
                //     cityPoint.Country.CountryData.MaxPopulation, 0.05f, 0.8f, cityPoint._data.Population);
                //
                //cityPoint.transform.localScale = new Vector3(1f, 1f, 1f);
                Handles.DrawSolidDisc(cityPoint.transform.position, Vector3.forward, 0.1f);
            }
        }

        private static void ToggleCityGizmo(CityPoint cityPoint, bool enable)
        {
            cityPoint.ShowCitiesGizmo = enable;
            SceneView.RepaintAll();
        }

        private static void ToggleMeshRenderer(CityPoint cityPoint, bool enable)
        {
            MeshRenderer meshRenderer = cityPoint.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = enable;
                cityPoint.ShowCitiesGizmo = enable;
            }
            SceneView.RepaintAll();
        }
    }
}
#endif