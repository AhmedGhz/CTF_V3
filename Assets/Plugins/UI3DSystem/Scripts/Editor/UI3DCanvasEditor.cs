using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CanEditMultipleObjects()]
[CustomEditor(typeof(UI3DCanvas))]
public class UI3DCanvasEditor : Editor
{
    private UI3DCanvas canvas;
    private static bool debugMask;
    private static bool debugCullMask;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        canvas = (UI3DCanvas)target;

        if(canvas)
        {
            EditorGUI.BeginChangeCheck();

            canvas.maskResolutionScale = EditorGUILayout.Slider("Mask resolution scale", canvas.maskResolutionScale, 0.05f, 1f);
            canvas.maskLayer = EditorGUILayout.LayerField("Mask layer", canvas.maskLayer);
			if(canvas.Canvas.worldCamera == null)
			{
				EditorGUILayout.HelpBox("Attach camera to GUI Canvas!", MessageType.Error);
			}
			else
			{
				bool layerIsInCulling = canvas.Canvas.worldCamera.cullingMask == (canvas.Canvas.worldCamera.cullingMask | (1 << canvas.maskLayer));
				if(layerIsInCulling)
					EditorGUILayout.HelpBox("Mask layer shouldn't be the same as layer rendered by GUI camera, change the mask layer or change culling mask in GUI camera", MessageType.Error);
			}

            if(EditorGUI.EndChangeCheck())
            {
                canvas.enabled = false;
                canvas.SetToReinitialize();
                canvas.enabled = true;
                if(!Application.isPlaying)
                {
                    EditorUtility.SetDirty(canvas);
                    if(canvas.gameObject.scene.IsValid())
                        EditorSceneManager.MarkSceneDirty(canvas.gameObject.scene);
                }
            }

            if(canvas.Mask != null && (debugMask = EditorGUILayout.Foldout(debugMask, "Show depth mask")))
            {
                Rect texRect = GUILayoutUtility.GetAspectRect((float)canvas.Mask.width / canvas.Mask.height);
                EditorGUI.DrawPreviewTexture(texRect, canvas.Mask);
            }

            if(canvas.Mask != null && (debugCullMask = EditorGUILayout.Foldout(debugCullMask, "Show culling mask")))
            {
                Rect texRect = GUILayoutUtility.GetAspectRect((float)canvas.Mask.width / canvas.Mask.height);
                EditorGUI.DrawTextureAlpha(texRect, canvas.Mask);
            }

            if(GUILayout.Button("Refresh"))
            {
                canvas.enabled = false;
                canvas.SetToReinitialize();
                canvas.enabled = true;
            }

            if(canvas.EditorErrorLog != null && canvas.EditorErrorLog.Length > 0)
            {
                EditorGUILayout.HelpBox(canvas.EditorErrorLog, MessageType.Error);
            }
        }
    }

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

}
