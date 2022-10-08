using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using CinematicPerspective;

namespace CinematicPerspectiveEditor
{
    [CustomEditor(typeof(CinematicController))]
    public class CinematicControllerEditor : Editor
    {
        CinematicController script
        {
            get
            {
                return (CinematicController)target;
            }
        }

        CinematicTakes takes;

        CinematicTakesRig[] rigs
        {
            get
            {
                return script.GetComponentsInChildren<CinematicTakesRig>();
            }
        }        

        private static Texture2D _logo;

        internal static Texture2D logo
        {
            get
            {
                return _logo = _logo ?? AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CinematicPerspective/Dependencies/logo.png");
            }
        }

        public override void OnInspectorGUI()
        {
            takes = takes ?? script.GetComponentInChildren<CinematicTakes>();

            GUILayout.Label(logo, GUILayout.Width(EditorGUIUtility.currentViewWidth / 2), GUILayout.Height(EditorGUIUtility.currentViewWidth / 8));

            var editor = (CinematicTakesEditor)CreateEditor(takes);
            editor.OnInspectorGUI();
            SetForceActive();
        }

        private void SetForceActive()
        {
            if (!Application.isPlaying) return;
            GUILayout.Label("Force Active Rig");
            foreach (var rig in cinematicTakes.rigs)
            {

                if (rig == cinematicTakes.selectedRig)
                {
                    var color = GUI.backgroundColor;
                    GUI.backgroundColor = Color.cyan;

                    var currentIsForced = cinematicTakes.rigIsForced && cinematicTakes.selectedRig == rig.transform; 
                    if (GUILayout.Button(currentIsForced ? "Forced! " + rig.name : rig.name))
                    {
                        cinematicTakes.ForceActiveRig(rig.GetComponent<CinematicTakesRig>());
                    }

                    GUI.backgroundColor = color;

                }
                else
                {
                    if (GUILayout.Button(rig.name))
                    {
                        cinematicTakes.ForceActiveRig(rig.GetComponent<CinematicTakesRig>());
                    }
                }

                
            }
        }


        private void OnSceneGUI()
        {
            if(takes != null && takes.rigToCollider)
                DrawCursor();
        }

        CinematicTakes _takes;

        CinematicTakes cinematicTakes
        {
            get
            {
                return _takes = _takes ?? script.GetComponentInChildren<CinematicTakes>();
            }
        }

        void DrawCursor()
        {
            var cursor = CinematicTakesEditor.cursor;
            Handles.color = Color.green;
            var size = HandleUtility.GetHandleSize(cursor) * .1f;
            Handles.FreeMoveHandle(cursor, Quaternion.identity, size, Vector3.one * .1f, Handles.SphereHandleCap);
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    CinematicTakesEditor.cursor = hit.point;
                }
            }
        }



    }
}
