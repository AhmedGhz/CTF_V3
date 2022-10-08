using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using HutongGames.PlayMaker;

namespace BehaviorDesigner.Editor
{
    public static class VariableSynchronizerInspector_PlayMaker
    {
        public static void DrawPlayMakerSynchronizer(VariableSynchronizerInspector.Synchronizer synchronizer, Type valueType)
        {
            VariableSynchronizerInspector.DrawComponentSelector(synchronizer, typeof(PlayMakerFSM), VariableSynchronizerInspector.ComponentListType.None);

            if (synchronizer.gameObject != null) {
                var allFSMs = synchronizer.gameObject.GetComponents<PlayMakerFSM>();
                if (allFSMs != null && allFSMs.Length > 1) {
                    synchronizer.componentName = EditorGUILayout.TextField("PlayMaker FSM Name", synchronizer.componentName);
                }
                synchronizer.component = GetFSMWithName(allFSMs, synchronizer.componentName);
            } else if (synchronizer.component != null) {
                synchronizer.component = null;
            }

            if (synchronizer.component == null) {
                GUI.enabled = false;
            }

            int index = 0;
            int globalStartIndex = -1;
            var names = new List<string>();
            names.Add("None");
            if (synchronizer.component != null) {
                var fsm = synchronizer.component as PlayMakerFSM;
                var localIndex = GetPlayMakerVariables(valueType, synchronizer.targetName, false, fsm.FsmVariables.GetAllNamedVariables(), ref names);
                if (!synchronizer.global) {
                    index = localIndex;
                }
                if (FsmVariables.GlobalVariables != null) {
                    globalStartIndex = names.Count;
                    var globalIndex = GetPlayMakerVariables(valueType, synchronizer.targetName, true, FsmVariables.GlobalVariables.GetAllNamedVariables(), ref names);
                    if (synchronizer.global) {
                        index = globalIndex;
                    }
                }
            }
            EditorGUI.BeginChangeCheck();
            index = EditorGUILayout.Popup("Named Variable", index, names.ToArray());
            if (EditorGUI.EndChangeCheck()) {
                if (index != 0) {
                    if (globalStartIndex != -1 && index >= globalStartIndex) {
                        synchronizer.targetName = names[index].Substring(8, names[index].Length - 8); // ignore the "Global/" text
                        synchronizer.global = true;
                    } else {
                        synchronizer.targetName = names[index];
                        synchronizer.global = false;
                    }
                } else {
                    synchronizer.targetName = null;
                }
            }
        }

        private static int GetPlayMakerVariables(Type valueType, string targetName, bool global, NamedVariable[] namedVariables, ref List<string> names)
        {
            int index = 0;
            for (int i = 0; i < namedVariables.Length; ++i) {
                if (CanSelect(namedVariables[i], valueType)) {
                    if (namedVariables[i].Name.Equals(targetName)) {
                        index = names.Count;
                    }
                    names.Add((global ? "Globals/" : "" ) + namedVariables[i].Name);
                }
            }
            return index;
        }

        private static PlayMakerFSM GetFSMWithName(PlayMakerFSM[] fsms, string name)
        {
            if (fsms == null || fsms.Length == 0) {
                return null;
            }
            if (fsms.Length == 1) {
                return fsms[0];
            }
            for (int i = 0; i < fsms.Length; ++i) {
                if (fsms[i].FsmName == name) {
                    return fsms[i];
                }
            }
            // no fsm with the specified name, just return the first
            return fsms[0];
        }

        private static bool CanSelect(NamedVariable namedVariable, Type valueType)
        {
            if (namedVariable == null || valueType == null) {
                return false;
            }

            if (namedVariable is FsmBool && valueType.Equals(typeof(bool))) {
                return true;
            } else if (namedVariable is FsmColor && valueType.Equals(typeof(Color))) {
                return true;
            } else if (namedVariable is FsmFloat && valueType.Equals(typeof(float))) {
                return true;
            } else if (namedVariable is FsmGameObject && valueType.Equals(typeof(GameObject))) {
                return true;
            } else if (namedVariable is FsmInt && valueType.Equals(typeof(int))) {
                return true;
            } else if (namedVariable is FsmObject && valueType.Equals(typeof(UnityEngine.Object))) {
                return true;
            } else if (namedVariable is FsmQuaternion && valueType.Equals(typeof(Quaternion))) {
                return true;
            } else if (namedVariable is FsmRect && valueType.Equals(typeof(Rect))) {
                return true;
            } else if (namedVariable is FsmString && valueType.Equals(typeof(string))) {
                return true;
            } else if (namedVariable is FsmVector2 && valueType.Equals(typeof(Vector2))) {
                return true;
            } else if (namedVariable is FsmVector3 && valueType.Equals(typeof(Vector3))) {
                return true;
            }
            return false;
        }
    }
}