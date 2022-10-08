using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEditor;
using UnityEditor.Events;
using MalbersAnimations.Events;

namespace MalbersAnimations.Selector
{
    [CustomEditor(typeof(SelectorController))]
    public class SelectorControllerEditor : Editor
    {

        SelectorController M;
        SelectorEditor E;
        MonoScript script;

        SerializedProperty RestoreTime, AnimateSelection, SoloSelection, SelectionTime, SelectionCurve, DragSpeed, dragHorizontal, inertia, inertiaTime, minInertiaSpeed, inertiaCurve, debug,
            UseSelectionZone, Hover, EditorIdleAnims, MoveIdle, RotateIdle, ScaleIdle, MoveIdleAnim, ItemRotationSpeed, TurnTableVector, ScaleIdleAnim, LockMaterial, frame_Camera, frame_Multiplier,
            EditorAdvanced, ClickToFocus, ChangeOnEmptySpace, Threshold, EditorShowEvents, OnClickOnItem, /*OnItemFocused,*/ OnIsChangingItem;
        private void OnEnable()
        {
            M = (SelectorController)target;
            E = M.GetComponent<SelectorEditor>();
            script = MonoScript.FromMonoBehaviour((MonoBehaviour)target);

            AnimateSelection = serializedObject.FindProperty("AnimateSelection");
            debug = serializedObject.FindProperty("debug");
            OnIsChangingItem = serializedObject.FindProperty("OnIsChangingItem");
         //   OnItemFocused = serializedObject.FindProperty("OnItemFocused");
            OnClickOnItem = serializedObject.FindProperty("OnClickOnItem");
            EditorShowEvents = serializedObject.FindProperty("EditorShowEvents");
            Threshold = serializedObject.FindProperty("Threshold");
            ClickToFocus = serializedObject.FindProperty("ClickToFocus");
            ChangeOnEmptySpace = serializedObject.FindProperty("ChangeOnEmptySpace");
            EditorAdvanced = serializedObject.FindProperty("EditorAdvanced");
            frame_Multiplier = serializedObject.FindProperty("frame_Multiplier");
            frame_Camera = serializedObject.FindProperty("frame_Camera");
            LockMaterial = serializedObject.FindProperty("LockMaterial");
            ScaleIdleAnim = serializedObject.FindProperty("ScaleIdleAnim");
            TurnTableVector = serializedObject.FindProperty("TurnTableVector");
            ItemRotationSpeed = serializedObject.FindProperty("ItemRotationSpeed");
            MoveIdleAnim = serializedObject.FindProperty("MoveIdleAnim");
            ScaleIdle = serializedObject.FindProperty("ScaleIdle");
            RotateIdle = serializedObject.FindProperty("RotateIdle");
            MoveIdle = serializedObject.FindProperty("MoveIdle");
            EditorIdleAnims = serializedObject.FindProperty("EditorIdleAnims");
            Hover = serializedObject.FindProperty("Hover");
            UseSelectionZone = serializedObject.FindProperty("UseSelectionZone");
            inertiaCurve = serializedObject.FindProperty("inertiaCurve");
            minInertiaSpeed = serializedObject.FindProperty("minInertiaSpeed");
            inertiaTime = serializedObject.FindProperty("inertiaTime");
            inertia = serializedObject.FindProperty("inertia");
            DragSpeed = serializedObject.FindProperty("DragSpeed");
            dragHorizontal = serializedObject.FindProperty("dragHorizontal");
            SelectionTime = serializedObject.FindProperty("SelectionTime");
            SelectionCurve = serializedObject.FindProperty("SelectionCurve");
            SoloSelection = serializedObject.FindProperty("SoloSelection");
            RestoreTime = serializedObject.FindProperty("RestoreTime");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            MalbersEditor.DrawDescription("All the selector actions and animations are managed here");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            {
                MalbersEditor.DrawScript(script);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    M.FocusedItemIndex = EditorGUILayout.IntField(new GUIContent("Focused Item", "Index of the first Item to appear on Focus (Zero Index Based)"), M.FocusedItemIndex);

                    if (M.FocusedItemIndex == -1) EditorGUILayout.HelpBox("-1 Means no item is selected", MessageType.Info);
                    if (M.FocusedItemIndex < -1) M.FocusedItemIndex = -1;

                    EditorGUILayout.PropertyField(RestoreTime, new GUIContent("Restore Time", "Time to restore the previuous item to his original position"));
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginHorizontal();
                {

                    AnimateSelection.boolValue = GUILayout.Toggle(AnimateSelection.boolValue, new GUIContent("Animate Selection", "Animate the selection between items"), EditorStyles.toolbarButton);
                    SoloSelection.boolValue = !AnimateSelection.boolValue;

                    SoloSelection.boolValue = GUILayout.Toggle(SoloSelection.boolValue, new GUIContent("Solo Selection", "Animate the selection between items"), EditorStyles.toolbarButton);
                    AnimateSelection.boolValue = !SoloSelection.boolValue;
                }
                EditorGUILayout.EndHorizontal();

                if (AnimateSelection.boolValue)
                {
                    EditorGUILayout.BeginVertical(MalbersEditor.StyleGreen);
                    EditorGUILayout.HelpBox("The Selector Controller will move and rotate to center the selected Item", MessageType.None);
                    EditorGUILayout.EndVertical();


                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUILayout.PropertyField(SelectionTime, new GUIContent("Selection Time", "Time between the selection among the objects"));

                        if (SelectionTime.floatValue < 0) SelectionTime.floatValue = 0; //Don't put time below zero;
                       
                        if (SelectionTime.floatValue != 0)
                            EditorGUILayout.PropertyField(SelectionCurve, new GUIContent("Selection Curve", "Timing of the selection animation"));


                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PropertyField(DragSpeed, new GUIContent("Drag Speed", "Swipe speed when swiping  :)"));
                            if (DragSpeed.floatValue != 0)
                                dragHorizontal.boolValue = GUILayout.Toggle(dragHorizontal.boolValue, new GUIContent(dragHorizontal.boolValue ? "Horizontal" : "Vertical", "Drag/Swipe type from the mouse/touchpad "), EditorStyles.popup);
                        }
                        EditorGUILayout.EndHorizontal();
                        if (DragSpeed.floatValue == 0) EditorGUILayout.HelpBox("Drag is disabled", MessageType.Info);
                    }
                    EditorGUILayout.EndVertical();

                    if (DragSpeed.floatValue != 0)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                       
                        EditorGUILayout.PropertyField(inertia, new GUIContent("Inertia", "Add inertia when Draging is enabled and the mouse is released"));

                        if (inertia.boolValue)
                        {
                            EditorGUILayout.PropertyField(inertiaTime, new GUIContent("Inertia Time", "The time on inertia when the Drag is released"));
                            EditorGUILayout.PropertyField(minInertiaSpeed, new GUIContent("Inertia Min Speed", "Min Speed to apply inertia"));
                            EditorGUILayout.PropertyField(inertiaCurve, new GUIContent("Inertia Curve", "the Curve for the time inertia"));
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.PropertyField(UseSelectionZone, new GUIContent("Use Selection Zone", "Add inertia when Draging is enabled and the mouse is released"));

                        if (UseSelectionZone.boolValue)
                        {
                            EditorGUI.BeginChangeCheck();

                            EditorGUILayout.PropertyField(serializedObject.FindProperty("ZMinX"), new GUIContent("Min X", "Region enableed for the Dragging/Swapping"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("ZMaxX"), new GUIContent("Max X", "Region enableed for the Dragging/Swapping"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("ZMinY"), new GUIContent("Min Y", "Region enableed for the Dragging/Swapping"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("ZMaxY"), new GUIContent("Max Y", "Region enableed for the Dragging/Swapping"));
                        }
                        EditorGUILayout.EndVertical();
                    }
                }

                if (SoloSelection.boolValue)
                {
                    EditorGUILayout.BeginVertical(MalbersEditor.StyleGreen);
                    EditorGUILayout.HelpBox("The Selector Controller will not move", MessageType.None);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.PropertyField(Hover, new GUIContent("Hover Selection", "Select by hovering the mouse over an item"));
                    EditorGUILayout.EndVertical();
                }
             

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUI.indentLevel++;
                EditorIdleAnims.boolValue = EditorGUILayout.Foldout(EditorIdleAnims.boolValue, "Idle Animations");
                EditorGUI.indentLevel--;

                if (EditorIdleAnims.boolValue)
                {
                    EditorGUILayout.BeginHorizontal();
                    MoveIdle.boolValue = GUILayout.Toggle(MoveIdle.boolValue, new GUIContent("Move", "Repeating moving motion for the focused item"), EditorStyles.miniButton);
                    RotateIdle.boolValue = GUILayout.Toggle(RotateIdle.boolValue, new GUIContent("Rotate", "Turning table for the focused item"), EditorStyles.miniButton);
                    ScaleIdle.boolValue = GUILayout.Toggle(ScaleIdle.boolValue, new GUIContent("Scale", "Repeating scale motion for the focused item"), EditorStyles.miniButton);
                    EditorGUILayout.EndHorizontal();

                    if (MoveIdle.boolValue)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.PropertyField(MoveIdleAnim, new GUIContent("Move Idle", "Idle Move Animation when is on focus"));
                        EditorGUILayout.EndVertical();
                    }
                    if (M.RotateIdle)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.PropertyField(ItemRotationSpeed, new GUIContent("Speed", "How fast the focused Item will rotate"));
                        EditorGUILayout.PropertyField(TurnTableVector, new GUIContent("Rotation Vector", "Choose your desire vector to rotate around"));
                        EditorGUILayout.EndVertical();
                    }

                    if (M.ScaleIdle)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.PropertyField(ScaleIdleAnim, new GUIContent("Scale Idle", "Idle Scale Animation when is on focus"));
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(LockMaterial, new GUIContent("Lock Material", "Material choosed for the locked objects"));
                
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 95;

                if (E && E.SelectorCamera)
                {
                    EditorGUILayout.PropertyField(frame_Camera, new GUIContent("Frame Camera", " Auto Adjust the camera position by the size of the object"), GUILayout.MinWidth(20));
                    if (frame_Camera.boolValue)
                    {
                        EditorGUIUtility.labelWidth = 55;
                        EditorGUILayout.PropertyField(frame_Multiplier, new GUIContent("Multiplier", "Distance Mupltiplier for the camera frame"), GUILayout.MaxWidth(100));
                    }
                }
                    EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();


                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                EditorAdvanced.boolValue = EditorGUILayout.Foldout(EditorAdvanced.boolValue, "Advanced");
              
                if (EditorAdvanced.boolValue)
                {
                    if (!Hover.boolValue)
                        EditorGUILayout.PropertyField(ClickToFocus, new GUIContent("Click to Focus", "If a another item is touched/clicked, focus on it"));
                        
                    EditorGUILayout.PropertyField(ChangeOnEmptySpace, new GUIContent("Change on Empty Space", "If there's a Click/Touch on an empty space change to the next/previous item"));
                    EditorGUILayout.PropertyField(Threshold, new GUIContent("Threshold", "Max Threshold to identify if is a click/touch or a drag/swipe"));
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                EditorShowEvents.boolValue = EditorGUILayout.Foldout(EditorShowEvents.boolValue, "Events");
                EditorGUI.indentLevel--;

                if (EditorShowEvents.boolValue)
                {
                    EditorGUILayout.PropertyField(OnClickOnItem, new GUIContent("On Click/Touch an Item"));
                  //  EditorGUILayout.PropertyField(OnItemFocused, new GUIContent("On Item Focused"));
                    if (AnimateSelection.boolValue)
                      EditorGUILayout.PropertyField(OnIsChangingItem, new GUIContent("Is Changing Item"));
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.PropertyField(debug);
              
            }
            EditorGUILayout.EndVertical();


            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Selector Controller Inspector");
                //EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}