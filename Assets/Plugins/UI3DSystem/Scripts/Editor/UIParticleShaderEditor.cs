using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class UIParticleShaderEditor : UI3DShaderEditor
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        base.OnGUI(materialEditor, properties);

        Material material = materialEditor.target as Material;

        EditorGUI.BeginChangeCheck();

        DrawClippingMaskSettings(material);

        DrawBlendingSettings(material);

        if(EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(material);
        }
    }
    

}