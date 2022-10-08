using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using CinematicPerspective;

namespace CinematicPerspectiveEditor
{
    [CustomEditor(typeof(CinematicTakesRig))]
    public class CinematicTakesRigEditor : Editor
    {
        /// <summary>
        /// Rig
        /// </summary>
        CinematicTakesRig rig
        {
            get
            {
                return (CinematicTakesRig)target;
            }
        }

        /// <summary>
        /// If true, handles will be in camera
        /// </summary>
        public bool EditCamera = true;

        /// <summary>
        /// Range radius for gizmo trigger
        /// </summary>
        /// <param name="rig"></param>
        /// <returns></returns>
        private static float GetRadius(CinematicTakesRig rig)
        {
            return rig.overrideRange ? rig.range : rig.cinematicTakes.defaultRange;
        }

        /// <summary>
        /// True if cinematic controller has chosen this rig
        /// as selected rig
        /// </summary>
        /// <param name="rig"></param>
        /// <returns></returns>
        private static bool isSelected(CinematicTakesRig rig)
        {
            return rig.transform == rig.cinematicTakes.selectedRig;
        }

        /// <summary>
        /// Add method to delegate to be able to trigger camera handles
        /// from hierarchy
        /// </summary>
        protected virtual void OnEnable()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            try
            {
                // Change the handles from the cam to the rig and viceversa when in hierarchy
                if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Space)
                {
                    EditCamera = !EditCamera;
                    SceneView.RepaintAll();
                    // Run only once
                    Event.current.Use();
                }
            }
            catch
            {
                throw;
            }

        }



        /// <summary>
        /// Change handles between rig and camera
        /// depends on the mode
        /// </summary>
        /// <param name="editCamera">yes if handles are in camera</param>
        private void ChangeHandles(bool editCamera)
        {
            if (editCamera)
            {                

                if (rig.selectedRigMode == CinematicTakes.DefaultCameraMode.Steadicam)
                {
                    if (Tools.current == Tool.Move)
                    {
                        EditorGUI.BeginChangeCheck();
                        var cameraOffset = Handles.DoPositionHandle(rig.transform.position +
                            rig.steadyMode.cameraOffset,
                            Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : Quaternion.Euler(rig.cameraRotation)
                            ) - rig.transform.position;

                        if (EditorGUI.EndChangeCheck())
                        {
                            EditorUtility.SetDirty(target);
                            Undo.RecordObject(rig, "Changed camera offset");
                            rig.steadyMode.cameraOffset = cameraOffset;
                        }
                    }
                    else if (Tools.current == Tool.Rotate)
                    {
                        EditorGUI.BeginChangeCheck();
                        var cameraRotation = Handles.DoRotationHandle(
                            Quaternion.Euler(rig.steadyMode.cameraRotation), rig.transform.position +
                            rig.steadyMode.cameraOffset
                            ).eulerAngles;
                        if (EditorGUI.EndChangeCheck())
                        {
                            EditorUtility.SetDirty(target);
                            Undo.RecordObject(rig, "Changed camera rotation");
                            rig.steadyMode.cameraRotation = cameraRotation;
                        }
                    }
                        
                }
                else
                {

                    if (Tools.current == Tool.Move && rig != null)
                    {
                        EditorGUI.BeginChangeCheck();
                        var cameraPosition = Handles.PositionHandle(rig.transform.position + rig.cameraPositionOffset,
                        Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : Quaternion.Euler(rig.cameraRotation)
                        ) - rig.transform.position;
                        if (EditorGUI.EndChangeCheck())
                        {
                            EditorUtility.SetDirty(target);
                            Undo.RecordObject(rig, "Changed camera offset");
                            rig.cameraPositionOffset = cameraPosition;
                        }
                    }
                    else if (Tools.current == Tool.Rotate)
                    {
                        EditorGUI.BeginChangeCheck();
                        var cameraRotation = Handles.DoRotationHandle(
                            Quaternion.Euler(rig.cameraRotation), rig.transform.position +
                            rig.cameraPositionOffset
                            ).eulerAngles;
                        if (EditorGUI.EndChangeCheck())
                        {
                            EditorUtility.SetDirty(target);
                            Undo.RecordObject(rig, "Changed camera rotation");
                            rig.cameraRotation = cameraRotation;
                        }
                    }
                }
            }
            else
            {
                if (Tools.current == Tool.Move)
                {
                    Tools.hidden = true;
                }

                EditorGUI.BeginChangeCheck();

                var position = Handles.DoPositionHandle(
                    rig.transform.position,
                    Tools.pivotRotation == PivotRotation.Global ?
                    Quaternion.identity : rig.transform.rotation
                    );

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(rig, target.name + "Moved");
                    rig.cameraPositionOffset -= position - rig.transform.position;
                    rig.transform.position = position;
                }
            }
        }

        /// <summary>
        /// Draw range gizmo for the selected rig
        /// </summary>
        /// <param name="rig"></param>
        private static void RangeGizmo(CinematicTakesRig rig, float alpha = 1)
        {
            var radius = GetRadius(rig);
            Handles.color = new Color(0, 0, 0, .2f * alpha);
            Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), rig.transform.position, rig.transform.rotation, radius * 2f, EventType.Layout | EventType.Repaint);
            Handles.color = new Color(1, 1, 1, .10f * alpha);
            Handles.DrawSolidDisc(rig.transform.position, -rig.transform.up, radius);
            Gizmos.color = new Color(1, 1, 1, .20f * alpha);
            Gizmos.DrawWireSphere((Vector3)rig.transform.position, radius);
        }

        /// <summary>
        /// Camera Gizmo
        /// IMPORTANT: if the package folder is not under "Assets/" 
        /// please change this function accordingly
        /// </summary>
        [InitializeOnLoadMethod]
        public static void FindCameraGizmo()
        {
            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/CinematicPerspective/Dependencies/CameraGizmo.asset");

            gizmoMesh = new Mesh()
            {
                name = mesh.name,
                vertices = mesh.vertices,
                triangles = mesh.triangles,
                normals = mesh.normals,
                bounds = mesh.bounds
            };


        }

        /// <summary>
        /// Draw camera gizmo
        /// </summary>
        /// <param name="rig"></param>
        public static void CameraGizmo(CinematicTakesRig rig)
        {
            var size = HandleUtility.GetHandleSize(rig.cameraPositionOffset + rig.transform.position) * rig.cinematicTakes.cameraGizmoSize;
            gizmoMesh.RecalculateBounds();
            var vertices = gizmoMesh.vertices;
            for (int i = 0; i < gizmoMesh.vertexCount; i++)
            {
                vertices[i] *= Mathf.Abs(size / (gizmoMesh.bounds.min - gizmoMesh.bounds.max).magnitude);

            }
            gizmoMesh.vertices = vertices;
            gizmoMesh.RecalculateBounds();
            Gizmos.color = Color.cyan * .25f;
            
            Gizmos.color = Color.cyan;
            if (rig.selectedRigMode == CinematicTakes.DefaultCameraMode.Steadicam)
            {
                Gizmos.DrawLine(rig.transform.position + rig.steadyMode.cameraOffset, rig.transform.position);
                Gizmos.DrawMesh(gizmoMesh, rig.transform.position + rig.steadyMode.cameraOffset, Quaternion.Euler(rig.steadyMode.cameraRotation));
            }
            else
            {
                Gizmos.DrawLine(rig.transform.position + rig.cameraPositionOffset, rig.transform.position);
                Gizmos.DrawMesh(gizmoMesh, rig.transform.position + rig.cameraPositionOffset, Quaternion.Euler(rig.cameraRotation));
            }
        }   

        /// <summary>
        /// Rig + Range Gizmo
        /// </summary>
        /// <param name="rig"></param>
        /// <param name="gizmoType"></param>
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Pickable)]
        public static void drawRigGizmos(CinematicTakesRig rig, GizmoType gizmoType)
        {
            gizmoMesh = null;
            if (gizmoMesh == null) FindCameraGizmo();

            CameraGizmo(rig);

            if (!rig.cinematicTakes.active)
                return;

            // In case a rig has been added or deleted
            rig.cinematicTakes.GetRigs();

            var overrideMode = rig.rigCameraMode != CinematicTakesRig.OverrideCameraMode.Default;
            var mode = !overrideMode ?  rig.cinematicTakes.defaultCameraMode : (CinematicTakes.DefaultCameraMode)rig.rigCameraMode;
            RigGizmo(mode, rig);
            RangeGizmo(rig);

            if (Selection.activeGameObject.GetComponent<CinematicTakesRig>() != null)
            {
                rig.cinematicTakes.GetRigs();
                foreach (Transform r in rig.cinematicTakes.rigs)
                {
                    if (r != null && r != rig.transform)
                    {
                        var takerig = r.GetComponent<CinematicTakesRig>();
                        float tempRange = takerig.range;
                        if (!takerig.overrideRange)
                            tempRange = takerig.cinematicTakes.defaultRange;

                        Handles.color = new Color(0, 0, 0, .15f);
                        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), r.transform.position, r.transform.rotation, tempRange * 2f, EventType.Layout | EventType.Repaint);
                    }
                }
            }

            var selected = Selection.gameObjects.Where(g => rig.cinematicTakes.rigs.Contains(g.transform)).ToArray();
            if (selected.Length > 0)
            {
                foreach (var arig in rig.cinematicTakes.rigs)
                {
                    if (!selected.Contains(arig.gameObject))
                        RangeGizmo(arig.GetComponent<CinematicTakesRig>(), .3f);
                }
            }
            

        }

        /// <summary>
        /// Rig Gizmos
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="rig"></param>
        private static void RigGizmo(CinematicTakes.DefaultCameraMode mode, CinematicTakesRig rig)
        {
            switch (mode)
            {
                case CinematicTakes.DefaultCameraMode.LookAt:
                    LookAtGizmo(rig);
                    break;
                case CinematicTakes.DefaultCameraMode.Steadicam:
                    SteadyGizmo(rig);
                    break;
                case CinematicTakes.DefaultCameraMode.Fixed:
                    FixedGizmo(rig);
                    break;
                case CinematicTakes.DefaultCameraMode.Dolly:
                    DollyGizmo(rig);
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Steady mode gizmo
        /// </summary>
        /// <param name="rig"></param>
        private static void SteadyGizmo(CinematicTakesRig rig)
        {
            if (Application.isPlaying && isSelected(rig) && rig.cinematicTakes.selectedCamera != null)
            {
                Camera rigCam = rig.cinematicTakes.selectedCamera;
                FOVGizmo(rig, rigCam.transform.position, rigCam.transform.rotation, rigCam.fieldOfView);
            }
            else
            {
                FOVGizmo(rig, rig.transform.position + rig.steadyMode.cameraOffset, Quaternion.Euler(rig.steadyMode.cameraRotation), rig.steadyMode.steadyZoomLevel);
            }
        }

        /// <summary>
        /// Fixed mode gizmo
        /// </summary>
        /// <param name="rig"></param>
        private static void FixedGizmo(CinematicTakesRig rig)
        {           
            var fixedMode = rig.overridesMode ? rig.fixedMode : rig.cinematicTakes.fixedMode;
            FOVGizmo(rig, rig.transform.position + rig.cameraPositionOffset, Quaternion.Euler(rig.cameraRotation),  fixedMode.zoomLevel);
        }

        /// <summary>
        /// Dolly mode gizmo
        /// </summary>
        /// <param name="rig"></param>
        private static void DollyGizmo(CinematicTakesRig rig)
        {
            var dollyMode = rig.overridesMode ? rig.dollyMode : rig.cinematicTakes.dollyMode;
            var rigCam = rig.cinematicTakes.selectedCamera;
            var offsetGizmo = dollyMode.reverseDirection || (Application.isPlaying && isSelected(rig)) ? Vector3.zero : dollyMode.destination;
            if (dollyMode.autoZoom)
            {
                if (Application.isPlaying && isSelected(rig) && isSelected(rig))
                    MinMaxFOVGizmo(rig, rigCam.transform.position, rigCam.transform.rotation, dollyMode.minAutoZoom, dollyMode.maxAutoZoom, offsetGizmo);// + rig.cameraPositionOffset);
                else
                    MinMaxFOVGizmo(rig, rig.transform.position, Quaternion.Euler(rig.cameraRotation), dollyMode.minAutoZoom, dollyMode.maxAutoZoom, offsetGizmo + rig.cameraPositionOffset);
            }
            if (Application.isPlaying && isSelected(rig))
                FOVGizmo(rig, rigCam.transform.position + offsetGizmo, rigCam.transform.rotation, rigCam.fieldOfView);
            else
                FOVGizmo(rig, rig.transform.position + offsetGizmo + rig.cameraPositionOffset, Quaternion.Euler(rig.cameraRotation), dollyMode.manualZoomLevel);

            if (Selection.activeGameObject != rig.gameObject)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(rig.transform.position + rig.cameraPositionOffset, rig.transform.position + rig.cameraPositionOffset + dollyMode.destination);
                Gizmos.DrawSphere(rig.transform.position + dollyMode.destination, .1f);
            }
        }

        /// <summary>
        /// LookAt mode Gizmo
        /// </summary>
        /// <param name="rig"></param>
        private static void LookAtGizmo(CinematicTakesRig rig)
        {

            var lookAtMode = rig.overridesMode ? rig.lookAtMode : rig.cinematicTakes.lookAtMode;
            var rigCam = rig.overrideCamera != null ? rig.overrideCamera : rig.cinematicTakes.selectedCamera;
            if (lookAtMode.autoZoom)
            {
                if (Application.isPlaying && isSelected(rig))
                    MinMaxFOVGizmo(rig, rigCam.transform.position, rigCam.transform.rotation, lookAtMode.minZoom, lookAtMode.maxZoom);
                else
                    MinMaxFOVGizmo(rig, rig.transform.position, Quaternion.Euler(rig.cameraRotation), lookAtMode.minZoom, lookAtMode.maxZoom, rig.cameraPositionOffset);
            }

            if (Application.isPlaying && isSelected(rig))
            {
                FOVGizmo(rig, rigCam.transform.position, rigCam.transform.rotation, rig.cinematicTakes.selectedCamera.fieldOfView);
            }
            else if(!lookAtMode.autoZoom)
                FOVGizmo(rig, rig.transform.position + rig.cameraPositionOffset, Quaternion.Euler(rig.cameraRotation), lookAtMode.staticZoom);
        }

        /// <summary>
        /// Min Max FOV disc gizmo
        /// </summary>
        /// <param name="rig"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="offsetGizmo"></param>
        public static void MinMaxFOVGizmo(CinematicTakesRig rig, Vector3 position, Quaternion rotation, float min, float max, Vector3 offsetGizmo = default(Vector3))
        {


            var radius = GetRadius(rig);

            //Min field of view Gizmo
            Handles.color = new Color(.27f, 1f, .45f, .3f);
            Handles.DrawSolidArc(position + offsetGizmo, rotation * Vector3.up, rotation * Vector3.forward, min / 2f, radius);
            Handles.DrawSolidArc(position + offsetGizmo, rotation * Vector3.up, rotation * Vector3.forward, -min / 2f, radius);

            //Max field of View Gizmo
            Handles.color = new Color(0.56f, .76f, .83f, .15f);
            Handles.DrawSolidArc(position + offsetGizmo, rotation * Vector3.up, rotation * Vector3.forward, max / 2f, radius);
            Handles.DrawSolidArc(position + offsetGizmo, rotation * Vector3.up, rotation * Vector3.forward, -max / 2f, radius);
        } 

        /// <summary>
        /// Fixed fov gizmo
        /// </summary>
        /// <param name="rig"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="FOVValue"></param>
        public static void FOVGizmo(CinematicTakesRig rig, Vector3 position, Quaternion rotation, float FOVValue)
        {
            var radius = GetRadius(rig);
            var rigCam = rig.cinematicTakes.selectedCamera;
            //Current field of view Gizmo
            Handles.color = new Color(0f, .79f, .7f, .3f);

            if (Application.isPlaying && isSelected(rig))
            {
                Handles.DrawSolidArc(position, rotation * Vector3.up, rigCam.transform.rotation * Vector3.forward, FOVValue / 2f, radius);
                Handles.DrawSolidArc(position, rotation * Vector3.up, rigCam.transform.rotation * Vector3.forward, -FOVValue / 2f, radius);

            }
            else
            {
                Handles.DrawSolidArc(position, rotation * Vector3.up, rotation * Vector3.forward, FOVValue / 2f, radius);
                Handles.DrawSolidArc(position, rotation * Vector3.up, rotation * Vector3.forward, -FOVValue / 2f, radius);

            }
        }

        Vector3 lastCameraPos;
        Vector3 lastCameraRot;
        private GUIContent cameraOptionsLabel = new GUIContent("Camera Options");
        private static Mesh gizmoMesh;

        private void OnSceneGUI()
        {
            // Change the handles from the cam to the rig and viceversa when in sceneview
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
                EditCamera = !EditCamera;

            // Case when rig is added, the cam handle will be selected by default
            if (rig.editCamera)
            {
                EditCamera = true;
                rig.editCamera = false;
            }

            ChangeHandles(EditCamera);

            var radius = !rig.overrideRange ? rig.cinematicTakes.defaultRange : rig.range;
            var rangePosition = rig.transform.position + Vector3.forward * radius;

            EditorGUI.BeginChangeCheck();
            var handleSize = HandleUtility.GetHandleSize(rangePosition) * 1f;
            Handles.color = Color.white;
            var range = Handles.ScaleValueHandle(radius, rangePosition, Quaternion.identity, handleSize, Handles.CircleHandleCap, 1);
            if (EditorGUI.EndChangeCheck())
            {
                rig.overrideRange = rig.range != range;
                rig.range = range;
            }
            var rangePosition2 = rig.transform.position * 2 - rangePosition;
            EditorGUI.BeginChangeCheck();

            Handles.color = Color.white;
            var forRange2 = Handles.FreeMoveHandle(rangePosition2, Quaternion.LookRotation(rangePosition2 - rig.transform.position, Vector3.up), .05f * rig.range, Vector3.one, Handles.SphereHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                var range2 = Vector3.Distance(forRange2, rig.transform.position);
                rig.overrideRange = rig.range != range2;
                rig.range = range2;
            }

            //target of dolly
            var overrideMode = rig.rigCameraMode != CinematicTakesRig.OverrideCameraMode.Default;
            var defaultCameraMode = !overrideMode ? rig.cinematicTakes.defaultCameraMode : (CinematicTakes.DefaultCameraMode)rig.rigCameraMode;
            if (defaultCameraMode == CinematicTakes.DefaultCameraMode.Dolly)
            {
                var dollyMode = overrideMode ? rig.dollyMode : rig.cinematicTakes.dollyMode;
                Handles.color = Color.red;
                dollyMode.destination = Handles.FreeMoveHandle(rig.transform.position + rig.cameraPositionOffset + dollyMode.destination, Quaternion.identity, 1f, Vector3.one, Handles.SphereHandleCap) - rig.transform.position - rig.cameraPositionOffset;
                Handles.DrawAAPolyLine(rig.transform.position + rig.cameraPositionOffset, rig.transform.position + rig.cameraPositionOffset + dollyMode.destination);
            }
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label(AssetPreview.GetAssetPreview(CinematicControllerEditor.logo));

            var hideVars = "m_Script&lookAtMode&fixedMode&dollyMode&steadyMode";

            if (rig.overrideRange)
            {
                if (rig.range > rig.cinematicTakes.selectedCamera.farClipPlane)
                    EditorGUILayout.HelpBox("Range is bigger than the camera clipping Plane, target might not be visible when it gets detected.", MessageType.Warning);
            }
            else
            {
                hideVars += "&range";
            }

            if (rig.selectedRigMode == CinematicTakes.DefaultCameraMode.Steadicam)
            {
                if (rig.cameraPositionOffset != rig.steadyMode.cameraOffset)
                {
                    if (lastCameraPos == rig.cameraPositionOffset)
                        rig.cameraPositionOffset = rig.steadyMode.cameraOffset;
                    if (lastCameraPos == rig.steadyMode.cameraOffset)
                        rig.steadyMode.cameraOffset = rig.cameraPositionOffset;

                }

                if (rig.cameraRotation != rig.steadyMode.cameraRotation)
                {
                    if (lastCameraRot == rig.cameraRotation)
                        rig.cameraRotation = rig.steadyMode.cameraRotation;
                    if (lastCameraRot == rig.steadyMode.cameraRotation)
                        rig.steadyMode.cameraRotation = rig.cameraRotation;
                }

                lastCameraPos = rig.cameraPositionOffset;
                lastCameraRot = rig.cameraRotation;
            }

            DrawPropertiesExcluding(serializedObject, hideVars.Split('&'));

            ModeOptionsFields();

            GUILayout.Label("Actions");
            if (GUILayout.Button("< Return to parent"))
            {
                Selection.activeObject = rig.cinematicTakes.gameObject.transform.parent.gameObject;
            }

            if (Application.isPlaying)
            {
                if (rig.cinematicTakes.rigIsForced && rig.cinematicTakes.selectedRig == rig.transform)
                {
                    if (GUILayout.Button("Relase Forced Active"))
                    {
                        rig.cinematicTakes.ReleaseForcedRig();
                    }
                }
                else if(GUILayout.Button("Force Active"))
                {
                    rig.cinematicTakes.ForceActiveRig(rig);
                }
            }

            if (GUILayout.Button("Toggle Handles (Rig<->Camera)"))
            {
                EditCamera = !EditCamera;
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Move & Align to View (Full Rig)"))
            {
                EditorUtility.SetDirty(target);
                Undo.RegisterFullObjectHierarchyUndo(rig.gameObject, "Aligned Full Rig");
                rig.MoveAndAlign();
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Move & Align to View (Just Camera)"))
            {
                EditorUtility.SetDirty(target);
                Undo.RegisterFullObjectHierarchyUndo(rig.gameObject, "Aligned Camera Rig");
                rig.MoveAndAlignCamera();
                SceneView.RepaintAll();
            }

            if (GUILayout.Button("Delete Rig Point"))
            {
                EditorUtility.SetDirty(target);
                Undo.RegisterFullObjectHierarchyUndo(rig.gameObject, "Rig Deleted");
                rig.DeleteRig();
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Default Transitions Options"))
            {
                rig.CameraTransitions.SetOptions();
                SceneView.RepaintAll();
            }
            else
            {
                // If the rig has been deleted this has no sense
                serializedObject.ApplyModifiedProperties();
            } 
        }

        /// <summary>
        /// Draw Camera Fields
        /// </summary>
        private void ModeOptionsFields()
        {
            var prop = "";
            switch (rig.rigCameraMode)
            {
                case CinematicTakesRig.OverrideCameraMode.LookAt:
                    prop = "lookAtMode";
                    break;
                case CinematicTakesRig.OverrideCameraMode.Steadicam:
                    prop = "steadyMode";
                    break;
                case CinematicTakesRig.OverrideCameraMode.Fixed:
                    prop = "fixedMode";
                    break;
                case CinematicTakesRig.OverrideCameraMode.Dolly:
                    prop = "dollyMode";
                    break;
                default:
                    return;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty(prop), cameraOptionsLabel, true);
        }

    }
}
