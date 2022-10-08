using UnityEngine;
using HutongGames.PlayMaker;

namespace BehaviorDesigner.Runtime
{
    public static class VariableSynchronizer_PlayMaker
    {
        public enum PlayMakerVariableType { Bool, Color, Float, GameObject, Int, Object, Quaternion, Rect, String, Vector2, Vector3 }

        public static int Start(VariableSynchronizer.SynchronizedVariable synchronizedVariable)
        {
            var playMakerFSM = synchronizedVariable.targetComponent as PlayMakerFSM;
            HutongGames.PlayMaker.NamedVariable namedVariable = null;
            if (synchronizedVariable.global && FsmVariables.GlobalVariables != null) {
                namedVariable = FsmVariables.GlobalVariables.GetVariable(synchronizedVariable.targetName);
            } else {
                namedVariable = playMakerFSM.FsmVariables.GetVariable(synchronizedVariable.targetName);
            }
            if (namedVariable == null) {
                return 1;
            }
            var valueType = synchronizedVariable.sharedVariable.GetType().GetProperty("Value").PropertyType;
            if (namedVariable is FsmBool && valueType.Equals(typeof(bool))) {
                synchronizedVariable.variableType = PlayMakerVariableType.Bool;
            } else if (namedVariable is FsmColor && valueType.Equals(typeof(Color))) {
                synchronizedVariable.variableType = PlayMakerVariableType.Color;
            } else if (namedVariable is FsmFloat && valueType.Equals(typeof(float))) {
                synchronizedVariable.variableType = PlayMakerVariableType.Float;
            } else if (namedVariable is FsmGameObject && valueType.Equals(typeof(GameObject))) {
                synchronizedVariable.variableType = PlayMakerVariableType.GameObject;
            } else if (namedVariable is FsmInt && valueType.Equals(typeof(int))) {
                synchronizedVariable.variableType = PlayMakerVariableType.Int;
            } else if (namedVariable is FsmObject && valueType.Equals(typeof(UnityEngine.Object))) {
                synchronizedVariable.variableType = PlayMakerVariableType.Object;
            } else if (namedVariable is FsmQuaternion && valueType.Equals(typeof(Quaternion))) {
                synchronizedVariable.variableType = PlayMakerVariableType.Quaternion;
            } else if (namedVariable is FsmRect && valueType.Equals(typeof(Rect))) {
                synchronizedVariable.variableType = PlayMakerVariableType.Rect;
            } else if (namedVariable is FsmString && valueType.Equals(typeof(string))) {
                synchronizedVariable.variableType = PlayMakerVariableType.String;
            } else if (namedVariable is FsmVector2 && valueType.Equals(typeof(Vector2))) {
                synchronizedVariable.variableType = PlayMakerVariableType.Vector2;
            } else if (namedVariable is FsmVector3 && valueType.Equals(typeof(Vector3))) {
                synchronizedVariable.variableType = PlayMakerVariableType.Vector3;
            } else {
                return 2;
            }
            synchronizedVariable.thirdPartyVariable = namedVariable;
            return 0;
        }

        public static void Tick(VariableSynchronizer.SynchronizedVariable synchronizedVariable)
        {
            var variableType = (PlayMakerVariableType)synchronizedVariable.variableType;
            switch (variableType) {
                case PlayMakerVariableType.Bool:
                    var boolVariable = synchronizedVariable.thirdPartyVariable as FsmBool;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(boolVariable.Value);
                    } else {
                        boolVariable.Value = (bool)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
                case PlayMakerVariableType.Color:
                    var colorVariable = synchronizedVariable.thirdPartyVariable as FsmColor;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(colorVariable.Value);
                    } else {
                        colorVariable.Value = (Color)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
                case PlayMakerVariableType.Float:
                    var floatVariable = synchronizedVariable.thirdPartyVariable as FsmFloat;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(floatVariable.Value);
                    } else {
                        floatVariable.Value = (float)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
                case PlayMakerVariableType.GameObject:
                    var playMakerVariable = synchronizedVariable.thirdPartyVariable as FsmGameObject;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(playMakerVariable.Value);
                    } else {
                        playMakerVariable.Value = (GameObject)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
                case PlayMakerVariableType.Int:
                    var intVariable = synchronizedVariable.thirdPartyVariable as FsmInt;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(intVariable.Value);
                    } else {
                        intVariable.Value = (int)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
                case PlayMakerVariableType.Object:
                    var objectVariable = synchronizedVariable.thirdPartyVariable as FsmObject;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(objectVariable.Value);
                    } else {
                        objectVariable.Value = (UnityEngine.Object)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
                case PlayMakerVariableType.Quaternion:
                    var quaternionVariable = synchronizedVariable.thirdPartyVariable as FsmQuaternion;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(quaternionVariable.Value);
                    } else {
                        quaternionVariable.Value = (Quaternion)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
                case PlayMakerVariableType.Rect:
                    var rectVariable = synchronizedVariable.thirdPartyVariable as FsmRect;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(rectVariable.Value);
                    } else {
                        rectVariable.Value = (Rect)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
                case PlayMakerVariableType.String:
                    var stringVariable = synchronizedVariable.thirdPartyVariable as FsmString;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(stringVariable.Value);
                    } else {
                        stringVariable.Value = (string)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
                case PlayMakerVariableType.Vector2:
                    var vector2Variable = synchronizedVariable.thirdPartyVariable as FsmVector2;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(vector2Variable.Value);
                    } else {
                        vector2Variable.Value = (Vector2)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
                case PlayMakerVariableType.Vector3:
                    var vector3Variable = synchronizedVariable.thirdPartyVariable as FsmVector3;
                    if (synchronizedVariable.setVariable) {
                        synchronizedVariable.sharedVariable.SetValue(vector3Variable.Value);
                    } else {
                        vector3Variable.Value = (Vector3)synchronizedVariable.sharedVariable.GetValue();
                    }
                    break;
            }
        }
    }
}