using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEditor;
using UnityEditor.Events;
using MalbersAnimations.Events;

namespace MalbersAnimations.Selector
{
    [CustomEditor(typeof(SelectorManager))]
    public class SelectorManagerEditor : Editor
    {
        SelectorManager M;
        MonoScript script;


        SerializedProperty enableSelector, DontDestroy, ItemSelected, OriginalItemSelected, InstantiateItems, SpawnPoint, RemoveLast, DontDestroySelectedItem, LoadItemSet,
            ShowAnims, Target, EnterAnimation, ExitAnimation, Data, OnSelected, OnOpen, OnClosed, OnDataChanged, ShowEvents, OnItemFocused, FocusItemAnim, SubmitItemAnim;

        bool DataHelp = false;
        bool EventHelp = false;

        private void OnEnable()
        {
            M = (SelectorManager)target;
            script = MonoScript.FromMonoBehaviour((MonoBehaviour)target);
            enableSelector = serializedObject.FindProperty("enableSelector");
            LoadItemSet = serializedObject.FindProperty("LoadItemSet");
            DontDestroy = serializedObject.FindProperty("DontDestroy");
            ItemSelected = serializedObject.FindProperty("ItemSelected");
            OriginalItemSelected = serializedObject.FindProperty("OriginalItemSelected");
            InstantiateItems = serializedObject.FindProperty("InstantiateItems");
            SpawnPoint = serializedObject.FindProperty("SpawnPoint");
            RemoveLast = serializedObject.FindProperty("RemoveLast");
            DontDestroySelectedItem = serializedObject.FindProperty("DontDestroySelectedItem");
            ShowAnims = serializedObject.FindProperty("ShowAnims");
            Target = serializedObject.FindProperty("Target");
            EnterAnimation = serializedObject.FindProperty("EnterAnimation");
            ExitAnimation = serializedObject.FindProperty("ExitAnimation");
            FocusItemAnim = serializedObject.FindProperty("FocusItemAnim");
            SubmitItemAnim = serializedObject.FindProperty("SubmitItemAnim");
            Data = serializedObject.FindProperty("Data");
            OnSelected = serializedObject.FindProperty("OnSelected");
            OnOpen = serializedObject.FindProperty("OnOpen");
            OnClosed = serializedObject.FindProperty("OnClosed");
            OnDataChanged = serializedObject.FindProperty("OnDataChanged");

            OnItemFocused = serializedObject.FindProperty("OnItemFocused");

            ShowEvents = serializedObject.FindProperty("ShowEvents");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Global Manager\nThe selector uses UI LAYER to work properly", MessageType.None);
            EditorGUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            {
                EditorGUI.BeginDisabledGroup(true);
                script = (MonoScript)EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                if (!Application.isPlaying)
                {
                    enableSelector.boolValue = EditorGUILayout.ToggleLeft(new GUIContent(enableSelector.boolValue ? "Open on Awake" : "Closed on Awake", "Initial State of the selector"), enableSelector.boolValue, GUILayout.MinWidth(80));
                }
                else
                {
                    EditorGUILayout.LabelField(enableSelector.boolValue ? "OPEN" : "CLOSED", EditorStyles.largeLabel);
                }

                DontDestroy.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Don't destroy on Load", "Set the Selector to not be destroyed automatically when loading a new scene."), DontDestroy.boolValue);

                EditorGUILayout.EndVertical(); 
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(LoadItemSet, new GUIContent("Load Item Set", "Loads a new Set of Items at the start of the Selector"));
                EditorGUILayout.EndVertical();


                if (Application.isPlaying)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(ItemSelected, new GUIContent("Item", "Item Holder"));
                    EditorGUILayout.PropertyField(OriginalItemSelected, new GUIContent("Original GO", "Original Game Object"));
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndVertical();
                }


                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                InstantiateItems.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Instantiate Item", "Insantiate the original prefab of the selected item\n If no Transform is selected, it will instantiate on (0,0,0)"),InstantiateItems.boolValue, GUILayout.MinWidth(80));

                if (InstantiateItems.boolValue)
                {
                    EditorGUILayout.PropertyField(SpawnPoint, new GUIContent("", "Spawn Point to Instantiate the items, if empty it will spawn it into(0, 0, 0)"), GUILayout.MinWidth(80));
                }
                EditorGUILayout.EndHorizontal();
                if (InstantiateItems.boolValue)
                {
                    RemoveLast.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Remove Last Spawn", "Remove the Last Spawned Object"), RemoveLast.boolValue);
                    DontDestroySelectedItem.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Don't destroy Selected Item", "Makes the Selected Item not be destroyed automatically when loading a new scene."), DontDestroySelectedItem.boolValue);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                ShowAnims.boolValue = EditorGUILayout.Foldout(ShowAnims.boolValue, "Transform Animations");
                EditorGUI.indentLevel--;

                if (ShowAnims.boolValue)
                {
                    EditorGUILayout.PropertyField(Target, new GUIContent("Items Root", "This is the Root Transform for the items to animate for Opening & Closing the selector"), GUILayout.MinWidth(50));

                    if (Target.objectReferenceValue != null)
                    {
                        EditorGUILayout.PropertyField(EnterAnimation, new GUIContent("Open", "Plays an animation on enter"), GUILayout.MinWidth(50));
                        EditorGUILayout.PropertyField(ExitAnimation, new GUIContent("Close", "Plays an animation on exit"), GUILayout.MinWidth(50));
                    }
                    EditorGUILayout.PropertyField(FocusItemAnim, new GUIContent("Focused Anim", "Plays an Transform Animation on the Focused Item"), GUILayout.MinWidth(50));
                    EditorGUILayout.PropertyField(SubmitItemAnim, new GUIContent("Sumbit Anim", "Plays an Transform Animation when Submit is called"), GUILayout.MinWidth(50));
                }


                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(Data, new GUIContent("Data", "This is an Scriptable Object to save all the important values of the Selector\n You can enable 'Use PlayerPref' and save the data there, but is recommended to use a Better and secure  'Saving System'"));
                DataHelp = GUILayout.Toggle(DataHelp, "?", EditorStyles.miniButton, GUILayout.MaxWidth(16));
                EditorGUILayout.EndHorizontal();

                if (Data.objectReferenceValue != null)
                {
                    // Coins = Data.FindPropertyRelative("Save").FindPropertyRelative("Coins");
                  // var usePlayerPref = Data.FindPropertyRelative("usePlayerPref");
                  //  EditorGUILayout.PropertyField(Coins, new GUIContent("Coins", "Amount of coins"));
                    M.Data.Save.Coins = EditorGUILayout.IntField(new GUIContent("Coins", "Amount of coins"), M.Data.Save.Coins);
                 //  usePlayerPref.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Use PlayerPref to Save Data", "Enable it to persistent save the Data using PlayerPref Class, but is recommended to use a better and secure 'Saving System' and connect it to the Data Asset"), usePlayerPref.boolValue);
                    M.Data.usePlayerPref = EditorGUILayout.ToggleLeft(new GUIContent("Use PlayerPref to Save Data", "Enable it to persistent save the Data using PlayerPref Class, but is recommended to use a better and secure 'Saving System' and connect it to the Data Asset"), M.Data.usePlayerPref);
                }

                if (DataHelp)
                {
                    EditorGUILayout.HelpBox("To create a Data Asset go to\nAssets -> Create -> MalbersAnimations -> Ultimate Selector -> SelectorData\n\nThe Data are 'Scriptable Objects Assets' used to save all the important values of the Selector, like Coins, Locked Items, Item Amount.\nYou can enable 'Use PlayerPref' to persistent save the Data using that method, but is recomended to use a better and secure 'Saving System' and connect it to the Data Asset", MessageType.None);
                }
                EditorGUILayout.BeginHorizontal();

                if (Data.objectReferenceValue != null)
                {
                    if (GUILayout.Button(new GUIContent("Save Default Data", "Store all the Items Values and Coins as the Restore/Default Data")))
                    {
                        M.SaveDefaultData();
                        if (Data.objectReferenceValue != null)
                        {
                            EditorUtility.SetDirty(M.Data);
                        }
                    }

                    if (GUILayout.Button(new GUIContent("Restore Data", "Restore all the values from  to the Default Data")))
                    {
                        M.RestoreToDefaultData();
                        if (Data.objectReferenceValue != null)
                        {
                            EditorUtility.SetDirty(M.Data);
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                ShowEvents.boolValue = EditorGUILayout.Foldout(ShowEvents.boolValue, "Events");
                EventHelp = GUILayout.Toggle(EventHelp, "?", EditorStyles.miniButton, GUILayout.MaxWidth(16));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;

                if (ShowEvents.boolValue)
                {
                    if (EventHelp)
                    {
                        EditorGUILayout.HelpBox("On Selected: Invoked when an Item is selected using the method: 'SelectItem()'\n\nBy Default the 'Select' button on the UI is the ones who calls 'SelectItem()'\n\nOn Open: Invoked when the selector is open\n\nOn Closed: Invoked when the selector is closedn\n\nOn Data Changed: Invoked when the Selector Data is modified \n\nYou can conect whatever you want to your own system using this unity event\n\nIf the Item has an Original GameObject (OGo), the OGo will be the one sent on the Event, if not the Item gameObject will be the one sent on the Event", MessageType.None);
                    }
                    EditorGUILayout.PropertyField(OnSelected);
                    EditorGUILayout.PropertyField(OnItemFocused);
                    EditorGUILayout.PropertyField(OnOpen);
                    EditorGUILayout.PropertyField(OnClosed);

                    if (Data.objectReferenceValue != null)
                    {
                        EditorGUILayout.PropertyField(OnDataChanged);
                    }
                }
                EditorGUILayout.EndVertical();
            }
          
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(M, "Manager Values Changed");
              //  EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
