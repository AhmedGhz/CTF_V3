using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.EventSystems;

namespace MalbersAnimations.Selector
{
    [CustomEditor(typeof(SelectorEditor))]
    public class SelectorEditorEditor : Editor
    {
        SelectorEditor M;
        MonoScript script;

        SerializedProperty SelectorCamera, WorldCamera, CameraOffset, CameraPosition, CameraRotation, SelectorType1, ItemRendererType, distance, RadialAxis,
            UseWorld, LookRotation, LinearX, LinearY, LinearZ, Grid, GridWidth, GridHeight, RotationOffSet, Items;
        private void OnEnable()
        {
            M = (SelectorEditor)target;
            script = MonoScript.FromMonoBehaviour((MonoBehaviour)target);

            Items = serializedObject.FindProperty("Items");
            SelectorCamera = serializedObject.FindProperty("SelectorCamera");
            WorldCamera = serializedObject.FindProperty("WorldCamera");
            CameraOffset = serializedObject.FindProperty("CameraOffset");
            CameraPosition = serializedObject.FindProperty("CameraPosition");
            CameraRotation = serializedObject.FindProperty("CameraRotation");
            SelectorType1 = serializedObject.FindProperty("SelectorType");
            ItemRendererType = serializedObject.FindProperty("ItemRendererType");
            distance = serializedObject.FindProperty("distance");
            RadialAxis = serializedObject.FindProperty("RadialAxis");
            UseWorld = serializedObject.FindProperty("UseWorld");
            LookRotation = serializedObject.FindProperty("LookRotation");
            LinearX = serializedObject.FindProperty("LinearX");
            LinearY = serializedObject.FindProperty("LinearY");
            LinearZ = serializedObject.FindProperty("LinearZ");
            Grid = serializedObject.FindProperty("Grid");
            GridWidth = serializedObject.FindProperty("GridWidth");
            GridHeight = serializedObject.FindProperty("GridHeight");
            RotationOffSet = serializedObject.FindProperty("RotationOffSet");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Manage the distribution of all Items\nItems are always child of this gameObject", MessageType.None);
            EditorGUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);

            MalbersEditor.DrawScript(script);


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(SelectorCamera, new GUIContent("Selector Camera", "Camera for the selector"));
            if (EditorGUI.EndChangeCheck())AddRaycaster();
                


            if (SelectorCamera.objectReferenceValue != null)
            {
            EditorGUILayout.PropertyField(WorldCamera, new GUIContent("World Spacing", "The camera is no longer child of the Selector\nIt should be child of the Main Camera, Use this when the the Selector on the hands of a character, on VR mode, etc"));

                if (!WorldCamera.boolValue)
                {

                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(CameraOffset, new GUIContent("Offset", "Camera Forward Offset"));
                    EditorGUILayout.PropertyField(CameraPosition, new GUIContent("Position", "Camera Position Offset"));
                    EditorGUILayout.PropertyField(CameraRotation, new GUIContent("Rotation", "Camera Rotation Offset"));
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(target, "Change Camera Values");
                        M.SetCamera();
                        EditorUtility.SetDirty(target);
                    }
                }
            }

            if (M.transform.childCount != M.Items.Count)
            {
                M.UpdateItemsList();
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(SelectorType1, new GUIContent("Selector Type", "Items Distribution"));

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(ItemRendererType, new GUIContent("Items Type", "Items Renderer Type"));
            if (EditorGUI.EndChangeCheck())
                AddRaycaster();

            EditorGUILayout.EndVertical();
            GUIContent DistanceName = new GUIContent("Radius", "Radius of the selector");
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            var selType =(SelectorType) SelectorType1.enumValueIndex;

            switch (selType)
            {
                case SelectorType.Radial:
                    EditorGUILayout.PropertyField(distance, DistanceName);
                    EditorGUILayout.PropertyField(RadialAxis, new GUIContent("Axis", "Radial Axis"));
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(UseWorld, new GUIContent("Use World Rotation", "The Items will keep the same initial Rotation"));
                    EditorGUILayout.PropertyField(LookRotation, new GUIContent("Use Look Rotation", "The items will look to the center of the selector"));
                    EditorGUILayout.EndHorizontal();
                    break;
                case SelectorType.Linear:
                    DistanceName = new GUIContent("Distance", "Distance between objects");
                    LookRotation.boolValue = false;
                  //  EditorUtility.SetDirty(target);
                    serializedObject.ApplyModifiedProperties();
                    EditorGUILayout.PropertyField(distance, DistanceName);
                    LinearX.floatValue = EditorGUILayout.Slider(new GUIContent("Linear X"), LinearX.floatValue, -1, 1);
                    LinearY.floatValue = EditorGUILayout.Slider(new GUIContent("Linear Y"), LinearY.floatValue, -1, 1);
                    LinearZ.floatValue = EditorGUILayout.Slider(new GUIContent("Linear Z"), LinearZ.floatValue, -1, 1);
                    UseWorld.boolValue = false;
                    break;
                case SelectorType.Grid:
                    UseWorld.boolValue = false;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 65;
                    EditorGUILayout.PropertyField(Grid, new GUIContent("Columns", "Ammount of the Columns for the Grid. the Rows are given my the ammount of Items"), GUILayout.MinWidth(100));
                    EditorGUIUtility.labelWidth = 15;
                    EditorGUILayout.PropertyField(GridWidth, new GUIContent("W", "Width"), GUILayout.MinWidth(50));
                    EditorGUILayout.PropertyField(GridHeight, new GUIContent("H", "Height"), GUILayout.MinWidth(50));
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();
                    break;
                case SelectorType.Custom:

                    if (GUILayout.Button(new GUIContent("Store Item Location", "Store the Initial Pos/Rot/Scale of every Item")))
                    {
                        M.StoreCustomLocation();
                    }
                    break;
                default:
                    break;
            }

            EditorGUILayout.PropertyField(RotationOffSet, new GUIContent("Rotation Offset", "Offset for the Rotation on the Radial Selector"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Editor Selector Changed");
                M.LinearVector = new Vector3(M.LinearX, M.LinearY, M.LinearZ);  //Set the new linear vector

                if (M.SelectorType == SelectorType.Custom)
                {
                    if (EditorUtility.DisplayDialog("Use Current Distribution", "Do you want to save the current distribution as a Custom Type Selector", "Yes", "No"))
                    {
                        M.StoreCustomLocation();
                    }
                }

                M.ItemsLocation();
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(Items, new GUIContent("Items"),true);
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            GUILayout.BeginHorizontal();

            GUILayout.EndHorizontal();

 
            EditorGUILayout.EndVertical();


            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(target, "Editor Values Changed");
               // EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddRaycaster()
        {
            if (SelectorCamera.objectReferenceValue != null)
            {
                DestroyImmediate(M.SelectorCamera.GetComponent<BaseRaycaster>());

                var IRenderType = (ItemRenderer) ItemRendererType.enumValueIndex;

                switch (IRenderType)
                {
                    case ItemRenderer.Mesh:
                        PhysicsRaycaster Ph = M.SelectorCamera.gameObject.AddComponent<PhysicsRaycaster>();
                        Ph.eventMask = 32;
                        break;
                    case ItemRenderer.Sprite:
                        Physics2DRaycaster Ph2d = M.SelectorCamera.gameObject.AddComponent<Physics2DRaycaster>();
                        Ph2d.eventMask = 32;
                        break;
                    case ItemRenderer.Canvas:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}