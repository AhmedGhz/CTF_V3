using System.Collections;
using System.Collections.Generic;
using UI3DEnums;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CanEditMultipleObjects()]
[CustomEditor(typeof(UI3DDepthObject))]
public class UI3DDepthObjectEditor : Editor
{
    private UI3DDepthObject depthObj;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        depthObj = (UI3DDepthObject)target;

        if(depthObj)
        {
            EditorGUI.BeginChangeCheck();

            depthObj.source = (UI3DMaskSourceMode)EditorGUILayout.EnumPopup("Graphic source", depthObj.source);
            depthObj.renderMode = (UI3DMaskRenderMode)EditorGUILayout.EnumPopup("Render mode", depthObj.renderMode);
            depthObj.alphaMode = (UI3DMaskAlphaMode)EditorGUILayout.EnumPopup("Alpha mode", depthObj.alphaMode);

            if(depthObj.source == UI3DMaskSourceMode.MaskTexture)
                depthObj.maskTexture = EditorGUILayout.ObjectField("Texture", depthObj.maskTexture, typeof(Texture2D), false) as Texture2D;

            if(depthObj.alphaMode == UI3DMaskAlphaMode.AlphaTest)
            {
                depthObj.alphaTestTreshold = EditorGUILayout.Slider("Alpha test treshold", depthObj.alphaTestTreshold, 0f, 1f);
            }

            if(depthObj.alphaMode == UI3DMaskAlphaMode.Dithering)
            {
                depthObj.ditheringSteps = EditorGUILayout.IntSlider("Alpha dithering steps", depthObj.ditheringSteps, 1, 255);
            }

            if(depthObj.renderMode == UI3DMaskRenderMode.CullingMask)
            {
                depthObj.cullingMaskVal = EditorGUILayout.IntSlider("Culling mask value(layer)", depthObj.cullingMaskVal, 0, 255);
            }

            depthObj.willRectResizeInRuntime = EditorGUILayout.Toggle("Will Rect resize in runtime", depthObj.willRectResizeInRuntime);

            if(EditorGUI.EndChangeCheck())
            {
                depthObj.RefreshRenderer(true);
                if(!Application.isPlaying)
                {
                    EditorUtility.SetDirty(depthObj);
                    if(depthObj.gameObject.scene.IsValid())
                        EditorSceneManager.MarkSceneDirty(depthObj.gameObject.scene);
                }
            }
            
            if(depthObj.EditorErrorLog != null && depthObj.EditorErrorLog.Length > 0)
            {
                EditorGUILayout.HelpBox(depthObj.EditorErrorLog, MessageType.Error);
            }
        }
    }

}
