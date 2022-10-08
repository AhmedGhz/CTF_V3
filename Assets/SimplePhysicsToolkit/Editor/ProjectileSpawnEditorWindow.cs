using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ProjectileSpawn))]
public class ProjectileSpawnEditorWindow : Editor {

	private SerializedObject m_Object;
	private SerializedProperty m_Property;

	void OnEnable() {
		m_Object = new SerializedObject(target);
	}

	public override void OnInspectorGUI(){
		ProjectileSpawn currentScript = (ProjectileSpawn)target;

		currentScript.enabled = EditorGUILayout.Toggle ("Enabled", currentScript.enabled);
		currentScript.addRandomInertiaToProjectiles = EditorGUILayout.Toggle ("Random Intertia", currentScript.addRandomInertiaToProjectiles);

		currentScript.inaccuracy = EditorGUILayout.FloatField ("Inaccuracy", currentScript.inaccuracy);
		currentScript.emitRate = EditorGUILayout.FloatField ("Emit Rate", currentScript.emitRate);
		currentScript.speed = EditorGUILayout.FloatField ("Speed", currentScript.speed);


		m_Property = m_Object.FindProperty("objects");
		EditorGUILayout.PropertyField(m_Property, new GUIContent("Objects"), true);
		m_Object.ApplyModifiedProperties();

		currentScript.limitObjectsInScene = EditorGUILayout.Toggle ("Limit Objects In Scene", currentScript.limitObjectsInScene);


		if (currentScript.limitObjectsInScene) {
			currentScript.limitAmount = EditorGUILayout.IntField ("Max Objects", currentScript.limitAmount);
			EditorGUILayout.HelpBox ("Deletes excessive objects in scene to reduce resource load. Set Limit above to max amount of objects.", MessageType.Info);
		}
	}
}
