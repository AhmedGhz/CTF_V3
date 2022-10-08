using UnityEngine;
using System.Collections;
using UnityEditor;
using MalbersAnimations.Utilities;

namespace MalbersAnimations.Selector
{
    [CustomEditor(typeof(MItem)), CanEditMultipleObjects]
    public class MItemEditor : Editor
    {
        MItem M;
        MonoScript script;
        bool EventHelp;

        private void OnEnable()
        {
            M = (MItem)target;
            script = MonoScript.FromMonoBehaviour((MonoBehaviour)target);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            MalbersEditor.DrawDescription("Items Properties used on the Selector Editor & Controller");

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
                {
                    MalbersEditor.DrawScript(script);

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.TextField("Name", M.name, EditorStyles.label);
                        EditorGUI.EndDisabledGroup();
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("description"), new GUIContent("Description"));
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("value"), new GUIContent("Value", "How many COINS is this item worth"), GUILayout.MinWidth(50));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("amount"), new GUIContent("Amount", "How many of these items are"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("locked"), new GUIContent("Locked", "True if this item is locked"));
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("originalItem"), new GUIContent("Original Item", "The Object you want to instantiate when is selected"));
                    }
                    EditorGUILayout.EndVertical();

                    if (M.GetComponentInChildren<Animator>())
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("CustomAnimation"), new GUIContent("Custom Animation", "Plays an specific animation when the PlayAnimation is called"));
                        }
                        EditorGUILayout.EndVertical();
                    }


                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUI.indentLevel++;
                            M.EditorShowEvents = EditorGUILayout.Foldout(M.EditorShowEvents, "Events");
                            EventHelp = GUILayout.Toggle(EventHelp, "?", EditorStyles.miniButton, GUILayout.MaxWidth(16));
                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.EndHorizontal();

                        if (M.EditorShowEvents)
                        {
                            if (EventHelp)
                                EditorGUILayout.HelpBox("On Selected: Invoked when an Item is selected using the method: 'SelectItem()'\n\n On Focused: Invoked when the item is focused(Highlighted)\n\nOn Locked: Invoked when the item is Locked \n\nOn Unlocked: Invoked when the item is unlocked(purchased) \n\nOn Ammount Changed: Invoked if the quantity of item has changed", MessageType.None);

                            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnSelected"), new GUIContent("On Selected"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnFocused"), new GUIContent("On Focused"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnLocked"), new GUIContent("On Locked"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnUnlocked"), new GUIContent("On Unlocked"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnAmountChanged"), new GUIContent("On Amount Changed"));
                        }

                    }
                    EditorGUILayout.EndVertical();

                }
                EditorGUILayout.EndVertical();
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "MItem Input Inspector");
                //EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}