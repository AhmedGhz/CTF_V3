using Polarith.AI.Package;
using UnityEditor;
using UnityEngine;

namespace Polarith.AI.PackageEditor
{
    [CustomEditor(typeof(CopterController))]
    internal sealed class CopterControllerEditor : Editor
    {
        #region Fields =================================================================================================

        private GUILayoutOption maxWidth = GUILayout.MaxWidth(50.0f);
        private GUILayoutOption maxWidthBig = GUILayout.MaxWidth(55.0f);

        #endregion // Fields

        #region Methods ================================================================================================

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("context"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("body"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("objectiveAsSpeed"));

            SerializedProperty usePid = serializedObject.FindProperty("usePidController");
            EditorGUILayout.PropertyField(usePid);

            GUI.enabled = usePid.boolValue;
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", maxWidthBig);
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("P", maxWidth);
            EditorGUILayout.LabelField("I", maxWidth);
            EditorGUILayout.LabelField("D", maxWidth);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Roll", maxWidthBig);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rollController").FindPropertyRelative("gainP"),
                new GUIContent(), maxWidth);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rollController").FindPropertyRelative("gainI"),
                new GUIContent(), maxWidth);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rollController").FindPropertyRelative("gainD"),
                new GUIContent(), maxWidth);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Pitch", maxWidthBig);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("pitchController").FindPropertyRelative("gainP"),
                new GUIContent(), maxWidth);
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("pitchController").FindPropertyRelative("gainI"),
                new GUIContent(), maxWidth);
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("pitchController").FindPropertyRelative("gainD"),
                new GUIContent(), maxWidth);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Yaw", maxWidthBig);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("yawController").FindPropertyRelative("gainP"),
                new GUIContent(), maxWidth);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("yawController").FindPropertyRelative("gainI"),
                new GUIContent(), maxWidth);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("yawController").FindPropertyRelative("gainD"),
                new GUIContent(), maxWidth);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Thrust", maxWidthBig);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("thrustController").FindPropertyRelative("gainP"),
                new GUIContent(), maxWidth);
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("thrustController").FindPropertyRelative("gainI"),
                new GUIContent(), maxWidth);
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("thrustController").FindPropertyRelative("gainD"),
                new GUIContent(), maxWidth);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
            GUI.enabled = true;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("observationPoint"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("flightMode"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speedFactor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rollFactor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pitchFactor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("yawFactor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("thrustFactor"));

            serializedObject.ApplyModifiedProperties();
        }

        #endregion // Methods
    } // class CopterControllerEditor
} // namespace Polarith.AI.PackageEditor
