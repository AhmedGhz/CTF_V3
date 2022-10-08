using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ThrustEngine))]

public class ThrustEngineEditorWindow : Editor {

	public override void OnInspectorGUI(){
		ThrustEngine currentScript = (ThrustEngine)target;

		currentScript.enabled = EditorGUILayout.Toggle ("Enabled", currentScript.enabled);
		currentScript.boundObject = (Rigidbody)EditorGUILayout.ObjectField ("Bound Body", currentScript.boundObject, typeof(Rigidbody), true);

		if (currentScript.boundObject == null && currentScript.GetComponent<Rigidbody> () == null) {
			EditorGUILayout.HelpBox ("Thruster will be disabled at runtime. \n\nThruster does not have a rigidbody to target. \n\nPlease link a bound body (parent) or add a rigidbody directly to the thruster.", MessageType.Warning);
		} else if (currentScript.boundObject == null && currentScript.GetComponent<Rigidbody> () != null) {
			EditorGUILayout.HelpBox ("Using rigidbody attached directly to thruster, no bound body (parent) attached.", MessageType.Info);
		}

		currentScript.maxPower = EditorGUILayout.FloatField ("Max Power", currentScript.maxPower);
		EditorGUILayout.LabelField ("Current Power (" + (currentScript.currentPowerPercentage * 100) + "%)");
		currentScript.currentPowerPercentage = EditorGUILayout.Knob (new Vector2 (40, 40), currentScript.currentPowerPercentage, 0.0f, 1.0f, "", Color.gray, Color.cyan, true); 

		currentScript.hoverMode = EditorGUILayout.Toggle ("Enable Hover Mode", currentScript.hoverMode);


		if (currentScript.hoverMode) {
			EditorGUILayout.HelpBox("Hover mode will dynamically manage thrust to allow object to hover a predefined distance from ground.", MessageType.Info);

			currentScript.hoverDistance = EditorGUILayout.FloatField ("Hover Distance", currentScript.hoverDistance);
			currentScript.hoverSafeRange = EditorGUILayout.FloatField ("Hover Safe Range", currentScript.hoverSafeRange);

		}

	}
}
