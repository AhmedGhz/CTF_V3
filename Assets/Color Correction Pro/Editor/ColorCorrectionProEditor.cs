/* Color Correction Pro - Image Effect for correction of digital graphics values
 * Copyright © 2017-2018 by Uniarts
 * http://www.u3d.as/DGV
 * beta version 0.9
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Uniarts.ColorCorrection {
	
	[CustomEditor(typeof(ColorCorrectionPro))]
	public class ColorCorrectionProEditor : Editor {

		private Texture2D headerTex;
		private Texture2D separatorTex;
		private Texture2D levelsTex;
		private Texture2D pointerUpTex;
		private Texture2D pointerDownTex;
		private Texture2D hueTex;

		SerializedProperty pRed;
		SerializedProperty pGreen;
		SerializedProperty pBlue;

        SerializedProperty pRedCurve;
		SerializedProperty pGreenCurve;
		SerializedProperty pBlueCurve;

        SerializedProperty pEditLevels;

		SerializedProperty pInputLevelBlack;
		SerializedProperty pInputLevelWhite;

		SerializedProperty pOutputLevelBlack;
		SerializedProperty pOutputLevelWhite;

		SerializedProperty pHue;
		SerializedProperty pSaturation;
		SerializedProperty pLightness;	

		SerializedProperty pBrightness;
		SerializedProperty pContrast;
		SerializedProperty pGamma;
		SerializedProperty pSharpness;
        SerializedProperty pTemperature;
        SerializedProperty pThreshold;

        SerializedProperty pColorFilter;
		SerializedProperty pColorFilterMode;
		SerializedProperty pColorForColorFilter;
		SerializedProperty pColorFilterIntensity;

		SerializedProperty pLutFilter;
		SerializedProperty pLutTexture;
		SerializedProperty pLutFilterIntensity;

		SerializedProperty pRampFilter;
		SerializedProperty pRampTexture;
		SerializedProperty pRampFilterIntensity;

		SerializedProperty pFilterMode;

		SerializedProperty pCompareMode;

		int selectedPreset = 14;
		static string[] presets = {
			"Warming Filter (85)", "Warming Filter (LBA)", "Warming Filter (81)",
			"Cooling Filter (80)", "Cooling Filter (LBB)", "Cooling Filter (82)",

			"Red", "Orange", "Yellow", 
			"Green", "Cyan", "Blue", 
			"Violet", "Magenta", "White",

			"Sepia", "Deep Red", "Deep Blue", 
			"Deep Emerald", "Deep Yellow", "Underwater" };

		static float [,] presetsData = { 
			{ 0.925f, 0.541f, 0.0f }, { 0.98f, 0.541f, 0.0f }, { 0.922f, 0.694f, 0.075f }, 
			{ 0.0f, 0.427f, 1.0f }, { 0.0f, 0.365f, 1.0f }, { 0.0f, 0.71f, 1.0f }, 

			{ 0.918f, 0.102f, 0.102f }, { 0.956f, 0.518f, 0.09f }, { 0.976f, 0.89f, 0.11f }, 
			{ 0.098f, 0.788f, 0.098f }, { 0.114f, 0.796f, 0.918f }, { 0.114f, 0.209f, 0.918f }, 
			{ 0.608f, 0.114f, 0.918f }, { 0.89f, 0.094f, 0.89f }, { 1.0f, 1.0f, 1.0f }, 

			{ 0.675f, 0.478f, 0.2f }, { 1.0f, 0.0f, 0.0f }, { 0.0f, 0.133f, 0.804f }, 
			{ 0.0f, 0.553f, 0.0f }, { 1.0f, 0.835f, 0.0f }, { 0.0f, 0.761f, 0.694f }  };

		void OnEnable() {

			try {
				
				pRed = serializedObject.FindProperty("redIntensity");
				pGreen = serializedObject.FindProperty("greenIntensity");
				pBlue = serializedObject.FindProperty("blueIntensity");

                pRedCurve = serializedObject.FindProperty("redCurve");
				pGreenCurve = serializedObject.FindProperty("greenCurve");
				pBlueCurve = serializedObject.FindProperty("blueCurve");           

                pEditLevels = serializedObject.FindProperty("editLevels");	

				pInputLevelBlack = serializedObject.FindProperty("inputLevelBlack");	
				pInputLevelWhite = serializedObject.FindProperty("inputLevelWhite");

				pOutputLevelBlack = serializedObject.FindProperty("outputLevelBlack");
				pOutputLevelWhite = serializedObject.FindProperty("outputLevelWhite");

				pHue = serializedObject.FindProperty("hue");
				pSaturation = serializedObject.FindProperty("saturation");
				pLightness = serializedObject.FindProperty("lightness");

				pBrightness = serializedObject.FindProperty("brightness");
				pContrast = serializedObject.FindProperty("contrast");
				pGamma = serializedObject.FindProperty("gamma");
				pSharpness = serializedObject.FindProperty("sharpness");
                pTemperature = serializedObject.FindProperty("temperature");
                pThreshold = serializedObject.FindProperty("threshold");

                pColorFilter = serializedObject.FindProperty("colorFilter");
				pColorFilterMode = serializedObject.FindProperty("colorFilterMode");
				pColorForColorFilter = serializedObject.FindProperty("colorForColorFilter");
				pColorFilterIntensity = serializedObject.FindProperty("colorFilterIntensity");

				pLutFilter = serializedObject.FindProperty("lutFilter");
				pLutTexture = serializedObject.FindProperty("lutTexture");
				pLutFilterIntensity = serializedObject.FindProperty("lutFilterIntensity");

				pRampFilter = serializedObject.FindProperty("rampFilter");
				pRampTexture = serializedObject.FindProperty("rampTexture");
				pRampFilterIntensity = serializedObject.FindProperty("rampFilterIntensity");

				pFilterMode = serializedObject.FindProperty("filterMode");

				pCompareMode = serializedObject.FindProperty("compareMode");


			} finally { }

		}

		void AddSeparator() {

			EditorGUILayout.Separator();

			if(separatorTex == null) {
				separatorTex = AssetDatabase.LoadAssetAtPath("Assets/Color Correction Pro/Editor/separator.png", typeof( Texture2D )) as Texture2D;
			}

			GUI.DrawTexture(GUILayoutUtility.GetRect(64, 1), separatorTex, ScaleMode.StretchToFill );

			EditorGUILayout.Separator();

		}
			
		public override void OnInspectorGUI() {

			serializedObject.Update();

			float inputLevelBlack = pInputLevelBlack.floatValue;
			float inputLevelWhite = pInputLevelWhite.floatValue;

			float outputLevelBlack = pOutputLevelBlack.floatValue;
			float outputLevelWhite = pOutputLevelWhite.floatValue;

			if (headerTex == null) {
				headerTex = AssetDatabase.LoadAssetAtPath("Assets/Color Correction Pro/Editor/header.png", typeof( Texture2D )) as Texture2D;
			}

			if (levelsTex == null) {
				levelsTex = AssetDatabase.LoadAssetAtPath("Assets/Color Correction Pro/Editor/levels.png", typeof( Texture2D )) as Texture2D;
				levelsTex.wrapMode = TextureWrapMode.Clamp;
			}

			if (pointerUpTex == null) {
				pointerUpTex = AssetDatabase.LoadAssetAtPath("Assets/Color Correction Pro/Editor/pointerUp.png", typeof( Texture2D )) as Texture2D;
			}

			if (pointerDownTex == null) {
				pointerDownTex = AssetDatabase.LoadAssetAtPath("Assets/Color Correction Pro/Editor/pointerDown.png", typeof( Texture2D )) as Texture2D;
			}

			if (hueTex == null) {
				hueTex = AssetDatabase.LoadAssetAtPath("Assets/Color Correction Pro/Editor/hue.png", typeof( Texture2D )) as Texture2D;
				hueTex.wrapMode = TextureWrapMode.Repeat;
			}

			if (headerTex != null) {
				
				EditorGUILayout.Separator();

				Rect rect = new Rect();
				rect.width = 256;
				rect.height = 32;

				Rect rect2 = GUILayoutUtility.GetRect(rect.width, rect.height);

				rect.x = rect2.x;
				rect.y = rect2.y;

				GUI.DrawTexture(rect, headerTex, ScaleMode.ScaleToFit);
					
				EditorGUILayout.Separator();

			}


            EditorGUILayout.PropertyField(pRed, new GUIContent("Red"));      
               
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(" ");
			EditorGUILayout.PropertyField(pRedCurve, new GUIContent (""));
			GUILayout.EndHorizontal();
			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(pGreen, new GUIContent ("Green"));
         
            GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(" ");
			EditorGUILayout.PropertyField(pGreenCurve, new GUIContent (""));
			GUILayout.EndHorizontal();
			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(pBlue, new GUIContent ("Blue"));

            GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(" ");
			EditorGUILayout.PropertyField(pBlueCurve, new GUIContent (""));
			GUILayout.EndHorizontal();

            AddSeparator ();

			GUI.DrawTexture(GUILayoutUtility.GetRect(9, 9), pointerUpTex, ScaleMode.ScaleToFit );

			Rect hueRect = GUILayoutUtility.GetRect(0, 12);

			GUI.DrawTextureWithTexCoords(hueRect, hueTex, new Rect( (pHue.floatValue + 180)/360.0f, 0f, 1.0f, 1.0f));

			GUI.DrawTexture(GUILayoutUtility.GetRect(9, 9), pointerDownTex, ScaleMode.ScaleToFit);
								
			EditorGUILayout.PropertyField(pHue);

			EditorGUILayout.PropertyField(pSaturation);

			EditorGUILayout.PropertyField(pLightness);	

			AddSeparator();

			EditorGUILayout.PropertyField(pBrightness);
			EditorGUILayout.PropertyField(pContrast);
			EditorGUILayout.PropertyField(pGamma);

            AddSeparator();

            EditorGUILayout.PropertyField(pSharpness);
            EditorGUILayout.PropertyField(pTemperature);
            EditorGUILayout.PropertyField(pThreshold);
           
            AddSeparator();

			EditorGUILayout.PropertyField(pEditLevels);

			if (pEditLevels.boolValue) {

				EditorGUILayout.Separator();
				GUILayout.Label ("Input Levels");

				if (levelsTex != null) {

					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label ("", GUILayout.Width (50));
					Rect inputLevelRect = GUILayoutUtility.GetRect (0, 12);
					GUI.DrawTextureWithTexCoords (inputLevelRect, levelsTex, new Rect ((255.0f - inputLevelBlack - inputLevelWhite) / 255.0f, 0f, 1.0f, 1.0f));
					GUILayout.Label ("", GUILayout.Width (50));
					EditorGUILayout.EndHorizontal ();

				}

				EditorGUILayout.BeginHorizontal ();
				inputLevelBlack = EditorGUILayout.FloatField (inputLevelBlack, GUILayout.Width (50));
				EditorGUILayout.MinMaxSlider (new GUIContent (""), ref inputLevelBlack, ref inputLevelWhite, 0.0f, 255.0f);
				inputLevelWhite = EditorGUILayout.FloatField (inputLevelWhite, GUILayout.Width (50));
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Separator ();
				EditorGUILayout.Separator ();

				GUILayout.Label ("Output Levels");

				if (levelsTex != null) {
					
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label ("", GUILayout.Width (50));
					Rect outputLevelRect = GUILayoutUtility.GetRect (0, 12);
					GUI.DrawTextureWithTexCoords (outputLevelRect, levelsTex, new Rect ((outputLevelBlack + outputLevelWhite - 255.0f) / 255.0f, 0f, 1.0f, 1.0f));
					GUILayout.Label ("", GUILayout.Width (50));
					EditorGUILayout.EndHorizontal ();

				}

				EditorGUILayout.BeginHorizontal ();
				outputLevelBlack = EditorGUILayout.FloatField (outputLevelBlack, GUILayout.Width (50));
				EditorGUILayout.MinMaxSlider (new GUIContent (""), ref outputLevelBlack, ref outputLevelWhite, 0.0f, 255.0f);
				outputLevelWhite = EditorGUILayout.FloatField (outputLevelWhite, GUILayout.Width (50));
				EditorGUILayout.EndHorizontal ();

			}

			AddSeparator();

			EditorGUILayout.PropertyField(pColorFilter);

			if (pColorFilter.boolValue) {

				EditorGUILayout.Separator ();

				EditorGUILayout.PropertyField (pColorFilterMode, new GUIContent ("Mode"));

				if (pColorFilterMode.intValue == 0) {

					selectedPreset = EditorGUILayout.Popup ("Color Preset", selectedPreset, presets);

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel ("Color");

					pColorForColorFilter.colorValue = new Color (
						presetsData [selectedPreset, 0],
						presetsData [selectedPreset, 1],
						presetsData [selectedPreset, 2]
					);

					Texture2D tex = new Texture2D (212, 15);
					for (int x = 0; x < tex.width; x++) {
						for (int y = 0; y < tex.height; y++) {
							if (x == 0 || y == 0 || x == (tex.width - 1) || y == (tex.height - 1)) { 
								tex.SetPixel (x, y, new Color (0.5f, 0.5f, 0.5f)); 
							} else {
								tex.SetPixel (x, y, pColorForColorFilter.colorValue);
							}
						}
					}

					tex.Apply ();
					GUI.DrawTexture (GUILayoutUtility.GetRect (0, 15), tex, ScaleMode.StretchToFill);
					EditorGUILayout.EndHorizontal ();

				} else if (pColorFilterMode.intValue == 1) {
					EditorGUILayout.PropertyField (pColorForColorFilter, new GUIContent ("Color"));
				}
				
				EditorGUILayout.PropertyField (pColorFilterIntensity, new GUIContent ("Intensity"));

			}

			AddSeparator();

			EditorGUILayout.PropertyField(pLutFilter, new GUIContent ("LUT Filter"));



			if (pLutFilter.boolValue) {

                Texture2D lut = (Texture2D)pLutTexture.objectReferenceValue;

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("Lookup Texture");               
                    lut = (Texture2D)EditorGUILayout.ObjectField(lut, typeof(Texture2D), false);
                }

                EditorGUILayout.EndHorizontal();

                pLutTexture.objectReferenceValue = lut;

                EditorGUILayout.PropertyField (pLutFilterIntensity, new GUIContent ("Intensity"));
               
            }

			AddSeparator();

			EditorGUILayout.PropertyField(pRampFilter);

			if( pRampFilter.boolValue ) {

                Texture2D ramp = (Texture2D)pRampTexture.objectReferenceValue;

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("Ramp Texture");
                    ramp = (Texture2D)EditorGUILayout.ObjectField(ramp, typeof(Texture2D), false);
                }

                EditorGUILayout.EndHorizontal();

                pRampTexture.objectReferenceValue = ramp;

                EditorGUILayout.PropertyField(pRampFilterIntensity, new GUIContent ("Intensity"));

			}

			AddSeparator();

			EditorGUILayout.PropertyField(pFilterMode, new GUIContent ("Filter Mode"));
			EditorGUILayout.PropertyField(pCompareMode);

			AddSeparator();

			GUILayout.BeginHorizontal();
			{

				EditorGUILayout.HelpBox ("beta version 0.9", MessageType.None);
				GUILayout.FlexibleSpace();

				if (GUILayout.Button ("Reset")) {
					
					ColorCorrectionPro ccp = GameObject.FindObjectOfType( typeof(ColorCorrectionPro) ) as ColorCorrectionPro;

					ccp.enabled = false;

					ccp.Reset();
					pColorFilterMode.intValue = 0;
					selectedPreset = 14;
					ccp.enabled = true;								

				}

			}

			GUILayout.EndHorizontal ();

			EditorGUILayout.Separator();

			pInputLevelBlack.floatValue = inputLevelBlack;
			pInputLevelWhite.floatValue = inputLevelWhite;

			pOutputLevelBlack.floatValue = outputLevelBlack;
			pOutputLevelWhite.floatValue = outputLevelWhite;

			serializedObject.ApplyModifiedProperties();

		}

	}

}
