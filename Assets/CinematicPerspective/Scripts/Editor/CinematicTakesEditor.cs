using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System;

using System.Linq;
using CinematicPerspective;

namespace CinematicPerspectiveEditor
{
    [CustomEditor(typeof(CinematicTakes))]
    public class CinematicTakesEditor : Editor
    {
        private CinematicTakes script
        {
            get
            {
                return (CinematicTakes)target;
            }
        }

        [InitializeOnLoadMethod]
        public void CheckRigs()
        {
            script.GetRigs();
        }

        public override void OnInspectorGUI()
        {

            if (Selection.activeGameObject == script.gameObject)
                HiddenInspector();
            else
                Inspector();
        }


        [InitializeOnLoadMethod]
        private static void CheckHideTools()
        {
#if UNITY_EDITOR
            Selection.selectionChanged += new Action(HideTools);
#endif
        }

        private static void HideTools()
        {
            if (UnityEditor.Selection.activeGameObject != null)
                UnityEditor.Tools.hidden = UnityEditor.Selection.activeGameObject.GetComponent<CinematicTakesRig>() != null;
        }

        public static void RepaintInspector()
        {
            var t = typeof(CinematicTakesEditor);
            Editor[] ed = (Editor[])Resources.FindObjectsOfTypeAll<Editor>();
            for (int i = 0; i < ed.Length; i++)
            {
                if (ed[i].GetType() == t)
                {
                    ed[i].Repaint();
                    return;
                }
            }

            
        }

        private void HiddenInspector()
        {
            GUILayout.Label(AssetPreview.GetAssetPreview(CinematicControllerEditor.logo));
            EditorGUILayout.HelpBox("Use the root to edit options of this Script", MessageType.Warning);
        }

        private void Inspector()
        {
            script.RepaintInspector = RepaintInspector;

            var prefabType = PrefabUtility.GetPrefabType(target);
            if (prefabType == PrefabType.Prefab)
            {
                EditorGUILayout.HelpBox("Default values are not editable, create an instance to edit the script", MessageType.Info);
                return;

            }

            if (script.selectedCamera != null && script.defaultRange > script.selectedCamera.farClipPlane)
                EditorGUILayout.HelpBox("Range is bigger than the camera clipping Plane, target might not be visible when it gets detected.", MessageType.Warning);
            DrawPropertiesExcluding(serializedObject, "m_Script", "lookAtMode", "fixedMode", "dollyMode", "steadyMode", "m_selectedCameraFov", "m_distanceToTarget", "m_rigsInRange", "m_selectedRig", "extraInfo");

            ModeOptionsFields();

            ActionFields();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("extraInfo"));
           
            serializedObject.ApplyModifiedProperties();

            
        }


        

        private GUIContent cameraOptionsLabel = new GUIContent("Camera Options");

        private void ModeOptionsFields()
        {
            var prop = "";
            switch (script.defaultCameraMode)
            {
                case CinematicTakes.DefaultCameraMode.LookAt:
                    prop = "lookAtMode";
                    break;
                case CinematicTakes.DefaultCameraMode.Steadicam:
                    prop = "steadyMode";
                    break;
                case CinematicTakes.DefaultCameraMode.Fixed:
                    prop = "fixedMode";
                    break;
                case CinematicTakes.DefaultCameraMode.Dolly:
                    prop = "dollyMode";
                    break;
                default:
                    return;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty(prop), cameraOptionsLabel, true);
        }

        

        

        internal static Vector3 cursor;

        private void ActionFields()
        {
            EditorGUILayout.LabelField("Actions");

            if (script.active)
            {
                if (GUILayout.Button("Add RigPoint in Scene"))
                {
                    EditorUtility.SetDirty(target);
                    Undo.RegisterFullObjectHierarchyUndo(script.gameObject, "Added Rig");
                    script.AddRigPoint(cursor);
                }
            }
            else
            {
                if(script.target == null)
                    EditorGUILayout.HelpBox("A target must be assigned", MessageType.Warning);
                if (script.selectedCamera == null)
                    EditorGUILayout.HelpBox("A camera must be assigned", MessageType.Warning);

                EditorGUILayout.HelpBox("Please activate to use.", MessageType.Warning);

            }

            if (Application.isPlaying && script.rigIsForced)
            {
                if (GUILayout.Button("Release Forced Rig"))
                {
                    script.ReleaseForcedRig();
                }
            }
        }
    }

}