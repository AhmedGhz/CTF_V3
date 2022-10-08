using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[ExecuteInEditMode]
public class UIBlur : MonoBehaviour, IMaterialModifier 
{
    [SerializeField, Range(0, 10)] private float blurSize = 5;

    private Material myMaterial;

    void OnEnable() {
        myMaterial = new Material(Shader.Find("Vertigo/UIGaussianBlur"));
    }

    void Update() {
        myMaterial.SetFloat("_blurSizeXY", blurSize);
    }

    public Material GetModifiedMaterial(Material baseMaterial) {
        return myMaterial;
    }
}
