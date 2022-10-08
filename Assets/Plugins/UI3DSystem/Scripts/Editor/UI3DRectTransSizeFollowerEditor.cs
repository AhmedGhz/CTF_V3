using System.Collections;
using System.Collections.Generic;
using UI3DEnums;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CanEditMultipleObjects()]
[CustomEditor(typeof(UI3DRectTransSizeFollower))]
public class UI3DRectTransSizeFollowerEditor : Editor
{
    private UI3DRectTransSizeFollower follower;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        follower = (UI3DRectTransSizeFollower)target;

        if(follower)
        {
            if(follower.targetRectTransform == null)
            {
                EditorGUILayout.HelpBox("Select target rect transform", MessageType.Info);
                follower.dataSnapshot = new UI3DRectTransSizeFollower.UISnapshot();
            }
            else if(follower.dataSnapshot.transformScale == Vector3.zero)
            {
                EditorGUILayout.HelpBox("Make rect snapshot to save settings to which object will be scaled", MessageType.Info);
            }

            if(follower.targetRectTransform != null && !Application.isPlaying && GUILayout.Button("Make rect snapshot"))
            {
                follower.MakeRectSnapshot();
                EditorUtility.SetDirty(follower);
            }
        }
    }

}
