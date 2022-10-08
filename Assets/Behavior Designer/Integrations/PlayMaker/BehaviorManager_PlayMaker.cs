using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.PlayMaker;
using HutongGames.PlayMaker;

namespace BehaviorDesigner.Runtime
{
    public static class BehaviorManager_PlayMaker
    {
        public static void PlayMakerFinished(this BehaviorManager behaviorManager, Fsm playMakerFSM, TaskStatus status)
        {
            if (behaviorManager == null) {
                return;
            }
            var task = behaviorManager.TaskForObject(playMakerFSM);
            if (task is StartFSM) {
                var playMakerTask = task as StartFSM;
                if (playMakerTask.PlayMakerFSM.Fsm.Equals(playMakerFSM)) {
                    playMakerTask.PlayMakerFinished(status);
                }
            } else if (task is RunConditionalFSM) {
                var playMakerTask = task as RunConditionalFSM;
                if (playMakerTask.PlayMakerFSM.Fsm.Equals(playMakerFSM)) {
                    playMakerTask.PlayMakerFinished(status);
                }
            }
        }

        public static bool StopPlayMaker(StartFSM playMakerTask)
        {
            var playMakerFSM = playMakerTask.PlayMakerFSM.Fsm;
            if (!string.IsNullOrEmpty(playMakerTask.endEventName.Value)) {
                playMakerFSM.Event(playMakerTask.endEventName.Value);
            }

            if (playMakerTask.resetOnComplete.Value) {
                bool prevRestartOnEnable = playMakerFSM.RestartOnEnable;
                if (!playMakerFSM.RestartOnEnable) {
                    playMakerFSM.RestartOnEnable = true;
                }
                // Enable/Disable PlayMaker to force it to restart from the beginning
                playMakerFSM.Owner.enabled = false;
                playMakerFSM.Owner.enabled = true;

                playMakerFSM.RestartOnEnable = prevRestartOnEnable;
            }

            return true;
        }

        public static void SyncVariablesToPlayMaker(this BehaviorManager behaviorManager, IVariableSource variableSource, FsmVariables fsmVariables)
        {
            DoVariablesToPlayMakerSync(variableSource, fsmVariables);
        }

        public static void SyncGlobalVariablesToPlayMaker(this BehaviorManager behaviorManager)
        {
            DoVariablesToPlayMakerSync(GlobalVariables.Instance, FsmVariables.GlobalVariables);
        }

        private static void DoVariablesToPlayMakerSync(IVariableSource variableSource, FsmVariables playMakerVariables)
        {
            if (variableSource == null || playMakerVariables == null) {
                return;
            }

            SharedVariable behaviorDesignerVariable = null;

            // FsmBool
            for (int i = 0; i < playMakerVariables.BoolVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.BoolVariables[i].Name)) != null) {
                    playMakerVariables.BoolVariables[i].Value = (bool)behaviorDesignerVariable.GetValue();
                }
            }

            // FsmColor
            for (int i = 0; i < playMakerVariables.ColorVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.ColorVariables[i].Name)) != null) {
                    playMakerVariables.ColorVariables[i].Value = (Color)behaviorDesignerVariable.GetValue();
                }
            }

            // FsmFloat
            for (int i = 0; i < playMakerVariables.FloatVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.FloatVariables[i].Name)) != null) {
                    playMakerVariables.FloatVariables[i].Value = (float)behaviorDesignerVariable.GetValue();
                }
            }

            // FsmGameObject
            for (int i = 0; i < playMakerVariables.GameObjectVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.GameObjectVariables[i].Name)) != null) {
                    playMakerVariables.GameObjectVariables[i].Value = (GameObject)behaviorDesignerVariable.GetValue();
                }
            }

            // FsmInt
            for (int i = 0; i < playMakerVariables.IntVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.IntVariables[i].Name)) != null) {
                    playMakerVariables.IntVariables[i].Value = (int)behaviorDesignerVariable.GetValue();
                }
            }

            // FsmObject
            for (int i = 0; i < playMakerVariables.ObjectVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.ObjectVariables[i].Name)) != null) {
                    playMakerVariables.ObjectVariables[i].Value = (Object)behaviorDesignerVariable.GetValue();
                }
            }

            // FsmQuaternion
            for (int i = 0; i < playMakerVariables.QuaternionVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.QuaternionVariables[i].Name)) != null) {
                    playMakerVariables.QuaternionVariables[i].Value = (Quaternion)behaviorDesignerVariable.GetValue();
                }
            }

            // FsmRect
            for (int i = 0; i < playMakerVariables.RectVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.RectVariables[i].Name)) != null) {
                    playMakerVariables.RectVariables[i].Value = (Rect)behaviorDesignerVariable.GetValue();
                }
            }

            // FsmString
            for (int i = 0; i < playMakerVariables.StringVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.StringVariables[i].Name)) != null) {
                    playMakerVariables.StringVariables[i].Value = (string)behaviorDesignerVariable.GetValue();
                }
            }

            // FsmVector2
            for (int i = 0; i < playMakerVariables.Vector2Variables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.Vector2Variables[i].Name)) != null) {
                    playMakerVariables.Vector2Variables[i].Value = (Vector2)behaviorDesignerVariable.GetValue();
                }
            }

            // FsmVector3
            for (int i = 0; i < playMakerVariables.Vector3Variables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.Vector3Variables[i].Name)) != null) {
                    playMakerVariables.Vector3Variables[i].Value = (Vector3)behaviorDesignerVariable.GetValue();
                }
            }
        }

        public static void SyncVariablesFromPlayMaker(this BehaviorManager behaviorManager, IVariableSource variableSource, FsmVariables fsmVariables)
        {
            DoVariablesFromPlayMakerSync(variableSource, fsmVariables);
        }

        public static void SyncGlobalVariablesFromPlayMaker(this BehaviorManager behaviorManager)
        {
            DoVariablesFromPlayMakerSync(GlobalVariables.Instance, FsmVariables.GlobalVariables);
        }

        private static void DoVariablesFromPlayMakerSync(IVariableSource variableSource, FsmVariables playMakerVariables)
        {
            if (variableSource == null || playMakerVariables == null) {
                return;
            }

            SharedVariable behaviorDesignerVariable = null;

            // FsmBool
            for (int i = 0; i < playMakerVariables.BoolVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.BoolVariables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.BoolVariables[i].Value);
                }
            }

            // FsmColor
            for (int i = 0; i < playMakerVariables.ColorVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.ColorVariables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.ColorVariables[i].Value);
                }
            }

            // FsmFloat
            for (int i = 0; i < playMakerVariables.FloatVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.FloatVariables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.FloatVariables[i].Value);
                }
            }

            // FsmGameObject
            for (int i = 0; i < playMakerVariables.GameObjectVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.GameObjectVariables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.GameObjectVariables[i].Value);
                }
            }

            // FsmInt
            for (int i = 0; i < playMakerVariables.IntVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.IntVariables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.IntVariables[i].Value);
                }
            }

            // FsmObject
            for (int i = 0; i < playMakerVariables.ObjectVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.ObjectVariables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.ObjectVariables[i].Value);
                }
            }

            // FsmQuaternion
            for (int i = 0; i < playMakerVariables.QuaternionVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.QuaternionVariables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.QuaternionVariables[i].Value);
                }
            }

            // FsmRect
            for (int i = 0; i < playMakerVariables.RectVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.RectVariables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.RectVariables[i].Value);
                }
            }

            // FsmString
            for (int i = 0; i < playMakerVariables.StringVariables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.StringVariables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.StringVariables[i].Value);
                }
            }

            // FsmVector2
            for (int i = 0; i < playMakerVariables.Vector2Variables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.Vector2Variables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.Vector2Variables[i].Value);
                }
            }

            // FsmVector3
            for (int i = 0; i < playMakerVariables.Vector3Variables.Length; ++i) {
                if ((behaviorDesignerVariable = variableSource.GetVariable(playMakerVariables.Vector3Variables[i].Name)) != null) {
                    behaviorDesignerVariable.SetValue(playMakerVariables.Vector3Variables[i].Value);
                }
            }
        }
    }
}