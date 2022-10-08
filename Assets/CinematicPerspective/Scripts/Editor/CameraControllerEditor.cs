using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CinematicPerspective;

namespace CinematicPerspectiveEditor
{
    [CustomEditor(typeof(CameraController0))]
    public class CameraControllerEditor : Editor
    {
        CameraController0 script
        {
            get
            {
                return (CameraController0)target;
            }
        }

        


        public override void OnInspectorGUI()
        {
            GUILayout.Label(AssetPreview.GetAssetPreview(CinematicControllerEditor.logo));
            EditorGUILayout.HelpBox("Use the root to edit options of this Script", MessageType.Warning);            
        }

        
    }
}
