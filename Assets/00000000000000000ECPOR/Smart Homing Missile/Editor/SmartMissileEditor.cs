using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SmartMissile), true), CanEditMultipleObjects]
public class SmartMissileEditor : Editor
{
	SerializedProperty m_drawSearchZone;
	SerializedProperty m_zoneColor;

	void OnEnable()
	{
		m_drawSearchZone = serializedObject.FindProperty("m_drawSearchZone");
		m_zoneColor = serializedObject.FindProperty("m_zoneColor");
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (target.GetType() == typeof(SmartMissile3D))
			(target as SmartMissile3D).TargetTag = EditorGUILayout.TagField(new GUIContent("Target Tag", "What the missile should target"), (target as SmartMissile3D).TargetTag);
		else if (target.GetType() == typeof(SmartMissile2D))
			(target as SmartMissile2D).TargetTag = EditorGUILayout.TagField(new GUIContent("Target Tag", "What the missile should target"), (target as SmartMissile2D).TargetTag);

		serializedObject.Update();
		EditorGUILayout.PropertyField(m_zoneColor, true);
		EditorGUILayout.PropertyField(m_drawSearchZone);
		serializedObject.ApplyModifiedProperties();
	}
}
