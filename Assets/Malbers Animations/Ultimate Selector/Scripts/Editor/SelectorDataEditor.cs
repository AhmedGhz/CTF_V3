using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEditor;
using UnityEditor.Events;
using MalbersAnimations.Events;

namespace MalbersAnimations.Selector
{
    [CustomEditor(typeof(SelectorData))]
    public class SelectorDataEditor : Editor
    {
        SerializedProperty usePlayerPref, PlayerPrefKey,Save, FocusedItem, UseMaterialChanger, UseActiveMesh, Coins, RestoreCoins, Locked, RestoreLocked, ItemsAmount, RestoreItemsAmount,
            RestoreActiveMeshIndex, ActiveMeshIndex;
        SelectorData M;
        MonoScript script;


        private void OnEnable()
        {
            M = (SelectorData)target;
            script = MonoScript.FromScriptableObject((ScriptableObject)target);

            Save = serializedObject.FindProperty("Save");
            ActiveMeshIndex = Save.FindPropertyRelative("ActiveMeshIndex");
            RestoreActiveMeshIndex = Save.FindPropertyRelative("RestoreActiveMeshIndex");
            ItemsAmount = Save.FindPropertyRelative("ItemsAmount");
            RestoreItemsAmount = Save.FindPropertyRelative("RestoreItemsAmount");
            Locked = Save.FindPropertyRelative("Locked");
            RestoreLocked = Save.FindPropertyRelative("RestoreLocked");


            usePlayerPref = serializedObject.FindProperty("usePlayerPref");
            PlayerPrefKey = serializedObject.FindProperty("PlayerPrefKey");

            FocusedItem = Save.FindPropertyRelative("FocusedItem");
            UseMaterialChanger = Save.FindPropertyRelative("UseMaterialChanger");
            UseActiveMesh = Save.FindPropertyRelative("UseActiveMesh");

            Coins = Save.FindPropertyRelative("Coins");
            RestoreCoins = Save.FindPropertyRelative("RestoreCoins");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            MalbersEditor.DrawDescription("Data values to save on each Selector");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            {
                MalbersEditor.DrawScript(script);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.PropertyField(usePlayerPref);
                EditorGUILayout.PropertyField(PlayerPrefKey);
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUIStyle newGuiStyle = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };

                EditorGUILayout.PropertyField(FocusedItem);
                EditorGUILayout.BeginHorizontal();
                UseMaterialChanger.boolValue = GUILayout.Toggle(UseMaterialChanger.boolValue, new GUIContent("Material Changer","The items on the selector have the Material Changer component"), EditorStyles.miniButton);
                UseActiveMesh.boolValue = GUILayout.Toggle(UseActiveMesh.boolValue, new GUIContent("Active Mesh", "The items on the selector have the Active Mesh component"), EditorStyles.miniButton);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();


                //-----------------

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Coins", newGuiStyle);
                    EditorGUILayout.BeginVertical(EditorStyles.textField); EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.indentLevel++;
                    EditorGUIUtility.labelWidth = 75;
                    EditorGUILayout.PropertyField(Coins, new GUIContent("Current"),  GUILayout.MinWidth(30));
                    EditorGUILayout.PropertyField(RestoreCoins, new GUIContent("Restore"), GUILayout.MinWidth(30));
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                //-----------------

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Locked", newGuiStyle);
                    EditorGUILayout.BeginVertical(EditorStyles.textField); EditorGUILayout.EndVertical();
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(Locked, new GUIContent("Current"), true );
                    EditorGUILayout.PropertyField(RestoreLocked, new GUIContent("Restore"), true );
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();


                //---------------------------------------------------------------------------------

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Items Amount", newGuiStyle);
                    EditorGUILayout.BeginVertical(EditorStyles.textField); EditorGUILayout.EndVertical();
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(ItemsAmount, new GUIContent("Current"), true );
                    EditorGUILayout.PropertyField(RestoreItemsAmount, new GUIContent("Restore"), true );
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();

                //---------------------------------------------------------------------------------

                if (UseMaterialChanger.boolValue)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUILayout.LabelField("Material Changer Index", newGuiStyle);
                        EditorGUILayout.BeginVertical(EditorStyles.textField); EditorGUILayout.EndVertical();
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(Save.FindPropertyRelative("MaterialIndex"), new GUIContent("Current"), true );
                        EditorGUILayout.PropertyField(Save.FindPropertyRelative("RestoreMaterialIndex"), new GUIContent("Restore"), true );
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                }

                //---------------------------------------------------------------------------------

                if ( UseActiveMesh.boolValue)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUILayout.LabelField("Active Meshes Index", newGuiStyle);
                        EditorGUILayout.BeginVertical(EditorStyles.textField); EditorGUILayout.EndVertical();
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(ActiveMeshIndex, new GUIContent("Current"), true );
                        EditorGUILayout.PropertyField(RestoreActiveMeshIndex, new GUIContent("Restore"), true );
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Selector Data Inspector");
                //EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}