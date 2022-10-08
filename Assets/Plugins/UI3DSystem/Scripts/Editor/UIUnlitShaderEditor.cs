using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class UIUnlitShaderEditor : UI3DShaderEditor
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        base.OnGUI(materialEditor, properties);

        Material material = materialEditor.target as Material;

        EditorGUI.BeginChangeCheck();

        DrawClippingMaskSettings(material);
        
        if(EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(material);
        }
    }
}