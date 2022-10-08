using UnityEngine;
using System;
using System.Collections;

namespace XFrameEffect {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("")]
    public class PostFrameCompositor : MonoBehaviour {
        [HideInInspector]
        public XFrameManager
                        xFrameManager;
        Material bMatDefault, bMatSharp;

        #region Game loop events

        // Creates a private material used to the effect
        void OnEnable() {
            bMatDefault = new Material(Shader.Find("XFrame/XFrame Default"));
            bMatDefault.hideFlags = HideFlags.DontSave;
            bMatSharp = new Material(Shader.Find("XFrame/XFrame Sharp"));
            bMatSharp.hideFlags = HideFlags.DontSave;
        }

        void Start() {
            if (xFrameManager == null) {
                Debug.Log("XFrameManager not initialized - add X-Frame to the main camera or remove XFrameCamera gameobject.");
            }
        }

        void OnDisable() {
            if (bMatDefault != null) {
                DestroyImmediate(bMatDefault);
                bMatDefault = null;
            }
            if (bMatSharp != null) {
                DestroyImmediate(bMatSharp);
                bMatSharp = null;
            }
        }

        void OnPostRender() {
            Material bMat = xFrameManager.sharpen ? bMatSharp : bMatDefault;
            if (bMat == null || xFrameManager == null || xFrameManager.rt == null || xFrameManager.method == XFRAME_DOWNSAMPLING_METHOD.Disabled) {
                return;
            }
            Graphics.Blit(xFrameManager.rt, null, bMat);
        }

        #endregion

    }

}