/* Color Correction Pro - Image Effect for correction of digital graphics values
 * Copyright © 2017-2018 by Uniarts
 * http://www.u3d.as/DGV
 * beta version 0.9
 */

using System.Collections;
using UnityEngine;

namespace Uniarts.ColorCorrection {

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class ColorCorrectionProBase : MonoBehaviour {

        protected Shader correctionShader;
        protected Material correctionMaterial;

        protected Material material {
            get {
                if (correctionMaterial == null) {
                    correctionMaterial = new Material(correctionShader);
                    correctionMaterial.hideFlags = HideFlags.HideAndDontSave;
                }

                return correctionMaterial;
            }
        }

        protected void Start() {

            correctionShader = Shader.Find("Hidden/ColorCorrectionPro");

            if (!SystemInfo.supportsImageEffects) {
                enabled = false;
                return;
            }

            if (!correctionShader && !correctionShader.isSupported) {
                enabled = false;
            }

        }

        protected void OnEnable() {
            if (GetComponent<Camera>() == null) {
                Debug.LogError("This script must be attached to a Camera");
            }
        }

        protected void OnDisable() {
            if (correctionMaterial) {
                DestroyImmediate(correctionMaterial);
            }
        }

    }

}
