/* Color Correction Pro - Image Effect for correction of digital graphics values
 * Copyright © 2017-2018 by Uniarts
 * http://www.u3d.as/DGV
 * beta version 0.9
 */

using System;
using System.Collections;
using UnityEngine;

namespace Uniarts.ColorCorrection {

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Color Adjustments/Color Correction Pro")]
	public class ColorCorrectionPro : ColorCorrectionProBase {

    public enum ColorFilterMode {
		Presets = 0,
		Select = 1
	}

	[Range(-1, 1)] public float redIntensity = 0.0f;
	[Range(-1, 1)] public float greenIntensity = 0.0f;
	[Range(-1, 1)] public float blueIntensity = 0.0f;

	public AnimationCurve redCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);
	public AnimationCurve greenCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);
	public AnimationCurve blueCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);
	
	// Texture for Curves
	private Texture2D rgbCurvesTex; 

	[Range(-180, 180)] public float hue = 0.0f;
	[Range(-1, 1)] public float saturation = 0.0f;
	[Range(-1, 1)] public float lightness = 0.0f;

	[Range(-1, 1)] public float brightness = 0.0f;
	[Range(-1, 1)] public float contrast = 0.0f;
	[Range(-1, 1)] public float gamma = 0.0f;
	[Range(-1, 1)] public float sharpness = 0.0f;
    [Range(-0.5f, 0.5f)] public float temperature = 0.0f;
    [Range(0, 0.5f)] public float threshold = 0.0f;
        
    public bool editLevels = false;
	public float inputLevelBlack = 0.0f;
	public float inputLevelWhite = 255.0f;
	public float outputLevelBlack = 0.0f;
	public float outputLevelWhite = 255.0f;

	public bool colorFilter = false;
	public Color colorForColorFilter = Color.white;
	[Range(0, 1)] public float colorFilterIntensity = 0.0f;
	
	public bool lutFilter = false;
	public Texture2D lutTexture = null;
	
	public Texture3D converted3DLut = null;
	[Range(0, 1)] public float lutFilterIntensity = 0.0f;

	public bool rampFilter = false;
	public Texture rampTexture;
	[Range(0, 1)] public float rampFilterIntensity = 0.0f;

	public bool compareMode = false;

	private Material separableBlurMaterial;

    public ColorFilterMode colorFilterMode = ColorFilterMode.Presets;
	
	public FilterMode filterMode = FilterMode.Bilinear;
		
	void Update () {
			
		redIntensity = Mathf.Clamp(redIntensity, -1.0f, 1.0f);
		greenIntensity = Mathf.Clamp(greenIntensity, -1.0f, 1.0f);
		blueIntensity = Mathf.Clamp(blueIntensity, -1.0f, 1.0f);

		hue = Mathf.Clamp(hue, -180.0f, 180.0f);
		saturation = Mathf.Clamp(saturation, -1.0f, 1.0f);
		lightness = Mathf.Clamp(lightness, -1.0f, 1.0f);

		brightness = Mathf.Clamp(brightness, -1.0f, 1.0f);
		contrast = Mathf.Clamp(contrast, -1.0f, 1.0f);
		gamma = Mathf.Clamp(gamma, -1.0f, 1.0f);
		sharpness = Mathf.Clamp(sharpness, -1.0f, 1.0f);
        temperature = Mathf.Clamp(temperature, -0.5f, 0.5f);

        inputLevelBlack = Mathf.Clamp(inputLevelBlack, 0, 255);
		inputLevelWhite = Mathf.Clamp(inputLevelWhite, 0, 255);
		outputLevelBlack = Mathf.Clamp(outputLevelBlack, 0, 255);
		outputLevelWhite = Mathf.Clamp(outputLevelWhite, 0, 255);
		
		colorFilterIntensity = Mathf.Clamp(colorFilterIntensity, 0, 1);
		lutFilterIntensity = Mathf.Clamp(lutFilterIntensity, 0, 1);
		rampFilterIntensity = Mathf.Clamp(rampFilterIntensity, 0, 1);

        if ( lutTexture != null ) {
            Convert(lutTexture);
        } else if (lutTexture == null) {
            converted3DLut = null;
        }

    }
	
	public void Reset() {

		redIntensity = 0;
		greenIntensity = 0;
		blueIntensity = 0;

		redCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);
		greenCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);
		blueCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);

        hue = 0.0f;
		saturation = 0.0f;
		lightness = 0.0f;
		brightness = 0.0f;
		contrast = 0.0f;
		gamma = 0.0f;
		sharpness = 0.0f;
        temperature = 0.0f;
        threshold = 0.0f;

        inputLevelBlack = 0.0f;
        inputLevelWhite = 255.0f;
        outputLevelBlack = 0.0f;
        outputLevelWhite = 255.0f;
        editLevels = false;

        colorFilterIntensity = 0.0f;
        colorFilter = false;
        lutFilterIntensity = 0.0f;
        lutTexture = null;
        lutFilter = false;
        rampFilterIntensity = 0.0f;
        rampTexture = null;
        rampFilter = false;

        filterMode = FilterMode.Bilinear;
		compareMode = false;

	}

	void SetRGBCurvesColor() {
			if (!rgbCurvesTex) {
				rgbCurvesTex = new Texture2D (256, 4, TextureFormat.ARGB32, false, true);
			}

			rgbCurvesTex.hideFlags = HideFlags.DontSave;
			rgbCurvesTex.wrapMode = TextureWrapMode.Clamp;

			if (redCurve != null && greenCurve != null && blueCurve != null) {
				for (float i = 0.0f; i <= 1.0f; i += 1.0f / 255.0f) {
					float rCh = Mathf.Clamp (redCurve.Evaluate(i), 0.0f, 1.0f);
					float gCh = Mathf.Clamp (greenCurve.Evaluate(i), 0.0f, 1.0f);
					float bCh = Mathf.Clamp (blueCurve.Evaluate(i), 0.0f, 1.0f);

					rgbCurvesTex.SetPixel ((int) Mathf.Floor(i*255.0f), 0, new Color(rCh,rCh,rCh) );
					rgbCurvesTex.SetPixel ((int) Mathf.Floor(i*255.0f), 1, new Color(gCh,gCh,gCh) );
					rgbCurvesTex.SetPixel ((int) Mathf.Floor(i*255.0f), 2, new Color(bCh,bCh,bCh) );
				}
				rgbCurvesTex.Apply();
				material.SetTexture("_RgbTex", rgbCurvesTex);
			}
	}

	// --------------------------------- LUT --------------------------------------------------

		public void SetIdentityLut() {

			if(lutTexture) {
				
				int lut3DSize = lutTexture.height;
				var newColor = new Color[ lut3DSize * lut3DSize * lut3DSize ];
				float oneOverLut3DSize = 1.0f / (1.0f * lut3DSize - 1.0f);

				for(int i = 0; i < lut3DSize; i++) {
					for(int j = 0; j < lut3DSize; j++) {
						for(int k = 0; k < lut3DSize; k++) {
							newColor[i + (j*lut3DSize) + (k*lut3DSize*lut3DSize)] = 
								new Color( (float)i * oneOverLut3DSize, (float)j * oneOverLut3DSize, (float)k * oneOverLut3DSize, 1.0f);
						}
					}
				}

				if (converted3DLut != null) {
					DestroyImmediate(converted3DLut);
				}

				converted3DLut = new Texture3D (lut3DSize, lut3DSize, lut3DSize, TextureFormat.ARGB32, false);
				converted3DLut.wrapMode = TextureWrapMode.Clamp;
				converted3DLut.SetPixels(newColor);
				converted3DLut.Apply ();
			
			}
		}

		public bool Valid3DSize( Texture2D texture2D ) {
			if (!texture2D) { return false; }
			int h = texture2D.height;
			if (h != Mathf.FloorToInt(Mathf.Sqrt(texture2D.width))) {
				return false;
			}
			return true;
		}

        public void Convert(Texture2D aLutTexture) {

            if (aLutTexture) {

                int lut3DSize = aLutTexture.height;

                if (!Valid3DSize(aLutTexture)) {
                    Debug.LogWarning("The given 2D texture " + aLutTexture.name + " cannot be used as a 3D LUT. " +
                        "The height of texture must equal the square root of the width.");
                    return;
                }

                var c = aLutTexture.GetPixels();
                var newColor = new Color[c.Length];

                for (int i = 0; i < lut3DSize; i++) {
                    for (int j = 0; j < lut3DSize; j++) {
                        for (int k = 0; k < lut3DSize; k++) {

                            newColor[i + (j * lut3DSize * lut3DSize) + (k * lut3DSize)] = c[i + (j * lut3DSize) + (k * lut3DSize * lut3DSize)];

                        }
                    }
                }

                if (converted3DLut != null) {
                    DestroyImmediate(converted3DLut);
                }

                converted3DLut = new Texture3D(lut3DSize, lut3DSize, lut3DSize, TextureFormat.ARGB32, false);
                converted3DLut.wrapMode = TextureWrapMode.Clamp;
                converted3DLut.SetPixels(newColor);

                converted3DLut.Apply();
                
            } else {
				Debug.LogError ("Couldn't color correct with 3D LUT texture. Image Effect will be disabled.");
			}
		}

	//--------------------------------------------------------------------------

	void OnRenderImage(RenderTexture sourceTexture, RenderTexture destinationTexture) {
			
		if(correctionShader != null) {

			sourceTexture.filterMode = filterMode;

			material.SetTexture("_MainTex", sourceTexture);

            material.SetFloat("_Red", redIntensity + 1.0f);
			material.SetFloat("_Green", greenIntensity + 1.0f);
			material.SetFloat("_Blue", blueIntensity + 1.0f);

			SetRGBCurvesColor();

			if( editLevels ) {
				material.SetInt("_EditLevels", 1);	
			} else if( !editLevels ) {
				material.SetInt("_EditLevels", 0);		
			}

			if( colorFilter ) {
				material.SetInt("_ColorFilter", 1);	
			} else if( !colorFilter ) {
				material.SetInt("_ColorFilter", 0);		
			}

			material.SetFloat("_InputBlack", inputLevelBlack);
			material.SetFloat("_InputWhite", inputLevelWhite);
			material.SetFloat("_OutputBlack", outputLevelBlack);
			material.SetFloat("_OutputWhite", outputLevelWhite);

			material.SetFloat("_Hue", hue );
			material.SetFloat("_Saturation", saturation + 1.0f);
			material.SetFloat("_Lightness", lightness + 1.0f);

			material.SetFloat("_Brightness", brightness + 1.0f);
			material.SetFloat("_Contrast", contrast + 1.0f);
			material.SetFloat("_Gamma", gamma + 1.0f);
            material.SetFloat("_Temperature", temperature + 1.0f);
            material.SetFloat("_Threshold", threshold);
                
            material.SetColor("_ColorForColorFilter", colorForColorFilter);
			material.SetFloat("_ColorFilterIntensity", colorFilterIntensity);

			if (converted3DLut == null) {
				SetIdentityLut();
			}

			if ( lutFilter && converted3DLut != null ) {

				int lutSize = converted3DLut.width;
				material.SetFloat ("_LutScale", (lutSize - 1) / (1.0f * lutSize));
				material.SetFloat ("_LutOffset", 1.0f / (2.0f * lutSize));
				material.SetFloat ("_LutIntensity", lutFilterIntensity);
				material.SetTexture ("_LutTex", converted3DLut);

				material.SetInt("_LutFilter", 1);

			} else if ( !lutFilter ) {
				material.SetInt("_LutFilter", 0);		
			}

			if ( rampFilter && rampTexture != null ) {
				material.SetInt("_RampFilter", 1);
				material.SetTexture("_RampTex", rampTexture);
				material.SetFloat("_RampIntensity", rampFilterIntensity);
				
			} else if( !rampFilter ) {
				material.SetInt("_RampFilter", 0);		
			}

			if ( compareMode ) {
				material.SetInt("_CompareMode", 1);	
			} else if( !compareMode ) {
				material.SetInt("_CompareMode", 0);		
			}
							
			// --------------------- Sharpness ----------------------

				int rtW = sourceTexture.width;
				int rtH = sourceTexture.height;

				RenderTexture color2 = RenderTexture.GetTemporary (rtW/2, rtH/2, 0);

				Graphics.Blit (sourceTexture, color2);
				RenderTexture color4a = RenderTexture.GetTemporary (rtW/4, rtH/4, 0);
				Graphics.Blit (color2, color4a);
				RenderTexture.ReleaseTemporary (color2);

				material.SetTexture ("_MainTexBlurred", color4a);
				material.SetFloat("_Sharpness", sharpness );

			// ------------------------------------------------------

			int pass = QualitySettings.activeColorSpace == ColorSpace.Linear ? 1 : 0;
			Graphics.Blit(sourceTexture, destinationTexture, material, pass);

			// ------------------- Sharpness ------------------------
				RenderTexture.ReleaseTemporary (color4a);
			// ------------------------------------------------------

		} else {
			Graphics.Blit(sourceTexture, destinationTexture);
		}
	}


  }

}
