//RealToonGUI
//MJQStudioWorks
//2018

using UnityEngine;
using UnityEditor;
using System;

public class RealToonShaderGUI : ShaderGUI
{
    #region foldout bools variable

    static bool ShowTextureColor;
    static bool ShowNormalMap;
    static bool ShowTransparency;
    static bool ShowCutout;
    static bool ShowColorAdjustment;
    static bool ShowOutline;
    static bool ShowSelfLit;
    static bool ShowGloss;
    static bool ShowShadow;
    static bool ShowLighting;
    static bool ShowReflection;
    static bool ShowFReflection;
    static bool ShowRimLight;
    static bool ShowDepth;
    static bool ShowSeeThrough;
    static bool ShowDisableEnable;
    static bool ShowTessellation;

    #endregion

    #region Variables

    string shader_name;
    string shader_type;

    #endregion

    #region Material Properties Variables

    MaterialProperty _DoubleSided;

    MaterialProperty _RefractionIntensity;
    
    MaterialProperty _TextureIntesnity;
    MaterialProperty _MainTex;
    MaterialProperty _TexturePatternStyle;
    MaterialProperty _ReduceTextureQuality;
    MaterialProperty _MainColor;
    MaterialProperty _MainColorAffectTexture;
    MaterialProperty _EnableTextureTransparent;

    MaterialProperty _Cutout;
    MaterialProperty _UseSecondaryCutout;
    MaterialProperty _SecondaryCutout;
    MaterialProperty _AlphaBaseCutout;

    MaterialProperty _Opacity;
    MaterialProperty _MaskTransparency;

    MaterialProperty _NormalMap;
    MaterialProperty _NormalMapIntensity;

    MaterialProperty _Saturation;

    MaterialProperty _OutlineWidth;
    MaterialProperty _OutlineWidthControl;
    MaterialProperty _OutlineExtrudeMethod;
    MaterialProperty _ReduceOutlineBackFace;
    MaterialProperty _EOPO;
    MaterialProperty _OutlineOffset;
    MaterialProperty _OutlineColor;
    MaterialProperty _NoisyOutlineIntensity;
    MaterialProperty _DynamicNoisyOutline;
    MaterialProperty _LightAffectOutlineColor;
    MaterialProperty _OutlineWidthAffectedByViewDistance;
    MaterialProperty _VertexColorRedAffectOutlineWitdh;

    MaterialProperty _SelfLitIntensity;
    MaterialProperty _SelfLitColor;
    MaterialProperty _SelfLitPower;
    MaterialProperty _SelfLitHighContrast;
    MaterialProperty _MaskSelfLit;

    MaterialProperty _GlossIntensity;
    MaterialProperty _Glossiness;
    MaterialProperty _GlossSoftness;
    MaterialProperty _GlossColor;
    MaterialProperty _GlossColorPower;
    MaterialProperty _MaskGloss;

    MaterialProperty _GlossTexture;
    MaterialProperty _GlossTextureSoftness;
    MaterialProperty _GlossTextureRotate;
    MaterialProperty _GlossTextureFollowObjectRotation;
    MaterialProperty _GlossTextureFollowLight;

    MaterialProperty _OverallShadowColor;
    MaterialProperty _OverallShadowColorPower;
    MaterialProperty _SelfShadowShadowTAtViewDirection;

    MaterialProperty _HighlightColor;
    MaterialProperty _HighlightColorPower;

    MaterialProperty _SelfShadowRealtimeShadowIntensity;
    MaterialProperty _SelfShadowIntensity;
    MaterialProperty _SelfShadowThreshold;
    MaterialProperty _VertexColorGreenControlSelfShadowThreshold;
    MaterialProperty _SelfShadowHardness;
    MaterialProperty _SelfShadowRealTimeShadowColor;
    MaterialProperty _SelfShadowRealTimeShadowColorPower;
    MaterialProperty _SelfShadowColor;
    MaterialProperty _SelfShadowColorPower;
    MaterialProperty _SelfShadowAffectedByLightShadowStrength;

    MaterialProperty _SmoothObjectNormal;
    MaterialProperty _VertexColorBlueControlSmoothObjectNormal;
    MaterialProperty _XYZPosition;
    MaterialProperty _XYZHardness;
    MaterialProperty _ShowNormal;

    MaterialProperty _ShadowColorTexture;
    MaterialProperty _ShadowColorTexturePower;

    MaterialProperty _ShadowTIntensity;
    MaterialProperty _ShadowT;
    MaterialProperty _ShadowTLightThreshold;
    MaterialProperty _ShadowTShadowThreshold;
    MaterialProperty _ShadowTColor;
    MaterialProperty _ShadowTColorPower;
    MaterialProperty _ShadowTHardness;
    MaterialProperty _N_F_STIS;
    MaterialProperty _N_F_STIAL;
    MaterialProperty _ShowInAmbientLightIntensity;
    MaterialProperty _ShowInAmbientLightShadowThreshold;
    MaterialProperty _LightFalloffAffectShadowT;

    MaterialProperty _PTexture;
    MaterialProperty _PTexturePower;

    MaterialProperty _GIFlatShade;
    MaterialProperty _GIShadeThreshold;
    MaterialProperty _LightAffectShadow;
    MaterialProperty _LightIntensity;
    MaterialProperty _EnvironmentalLightingIntensity;
    MaterialProperty _PointSpotlightIntensity;
    MaterialProperty _LightFalloffSoftness;
    MaterialProperty _ReduceShadowPointLight;
    MaterialProperty _ReduceShadowSpotDirectionalLight;

    MaterialProperty _CustomLightDirectionIntensity;
    MaterialProperty _CustomLightDirectionFollowObjectRotation;
    MaterialProperty _CustomLightDirection;

    MaterialProperty _ReflectionIntensity;
    MaterialProperty _ReflectionRoughtness;
    MaterialProperty _MaskReflection;
    MaterialProperty _UseFReflection;
    MaterialProperty _FReflection;
    
    MaterialProperty _RimLightUnfill;
    MaterialProperty _RimLightColor;
    MaterialProperty _RimLightColorPower;
    MaterialProperty _RimLightSoftness;
    MaterialProperty _RimLightInLight;
    MaterialProperty _LightAffectRimLightColor;

    MaterialProperty _Depth;
    MaterialProperty _DepthEdgeHardness;
    MaterialProperty _DepthColor;
    MaterialProperty _DepthColorPower;

    MaterialProperty _TessellationSmoothness;
    MaterialProperty _TessellationTransition;
    MaterialProperty _TessellationNear;
    MaterialProperty _TessellationFar;

    MaterialProperty _RefVal;
    MaterialProperty _Oper;
    MaterialProperty _Compa;

    MaterialProperty _N_F_CO;
    MaterialProperty _N_F_O;
    MaterialProperty _N_F_CA;
    MaterialProperty _N_F_SL;
    MaterialProperty _N_F_GLO;
    MaterialProperty _N_F_GLOT;
    MaterialProperty _N_F_SS;
    MaterialProperty _N_F_SON;
    MaterialProperty _N_F_SCT;
    MaterialProperty _N_F_ST;
    MaterialProperty _N_F_PT;
    MaterialProperty _N_F_CLD;
    MaterialProperty _N_F_R;
    MaterialProperty _N_F_FR;
    MaterialProperty _N_F_RL;
    MaterialProperty _N_F_D;
    MaterialProperty _N_F_HDLS;
    MaterialProperty _N_F_HPSS;
    MaterialProperty _ZWrite;

    #endregion

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        //This Material
        Material targetMat = materialEditor.target as Material;

        //Settings

        EditorGUIUtility.labelWidth += (Screen.width / 2) - 60;
        EditorGUIUtility.fieldWidth = 66;

        //Content

        #region Shader Name Switch

        switch (targetMat.shader.name)
        {
            case "RealToon/Version 5/Default/Default":
                shader_name = "default_d";
                shader_type = "Default";
                break;
            case "RealToon/Version 5/Default/Fade Transparency":
                shader_name = "default_ft";
                shader_type = "Fade Transperancy";
                break;
            case "RealToon/Version 5/Default/Refraction":
                shader_name = "default_ref";
                shader_type = "Refraction";
                break;
            case "RealToon/Version 5/Tessellation/Default":
                shader_name = "tessellation_d";
                shader_type = "Tessellation - Default";
                break;
            case "RealToon/Version 5/Tessellation/Fade Transparency":
                shader_name = "tessellation_ft";
                shader_type = "Tessellation - Fade Transparency";
                break;
            case "RealToon/Version 5/Tessellation/Refraction":
                shader_name = "tessellation_ref";
                shader_type = "Tessellation - Refraction";
                break;
            default:
                shader_name = string.Empty;
                shader_type = string.Empty;
                break;
        }


        #endregion

        #region Material Properties


        _DoubleSided = ShaderGUI.FindProperty("_DoubleSided", properties);

        if (shader_name == "default_ref" || shader_name == "tessellation_ref")
        {
            _RefractionIntensity = ShaderGUI.FindProperty("_RefractionIntensity", properties);
            _TextureIntesnity = ShaderGUI.FindProperty("_TextureIntesnity", properties);
            _MainColorAffectTexture = ShaderGUI.FindProperty("_MainColorAffectTexture", properties);
        }
        else if (shader_name == "default_ft" || shader_name == "tessellation_ft" || shader_name == "default_d" || shader_name == "tessellation_d")
        {
            _RefractionIntensity = null;
            _TextureIntesnity = null;
            _MainColorAffectTexture = null;
        }

        _MainTex = ShaderGUI.FindProperty("_MainTex", properties);
        _ReduceTextureQuality = ShaderGUI.FindProperty("_ReduceTextureQuality", properties);
        _TexturePatternStyle = ShaderGUI.FindProperty("_TexturePatternStyle", properties);

        _MainColor = ShaderGUI.FindProperty("_MainColor", properties);

        if (shader_name == "default_d" || shader_name == "tessellation_d")
        {
            _Cutout = ShaderGUI.FindProperty("_Cutout", properties);
            _UseSecondaryCutout = ShaderGUI.FindProperty("_UseSecondaryCutout", properties);
            _SecondaryCutout = ShaderGUI.FindProperty("_SecondaryCutout", properties);
            _AlphaBaseCutout = ShaderGUI.FindProperty("_AlphaBaseCutout", properties);
        }
        else if (shader_name == "default_ft" || shader_name == "tessellation_ft" || shader_name == "default_ref" || shader_name == "tessellation_ref")
        {
            _Cutout = null;
            _UseSecondaryCutout = null;
            _SecondaryCutout = null;
            _AlphaBaseCutout = null;
        }

        if (shader_name == "default_d" || shader_name == "tessellation_d")
        {
            _EnableTextureTransparent = ShaderGUI.FindProperty("_EnableTextureTransparent", properties);
            _Opacity = null;
            _MaskTransparency = null;

        }
        else if (shader_name == "default_ft" || shader_name == "tessellation_ft")
        {
            _EnableTextureTransparent = null;
            _Opacity = ShaderGUI.FindProperty("_Opacity", properties);
            _MaskTransparency = ShaderGUI.FindProperty("_MaskTransparency", properties);
        }

        _NormalMap = ShaderGUI.FindProperty("_NormalMap", properties);
        _NormalMapIntensity = ShaderGUI.FindProperty("_NormalMapIntensity", properties);

        _Saturation = ShaderGUI.FindProperty("_Saturation", properties);

        if (shader_name == "default_d" || shader_name == "tessellation_d" || shader_name == "default_ft" || shader_name == "tessellation_ft")
        {
            _OutlineWidth = ShaderGUI.FindProperty("_OutlineWidth", properties);
            _OutlineWidthControl = ShaderGUI.FindProperty("_OutlineWidthControl", properties);

            if (shader_name == "default_ft" || shader_name == "tessellation_ft")
            {
                _ReduceOutlineBackFace = ShaderGUI.FindProperty("_ReduceOutlineBackFace", properties);
            }
            else if (shader_name == "default_ref" || shader_name == "tessellation_ref" || shader_name == "default_d" || shader_name == "tessellation_d")
            {
                _ReduceOutlineBackFace = null;
            }

            _OutlineExtrudeMethod = ShaderGUI.FindProperty("_OutlineExtrudeMethod", properties);
            _EOPO = ShaderGUI.FindProperty("_EOPO", properties);
            _OutlineOffset = ShaderGUI.FindProperty("_OutlineOffset", properties);
            _OutlineColor = ShaderGUI.FindProperty("_OutlineColor", properties);
            _NoisyOutlineIntensity = ShaderGUI.FindProperty("_NoisyOutlineIntensity", properties);
            _DynamicNoisyOutline = ShaderGUI.FindProperty("_DynamicNoisyOutline", properties);
            _LightAffectOutlineColor = ShaderGUI.FindProperty("_LightAffectOutlineColor", properties);
            _OutlineWidthAffectedByViewDistance = ShaderGUI.FindProperty("_OutlineWidthAffectedByViewDistance", properties);
            _VertexColorRedAffectOutlineWitdh = ShaderGUI.FindProperty("_VertexColorRedAffectOutlineWitdh", properties);
        }
        else if (shader_name == "default_ref" || shader_name == "tessellation_ref")
        {
            _OutlineWidth = null;
            _OutlineWidthControl = null;
            _ReduceOutlineBackFace = null;
            _OutlineExtrudeMethod = null;
            _EOPO = null;
            _OutlineOffset = null;
            _OutlineColor = null;
            _NoisyOutlineIntensity = null;
            _DynamicNoisyOutline = null;
            _LightAffectOutlineColor = null;
            _OutlineWidthAffectedByViewDistance = null;
            _VertexColorRedAffectOutlineWitdh = null;
        }

        _SelfLitIntensity = ShaderGUI.FindProperty("_SelfLitIntensity", properties);
        _SelfLitColor = ShaderGUI.FindProperty("_SelfLitColor", properties);
        _SelfLitPower = ShaderGUI.FindProperty("_SelfLitPower", properties);
        _SelfLitHighContrast = ShaderGUI.FindProperty("_SelfLitHighContrast", properties);
        _MaskSelfLit = ShaderGUI.FindProperty("_MaskSelfLit", properties);

        _GlossIntensity = ShaderGUI.FindProperty("_GlossIntensity", properties);
        _Glossiness = ShaderGUI.FindProperty("_Glossiness", properties);
        _GlossSoftness = ShaderGUI.FindProperty("_GlossSoftness", properties);
        _GlossColor = ShaderGUI.FindProperty("_GlossColor", properties);
        _GlossColorPower = ShaderGUI.FindProperty("_GlossColorPower", properties);
        _MaskGloss = ShaderGUI.FindProperty("_MaskGloss", properties);

        _GlossTexture = ShaderGUI.FindProperty("_GlossTexture", properties);
        _GlossTextureSoftness = ShaderGUI.FindProperty("_GlossTextureSoftness", properties);
        _GlossTextureRotate = ShaderGUI.FindProperty("_GlossTextureRotate", properties);
        _GlossTextureFollowObjectRotation = ShaderGUI.FindProperty("_GlossTextureFollowObjectRotation", properties);
        _GlossTextureFollowLight = ShaderGUI.FindProperty("_GlossTextureFollowLight", properties);

        _OverallShadowColor = ShaderGUI.FindProperty("_OverallShadowColor", properties);
        _OverallShadowColorPower = ShaderGUI.FindProperty("_OverallShadowColorPower", properties);
        _SelfShadowShadowTAtViewDirection = ShaderGUI.FindProperty("_SelfShadowShadowTAtViewDirection", properties);

        _HighlightColor = ShaderGUI.FindProperty("_HighlightColor", properties);
        _HighlightColorPower = ShaderGUI.FindProperty("_HighlightColorPower", properties);

        _SelfShadowThreshold = ShaderGUI.FindProperty("_SelfShadowThreshold", properties);
        _VertexColorGreenControlSelfShadowThreshold = ShaderGUI.FindProperty("_VertexColorGreenControlSelfShadowThreshold", properties);
        _SelfShadowHardness = ShaderGUI.FindProperty("_SelfShadowHardness", properties);

        if (shader_name == "default_d" || shader_name == "tessellation_d")
        {
            _SelfShadowRealtimeShadowIntensity = ShaderGUI.FindProperty("_SelfShadowRealtimeShadowIntensity", properties);

            _SelfShadowRealTimeShadowColor = ShaderGUI.FindProperty("_SelfShadowRealTimeShadowColor", properties);
            _SelfShadowRealTimeShadowColorPower = ShaderGUI.FindProperty("_SelfShadowRealTimeShadowColorPower", properties);

            _SelfShadowIntensity = null;
            _SelfShadowColor = null;
            _SelfShadowColorPower = null;
        }
        else if (shader_name == "default_ft" || shader_name == "tessellation_ft" || shader_name == "default_ref" || shader_name == "tessellation_ref")
        {
            _SelfShadowRealtimeShadowIntensity = null;
            _SelfShadowRealTimeShadowColor = null;
            _SelfShadowRealTimeShadowColorPower = null;

            _SelfShadowIntensity = ShaderGUI.FindProperty("_SelfShadowIntensity", properties);
            _SelfShadowColor = ShaderGUI.FindProperty("_SelfShadowColor", properties);
            _SelfShadowColorPower = ShaderGUI.FindProperty("_SelfShadowColorPower", properties);
        }

        _SelfShadowAffectedByLightShadowStrength = ShaderGUI.FindProperty("_SelfShadowAffectedByLightShadowStrength", properties);

        _SmoothObjectNormal = ShaderGUI.FindProperty("_SmoothObjectNormal", properties);
        _VertexColorBlueControlSmoothObjectNormal = ShaderGUI.FindProperty("_VertexColorBlueControlSmoothObjectNormal", properties);
        _XYZPosition = ShaderGUI.FindProperty("_XYZPosition", properties);
        _XYZHardness = ShaderGUI.FindProperty("_XYZHardness", properties);
        _ShowNormal = ShaderGUI.FindProperty("_ShowNormal", properties);

        _ShadowColorTexture = ShaderGUI.FindProperty("_ShadowColorTexture", properties);
        _ShadowColorTexturePower = ShaderGUI.FindProperty("_ShadowColorTexturePower", properties);

        _ShadowTIntensity = ShaderGUI.FindProperty("_ShadowTIntensity", properties);
        _ShadowT = ShaderGUI.FindProperty("_ShadowT", properties);
        _ShadowTLightThreshold = ShaderGUI.FindProperty("_ShadowTLightThreshold", properties);
        _ShadowTShadowThreshold = ShaderGUI.FindProperty("_ShadowTShadowThreshold", properties);
        _ShadowTColor = ShaderGUI.FindProperty("_ShadowTColor", properties);
        _ShadowTColorPower = ShaderGUI.FindProperty("_ShadowTColorPower", properties);
        _ShadowTHardness = ShaderGUI.FindProperty("_ShadowTHardness", properties);
        _N_F_STIS = ShaderGUI.FindProperty("_N_F_STIS", properties);
        _N_F_STIAL = ShaderGUI.FindProperty("_N_F_STIAL", properties);
        _ShowInAmbientLightIntensity = ShaderGUI.FindProperty("_ShowInAmbientLightIntensity", properties);
        _ShowInAmbientLightShadowThreshold = ShaderGUI.FindProperty("_ShowInAmbientLightShadowThreshold", properties);

        _LightFalloffAffectShadowT = ShaderGUI.FindProperty("_LightFalloffAffectShadowT", properties);

        _PTexture = ShaderGUI.FindProperty("_PTexture", properties);
        _PTexturePower = ShaderGUI.FindProperty("_PTexturePower", properties);

        _GIFlatShade = ShaderGUI.FindProperty("_GIFlatShade", properties);
        _GIShadeThreshold = ShaderGUI.FindProperty("_GIShadeThreshold", properties);
        _LightAffectShadow = ShaderGUI.FindProperty("_LightAffectShadow", properties);
        _LightIntensity = ShaderGUI.FindProperty("_LightIntensity", properties);

        _EnvironmentalLightingIntensity = ShaderGUI.FindProperty("_EnvironmentalLightingIntensity", properties);

        _PointSpotlightIntensity = ShaderGUI.FindProperty("_PointSpotlightIntensity", properties);
        _LightFalloffSoftness = ShaderGUI.FindProperty("_LightFalloffSoftness", properties);

        if (shader_name == "default_d" || shader_name == "tessellation_d")
        {
            _ReduceShadowPointLight = ShaderGUI.FindProperty("_ReduceShadowPointLight", properties);
            _ReduceShadowSpotDirectionalLight = ShaderGUI.FindProperty("_ReduceShadowSpotDirectionalLight", properties);
        }
        else if (shader_name == "default_ft" || shader_name == "tessellation_ft" || shader_name == "default_ref" || shader_name == "tessellation_ref")
        {
            _ReduceShadowPointLight = null;
            _ReduceShadowSpotDirectionalLight = null;
        }

        _CustomLightDirectionIntensity = ShaderGUI.FindProperty("_CustomLightDirectionIntensity", properties);
        _CustomLightDirectionFollowObjectRotation = ShaderGUI.FindProperty("_CustomLightDirectionFollowObjectRotation", properties);
        _CustomLightDirection = ShaderGUI.FindProperty("_CustomLightDirection", properties);

        _ReflectionIntensity = ShaderGUI.FindProperty("_ReflectionIntensity", properties);
        _ReflectionRoughtness = ShaderGUI.FindProperty("_ReflectionRoughtness", properties);
        _MaskReflection = ShaderGUI.FindProperty("_MaskReflection", properties);
        _UseFReflection = ShaderGUI.FindProperty("_UseFReflection", properties);
        _FReflection = ShaderGUI.FindProperty("_FReflection", properties);
           
        _RimLightUnfill = ShaderGUI.FindProperty("_RimLightUnfill", properties);
        _RimLightColor = ShaderGUI.FindProperty("_RimLightColor", properties);
        _RimLightColorPower = ShaderGUI.FindProperty("_RimLightColorPower", properties);
        _RimLightSoftness = ShaderGUI.FindProperty("_RimLightSoftness", properties);
        _RimLightInLight = ShaderGUI.FindProperty("_RimLightInLight", properties);
        _LightAffectRimLightColor = ShaderGUI.FindProperty("_LightAffectRimLightColor", properties);

        if (shader_name == "default_ref" || shader_name == "tessellation_ref")
        {
            _Depth = ShaderGUI.FindProperty("_Depth", properties);
            _DepthEdgeHardness = ShaderGUI.FindProperty("_DepthEdgeHardness", properties);
            _DepthColor = ShaderGUI.FindProperty("_DepthColor", properties);
            _DepthColorPower = ShaderGUI.FindProperty("_DepthColorPower", properties);
        }
        else if(shader_name == "default_ft" || shader_name == "tessellation_ft" || shader_name == "default_d" || shader_name == "tessellation_d")
        {
            _Depth = null;
            _DepthEdgeHardness = null;
            _DepthColor = null;
            _DepthColorPower = null;
        }


        if (shader_name == "tessellation_d" || shader_name == "tessellation_ft" || shader_name == "tessellation_ref")
        {
            _TessellationSmoothness = ShaderGUI.FindProperty("_TessellationSmoothness", properties);
            _TessellationTransition = ShaderGUI.FindProperty("_TessellationTransition", properties);
            _TessellationNear = ShaderGUI.FindProperty("_TessellationNear", properties);
            _TessellationFar = ShaderGUI.FindProperty("_TessellationFar", properties);
        }
        else if (shader_name == "default_d" || shader_name == "default_ft" || shader_name == "default_ref")
        {

            _TessellationSmoothness = null;
            _TessellationTransition = null;
            _TessellationNear = null;
            _TessellationFar = null;

        }

        _RefVal = ShaderGUI.FindProperty("_RefVal", properties);
        _Oper = ShaderGUI.FindProperty("_Oper", properties);
        _Compa = ShaderGUI.FindProperty("_Compa", properties);

        if (shader_name == "default_d" || shader_name == "tessellation_d" || shader_name == "default_ft" || shader_name == "tessellation_ft")
        {
            if (shader_name == "default_d" || shader_name == "tessellation_d")
            {
                _N_F_CO = ShaderGUI.FindProperty("_N_F_CO", properties);
            }

            _N_F_O = ShaderGUI.FindProperty("_N_F_O", properties);
        }
        else if (shader_name == "default_ft" || shader_name == "tessellation_ft")
        {
            _N_F_CO = null;
            _N_F_O = null;
        }

        _N_F_CA = ShaderGUI.FindProperty("_N_F_CA", properties);
        _N_F_SL = ShaderGUI.FindProperty("_N_F_SL", properties);
        _N_F_GLO = ShaderGUI.FindProperty("_N_F_GLO", properties);
        _N_F_GLOT = ShaderGUI.FindProperty("_N_F_GLOT", properties);
        _N_F_SS = ShaderGUI.FindProperty("_N_F_SS", properties);
        _N_F_SON = ShaderGUI.FindProperty("_N_F_SON", properties);
        _N_F_SCT = ShaderGUI.FindProperty("_N_F_SCT", properties);
        _N_F_ST = ShaderGUI.FindProperty("_N_F_ST", properties);
        _N_F_PT = ShaderGUI.FindProperty("_N_F_PT", properties);
        _N_F_CLD = ShaderGUI.FindProperty("_N_F_CLD", properties);
        _N_F_R = ShaderGUI.FindProperty("_N_F_R", properties);
        _N_F_FR = ShaderGUI.FindProperty("_N_F_FR", properties);
        _N_F_RL = ShaderGUI.FindProperty("_N_F_RL", properties);

        if (shader_name == "default_ref" || shader_name == "tessellation_ref")
        {
            _N_F_D = ShaderGUI.FindProperty("_N_F_D", properties);
        }
        else if (shader_name == "default_ft" || shader_name == "tessellation_ft" || shader_name == "default_d" || shader_name == "tessellation_d")
        {
            _N_F_D = null;
        }


        if (shader_name == "default_d" || shader_name == "tessellation_d")
        {
            _N_F_HDLS = ShaderGUI.FindProperty("_N_F_HDLS", properties);
            _N_F_HPSS = ShaderGUI.FindProperty("_N_F_HPSS", properties);
            _ZWrite = null;
        }
        else if (shader_name == "default_ft" || shader_name == "tessellation_ft" || shader_name == "default_ref" || shader_name == "tessellation_ref")
        {
            _N_F_HDLS = null;
            _N_F_HPSS = null;
            _ZWrite = ShaderGUI.FindProperty("_ZWrite", properties);
        }


        #endregion

        //UI

        #region UI

        //Header
        Rect r_header = EditorGUILayout.BeginVertical("HelpBox");
        EditorGUILayout.LabelField("RealToon 5.2.1", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("(" + shader_type + ")", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();

        GUILayout.Space(20);


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //Double Sided

        #region Double Sided

        Rect r_doublesided = EditorGUILayout.BeginVertical("HelpBox");
        materialEditor.ShaderProperty(_DoubleSided, _DoubleSided.displayName);
        EditorGUILayout.EndVertical();

        #endregion

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(20);

        //Texture - Color - Refraction

        #region Texture - Color - Refraction

        Rect r_texturecolor = EditorGUILayout.BeginVertical("Button");

        if (shader_name == "default_ref" || shader_name == "tessellation_ref")
        {
            ShowTextureColor = EditorGUILayout.Foldout(ShowTextureColor, "(Texture - Color - Refraction)", true, EditorStyles.foldout);
        }
        else if(shader_name == "default_d" || shader_name == "tessellation_d" || shader_name == "default_ft" || shader_name == "tessellation_ft")
        {
            ShowTextureColor = EditorGUILayout.Foldout(ShowTextureColor, "(Texture - Color)", true, EditorStyles.foldout);
        }

        if (ShowTextureColor)
        {
            Rect r_inner_texturecolor = EditorGUILayout.BeginVertical("TextField");


            if (shader_name == "default_d" || shader_name == "tessellation_d" || shader_name == "default_ft" || shader_name == "tessellation_ft")
            {

                materialEditor.ShaderProperty(_MainTex, _MainTex.displayName);

                EditorGUI.BeginDisabledGroup(_MainTex.textureValue == null);
                materialEditor.ShaderProperty(_TexturePatternStyle, _TexturePatternStyle.displayName);
                materialEditor.ShaderProperty(_ReduceTextureQuality, _ReduceTextureQuality.displayName);
                EditorGUI.EndDisabledGroup();

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_MainColor, _MainColor.displayName);

                GUILayout.Space(10);

                if (shader_name == "default_d" || shader_name == "tessellation_d")
                {
                    EditorGUI.BeginDisabledGroup(_MainTex.textureValue == null);
                    materialEditor.ShaderProperty(_EnableTextureTransparent, _EnableTextureTransparent.displayName);
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    _EnableTextureTransparent = null;
                }

            }

            else if (shader_name == "default_ref" || shader_name == "tessellation_ref")
            {
                materialEditor.ShaderProperty(_RefractionIntensity, _RefractionIntensity.displayName);

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_MainColor, _MainColor.displayName);
                materialEditor.ShaderProperty(_MainColorAffectTexture, _MainColorAffectTexture.displayName);

                GUILayout.Space(10);

                EditorGUI.BeginDisabledGroup(_MainTex.textureValue == null);
                materialEditor.ShaderProperty(_TextureIntesnity, _TextureIntesnity.displayName);
                EditorGUI.EndDisabledGroup();

                materialEditor.ShaderProperty(_MainTex, _MainTex.displayName);

                EditorGUI.BeginDisabledGroup(_MainTex.textureValue == null);
                materialEditor.ShaderProperty(_ReduceTextureQuality, _ReduceTextureQuality.displayName);
                materialEditor.ShaderProperty(_TexturePatternStyle, _TexturePatternStyle.displayName);
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();

        #endregion

        //Cutout

        #region Cutout

        if (shader_name == "default_d" || shader_name == "tessellation_d")
        {
            if (_N_F_CO.floatValue == 1)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                EditorGUI.BeginDisabledGroup(_N_F_CO.floatValue == 0);

                Rect r_cutout = EditorGUILayout.BeginVertical("Button");
                ShowCutout = EditorGUILayout.Foldout(ShowCutout, "(Cutout)", true, EditorStyles.foldout);

                if (ShowCutout)
                {
                    Rect r_inner_cutout = EditorGUILayout.BeginVertical("TextField");


                    materialEditor.ShaderProperty(_Cutout, _Cutout.displayName);
                    materialEditor.ShaderProperty(_AlphaBaseCutout, _AlphaBaseCutout.displayName);

                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_UseSecondaryCutout, _UseSecondaryCutout.displayName);
                    materialEditor.ShaderProperty(_SecondaryCutout, _SecondaryCutout.displayName);

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();

                EditorGUI.EndDisabledGroup();
            }
        }

        #endregion

        //Transperancy

        #region Transperancy

        if (shader_name == "default_ft" || shader_name == "tessellation_ft")
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Rect r_transparency = EditorGUILayout.BeginVertical("Button");
            ShowTransparency = EditorGUILayout.Foldout(ShowTransparency, "(Transparency)", true, EditorStyles.foldout);

            if (ShowTransparency)
            {
                Rect r_inner_transparency = EditorGUILayout.BeginVertical("TextField");

                materialEditor.ShaderProperty(_Opacity, _Opacity.displayName);

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_MaskTransparency, _MaskTransparency.displayName);

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }

        #endregion

        //Normal Map

        #region Normal Map

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        Rect r_normalmap = EditorGUILayout.BeginVertical("Button");
        ShowNormalMap = EditorGUILayout.Foldout(ShowNormalMap, "(Normal Map)", true, EditorStyles.foldout);

        if (ShowNormalMap)
        {
            Rect r_inner_normalmap = EditorGUILayout.BeginVertical("TextField");

            materialEditor.ShaderProperty(_NormalMap, _NormalMap.displayName);

            EditorGUI.BeginDisabledGroup(_NormalMap.textureValue == null);
            materialEditor.ShaderProperty(_NormalMapIntensity, _NormalMapIntensity.displayName);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();

        #endregion

        //Color Adjustment

        #region Color Adjustment

        if (_N_F_CA.floatValue == 1)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Rect r_cadjustment = EditorGUILayout.BeginVertical("Button");
            ShowColorAdjustment = EditorGUILayout.Foldout(ShowColorAdjustment, "Color Adjustment", true, EditorStyles.foldout);

            if (ShowColorAdjustment)
            {
                Rect r_inner_cadjustment = EditorGUILayout.BeginVertical("TextField");

                materialEditor.ShaderProperty(_Saturation, _Saturation.displayName);

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

        }

        #endregion

        //Outline

        #region Outline

        if (shader_name == "default_d" || shader_name == "tessellation_d" || shader_name == "default_ft" || shader_name == "tessellation_ft")
        {
            if (_N_F_O.floatValue == 1)
            {

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_outline = EditorGUILayout.BeginVertical("Button");
                ShowOutline = EditorGUILayout.Foldout(ShowOutline, "(Outline)", true, EditorStyles.foldout);


                if (ShowOutline)
                {
                    Rect r_inner_outline = EditorGUILayout.BeginVertical("TextField");

                    materialEditor.ShaderProperty(_OutlineWidth, new GUIContent(_OutlineWidth.displayName, "Outline Width"));

                    EditorGUI.BeginDisabledGroup(_OutlineWidth.floatValue <= 0);
                    materialEditor.ShaderProperty(_OutlineWidthControl, _OutlineWidthControl.displayName);

                    if (shader_name == "default_ft" || shader_name == "tessellation_ft")
                    {
                        materialEditor.ShaderProperty(_ReduceOutlineBackFace, _ReduceOutlineBackFace.displayName);
                    }

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_OutlineExtrudeMethod, _OutlineExtrudeMethod.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_EOPO, _EOPO.displayName);

                    EditorGUI.BeginDisabledGroup(_EOPO.floatValue == 0);
                    materialEditor.ShaderProperty(_OutlineOffset, _OutlineOffset.displayName);
                    EditorGUI.EndDisabledGroup();

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_OutlineColor, _OutlineColor.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_NoisyOutlineIntensity, _NoisyOutlineIntensity.displayName);
                    materialEditor.ShaderProperty(_DynamicNoisyOutline, _DynamicNoisyOutline.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_LightAffectOutlineColor, _LightAffectOutlineColor.displayName);
                    materialEditor.ShaderProperty(_OutlineWidthAffectedByViewDistance, _OutlineWidthAffectedByViewDistance.displayName);
                    materialEditor.ShaderProperty(_VertexColorRedAffectOutlineWitdh, _VertexColorRedAffectOutlineWitdh.displayName);

                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();

            }

            int f_o_int = (int)_N_F_O.floatValue;

            switch (f_o_int)
            {
                case 0:
                    targetMat.SetShaderPassEnabled("Always", false);
                    break;
                case 1:
                    targetMat.SetShaderPassEnabled("Always", true);
                    break;
                default:
                    break;
            }
        }

        #endregion

        //Self Lit

        #region SelfLit

        if (_N_F_SL.floatValue == 1)
        {

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Rect r_selflit = EditorGUILayout.BeginVertical("Button");
            ShowSelfLit = EditorGUILayout.Foldout(ShowSelfLit, "(Self Lit)", true, EditorStyles.foldout);

            if (ShowSelfLit)
            {

                Rect r_inner_selflit = EditorGUILayout.BeginVertical("TextField");

                materialEditor.ShaderProperty(_SelfLitIntensity, _SelfLitIntensity.displayName);
                GUILayout.Space(10);
                materialEditor.ShaderProperty(_SelfLitColor, _SelfLitColor.displayName);
                materialEditor.ShaderProperty(_SelfLitPower, _SelfLitPower.displayName);
                materialEditor.ShaderProperty(_SelfLitHighContrast, _SelfLitHighContrast.displayName);

                GUILayout.Space(10);
                materialEditor.ShaderProperty(_MaskSelfLit, _MaskSelfLit.displayName);

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

        }
        #endregion

        //Gloss

        #region Gloss

        if (_N_F_GLO.floatValue == 1)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Rect r_gloss = EditorGUILayout.BeginVertical("Button");
            ShowGloss = EditorGUILayout.Foldout(ShowGloss, "(Gloss)", true, EditorStyles.foldout);

            if (ShowGloss)
            {
                Rect r_inner_gloss = EditorGUILayout.BeginVertical("TextField");

                materialEditor.ShaderProperty(_GlossIntensity, _GlossIntensity.displayName);
                EditorGUI.BeginDisabledGroup(_N_F_GLOT.floatValue == 1);
                materialEditor.ShaderProperty(_Glossiness, _Glossiness.displayName);
                materialEditor.ShaderProperty(_GlossSoftness, _GlossSoftness.displayName);
                EditorGUI.EndDisabledGroup();

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_GlossColor, _GlossColor.displayName);
                materialEditor.ShaderProperty(_GlossColorPower, _GlossColorPower.displayName);
                materialEditor.ShaderProperty(_MaskGloss, _MaskGloss.displayName);

                GUILayout.Space(10);

                //Gloss Texture

                #region Gloss Texture

                if (_N_F_GLOT.floatValue == 1)
                {

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    Rect r_glosstexture = EditorGUILayout.BeginVertical("Button");
                    GUILayout.Label("Gloss Texture", EditorStyles.boldLabel);

                    if (_N_F_GLOT.floatValue == 1)
                    {
                        Rect r_inner_glosstexture = EditorGUILayout.BeginVertical("TextField");

                        materialEditor.ShaderProperty(_GlossTexture, _GlossTexture.displayName);

                        GUILayout.Space(10);
                        EditorGUI.BeginDisabledGroup(_GlossTexture.textureValue == null);
                        materialEditor.ShaderProperty(_GlossTextureSoftness, _GlossTextureSoftness.displayName);

                        GUILayout.Space(10);

                        materialEditor.ShaderProperty(_GlossTextureRotate, _GlossTextureRotate.displayName);
                        materialEditor.ShaderProperty(_GlossTextureFollowObjectRotation, _GlossTextureFollowObjectRotation.displayName);
                        materialEditor.ShaderProperty(_GlossTextureFollowLight, _GlossTextureFollowLight.displayName);
                        EditorGUI.EndDisabledGroup();

                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndVertical();

                }
                #endregion

                EditorGUILayout.EndVertical();

            }

            EditorGUILayout.EndVertical();

        }

        #endregion

        //Shadow

        #region Shadow

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        Rect r_shadow = EditorGUILayout.BeginVertical("Button");
        ShowShadow = EditorGUILayout.Foldout(ShowShadow, "(Shadow)", true, EditorStyles.foldout);

        if (ShowShadow)
        {

            Rect r_inner_shadow = EditorGUILayout.BeginVertical("TextField");

            materialEditor.ShaderProperty(_OverallShadowColor, _OverallShadowColor.displayName);
            materialEditor.ShaderProperty(_OverallShadowColorPower, _OverallShadowColorPower.displayName);

            GUILayout.Space(10);

            materialEditor.ShaderProperty(_SelfShadowShadowTAtViewDirection, _SelfShadowShadowTAtViewDirection.displayName);

            if (shader_name == "default_d" || shader_name == "tessellation_d")
            {
                GUILayout.Space(10);

                materialEditor.ShaderProperty(_ReduceShadowPointLight, _ReduceShadowPointLight.displayName);
                materialEditor.ShaderProperty(_ReduceShadowSpotDirectionalLight, _ReduceShadowSpotDirectionalLight.displayName);

            }


            //Self Shadow

            #region Self Shadow

            if (_N_F_SS.floatValue == 1)
            {

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_selfshadow = EditorGUILayout.BeginVertical("Button");
                GUILayout.Label("Self Shadow", EditorStyles.boldLabel);

                if (_N_F_SS.floatValue == 1)
                {

                    Rect r_innerselfshadow = EditorGUILayout.BeginVertical("TextField");

                    materialEditor.ShaderProperty(_HighlightColor, _HighlightColor.displayName);
                    materialEditor.ShaderProperty(_HighlightColorPower, _HighlightColorPower.displayName);

                    GUILayout.Space(10);

                    if (shader_name == "default_d" || shader_name == "tessellation_d")
                    {
                        materialEditor.ShaderProperty(_SelfShadowRealtimeShadowIntensity, _SelfShadowRealtimeShadowIntensity.displayName);
                    }
                    else if (shader_name == "default_ft" || shader_name == "tessellation_ft" || shader_name == "default_ref" || shader_name == "tessellation_ref")
                    {
                        materialEditor.ShaderProperty(_SelfShadowIntensity, _SelfShadowIntensity.displayName);
                    }

                    materialEditor.ShaderProperty(_SelfShadowThreshold, _SelfShadowThreshold.displayName);
                    materialEditor.ShaderProperty(_VertexColorGreenControlSelfShadowThreshold, _VertexColorGreenControlSelfShadowThreshold.displayName);
                    materialEditor.ShaderProperty(_SelfShadowHardness, _SelfShadowHardness.displayName);

                    GUILayout.Space(10);

                    if (shader_name == "default_d" || shader_name == "tessellation_d")
                    {
                        materialEditor.ShaderProperty(_SelfShadowRealTimeShadowColor, _SelfShadowRealTimeShadowColor.displayName);
                        materialEditor.ShaderProperty(_SelfShadowRealTimeShadowColorPower, _SelfShadowRealTimeShadowColorPower.displayName);
                    }
                    else if (shader_name == "default_ft" || shader_name == "tessellation_ft" || shader_name == "default_ref" || shader_name == "tessellation_ref")
                    {
                        materialEditor.ShaderProperty(_SelfShadowColor, _SelfShadowColor.displayName);
                        materialEditor.ShaderProperty(_SelfShadowColorPower, _SelfShadowColorPower.displayName);
                    }

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_SelfShadowAffectedByLightShadowStrength, _SelfShadowAffectedByLightShadowStrength.displayName);

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();

            }
            #endregion

            //Smooth Object Normal

            #region Smooth Object normal

            if (_N_F_SON.floatValue == 1)
            {

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                if (_N_F_SS.floatValue == 0)
                {
                    _N_F_SON.floatValue = 0;
                    targetMat.DisableKeyword("F_SS_ON");
                    _ShowNormal.floatValue = 0;
                }

                Rect r_smoothobjectnormal = EditorGUILayout.BeginVertical("Button");
                GUILayout.Label("Smooth Object Normal [Experimental]", EditorStyles.boldLabel);

                if (_N_F_SON.floatValue == 1)
                {
                    Rect r_inner_ptexture = EditorGUILayout.BeginVertical("TextField");

                    materialEditor.ShaderProperty(_SmoothObjectNormal, _SmoothObjectNormal.displayName);
                    materialEditor.ShaderProperty(_VertexColorBlueControlSmoothObjectNormal, _VertexColorBlueControlSmoothObjectNormal.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_XYZPosition, _XYZPosition.displayName);
                    materialEditor.ShaderProperty(_XYZHardness, _XYZHardness.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_ShowNormal, _ShowNormal.displayName);

                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();

            }
            #endregion

            //Shadow Color Texture

            #region Shadow Color Texture

            if (_N_F_SCT.floatValue == 1)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_shadowcolortexture = EditorGUILayout.BeginVertical("Button");
                GUILayout.Label("Shadow Color Texture", EditorStyles.boldLabel);

                if (_N_F_SCT.floatValue == 1)
                {
                    Rect r_inner_shadowcolortexture = EditorGUILayout.BeginVertical("TextField");

                    materialEditor.ShaderProperty(_ShadowColorTexture, _ShadowColorTexture.displayName);
                    materialEditor.ShaderProperty(_ShadowColorTexturePower, _ShadowColorTexturePower.displayName);

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();

            }

            #endregion

            //ShadowT

            #region ShadowT

            if (_N_F_ST.floatValue == 1)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_shadowt = EditorGUILayout.BeginVertical("Button");
                GUILayout.Label("ShadowT", EditorStyles.boldLabel);

                if (_N_F_ST.floatValue == 1)
                {
                    Rect r_inner_shadowt = EditorGUILayout.BeginVertical("TextField");

                    materialEditor.ShaderProperty(_ShadowTIntensity, _ShadowTIntensity.displayName);
                    materialEditor.ShaderProperty(_ShadowT, _ShadowT.displayName);
                    materialEditor.ShaderProperty(_ShadowTLightThreshold, _ShadowTLightThreshold.displayName);
                    materialEditor.ShaderProperty(_ShadowTShadowThreshold, _ShadowTShadowThreshold.displayName);
                    materialEditor.ShaderProperty(_ShadowTHardness, _ShadowTHardness.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_ShadowTColor, _ShadowTColor.displayName);
                    materialEditor.ShaderProperty(_ShadowTColorPower, _ShadowTColorPower.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_N_F_STIS, _N_F_STIS.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_N_F_STIAL, _N_F_STIAL.displayName);

                    EditorGUI.BeginDisabledGroup(_N_F_STIAL.floatValue == 0);
                    materialEditor.ShaderProperty(_ShowInAmbientLightIntensity, _ShowInAmbientLightIntensity.displayName);
                    EditorGUI.EndDisabledGroup();

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_ShowInAmbientLightShadowThreshold, _ShowInAmbientLightShadowThreshold.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_LightFalloffAffectShadowT, _LightFalloffAffectShadowT.displayName);

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();

            }

            #endregion

            //Shadow PTexture

            #region PTexture

            if (_N_F_PT.floatValue == 1)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_ptexture = EditorGUILayout.BeginVertical("Button");
                GUILayout.Label("PTexture", EditorStyles.boldLabel);

                if (_N_F_PT.floatValue == 1)
                {
                    Rect r_inner_ptexture = EditorGUILayout.BeginVertical("TextField");

                    materialEditor.ShaderProperty(_PTexture, _PTexture.displayName);
                    materialEditor.ShaderProperty(_PTexturePower, _PTexturePower.displayName);

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();

            }

            #endregion

            EditorGUILayout.EndVertical();

        }

        EditorGUILayout.EndVertical();

        #endregion

        //Lighting

        #region Lighting

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        Rect r_lighting = EditorGUILayout.BeginVertical("Button");
        ShowLighting = EditorGUILayout.Foldout(ShowLighting, "(Lighting)", true, EditorStyles.foldout);

        if (ShowLighting)
        {
            Rect r_inner_ligthing = EditorGUILayout.BeginVertical("TextField");

            materialEditor.ShaderProperty(_GIFlatShade, _GIFlatShade.displayName);
            materialEditor.ShaderProperty(_GIShadeThreshold, _GIShadeThreshold.displayName);

            GUILayout.Space(10);

            materialEditor.ShaderProperty(_LightAffectShadow, _LightAffectShadow.displayName);
            EditorGUI.BeginDisabledGroup(_LightAffectShadow.floatValue == 0);
            materialEditor.ShaderProperty(_LightIntensity, _LightIntensity.displayName);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
            materialEditor.ShaderProperty(_EnvironmentalLightingIntensity, _EnvironmentalLightingIntensity.displayName);

            GUILayout.Space(10);
            materialEditor.ShaderProperty(_PointSpotlightIntensity, _PointSpotlightIntensity.displayName);
            materialEditor.ShaderProperty(_LightFalloffSoftness, _LightFalloffSoftness.displayName);

            EditorGUILayout.EndVertical();


            //Custom Light Direction

            #region Custom Light Direction

            if (_N_F_CLD.floatValue == 1)
            {

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                EditorGUI.BeginDisabledGroup(_N_F_CLD.floatValue == 0);

                Rect r_customlightdirection = EditorGUILayout.BeginVertical("Button");
                GUILayout.Label("Custom Light Direction", EditorStyles.boldLabel);

                if (_N_F_CLD.floatValue == 1)
                {
                    Rect r_inner_customlightdirection = EditorGUILayout.BeginVertical("TextField");

                    materialEditor.ShaderProperty(_CustomLightDirectionIntensity, _CustomLightDirectionIntensity.displayName);
                    materialEditor.ShaderProperty(_CustomLightDirection, _CustomLightDirection.displayName);
                    materialEditor.ShaderProperty(_CustomLightDirectionFollowObjectRotation, _CustomLightDirectionFollowObjectRotation.displayName);

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();

                EditorGUI.EndDisabledGroup();


            }

            #endregion
        }

        EditorGUILayout.EndVertical();

        #endregion

        //Reflection

        #region Reflection

        if (_N_F_R.floatValue == 1)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Rect r_reflection = EditorGUILayout.BeginVertical("Button");
            ShowReflection = EditorGUILayout.Foldout(ShowReflection, "(Reflection)", true, EditorStyles.foldout);

            if (ShowReflection)
            {
                Rect r_inner_reflection = EditorGUILayout.BeginVertical("TextField");

                materialEditor.ShaderProperty(_ReflectionIntensity, _ReflectionIntensity.displayName);
                materialEditor.ShaderProperty(_ReflectionRoughtness, _ReflectionRoughtness.displayName);

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_MaskReflection, _MaskReflection.displayName);

                GUILayout.Space(10);

                //FReflection

                #region FReflection

                if (_N_F_FR.floatValue == 1)
                {

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUI.BeginDisabledGroup(_N_F_FR.floatValue == 0);

                    Rect r_freflection = EditorGUILayout.BeginVertical("Button");
                    GUILayout.Label("FReflection", EditorStyles.boldLabel);

                    if (_N_F_FR.floatValue == 1)
                    {
                        Rect r_inner_freflection = EditorGUILayout.BeginVertical("TextField");

                        materialEditor.ShaderProperty(_UseFReflection, _UseFReflection.displayName);
                        materialEditor.ShaderProperty(_FReflection, _FReflection.displayName);

                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        _UseFReflection.floatValue = 0;
                    }

                    EditorGUILayout.EndVertical();

                    EditorGUI.EndDisabledGroup();
                }

                EditorGUILayout.EndVertical();

            }

            #endregion

            EditorGUILayout.EndVertical();
        }

       #endregion

        // Rim Light

        #region Rim Light

        if (_N_F_RL.floatValue == 1)
        {

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Rect r_rimlight = EditorGUILayout.BeginVertical("Button");
            ShowRimLight = EditorGUILayout.Foldout(ShowRimLight, "(Rim Light)", true, EditorStyles.foldout);

            if (ShowRimLight)
            {
                Rect r_inner_rimlight = EditorGUILayout.BeginVertical("TextField");

                materialEditor.ShaderProperty(_RimLightUnfill, _RimLightUnfill.displayName);
                materialEditor.ShaderProperty(_RimLightSoftness, _RimLightSoftness.displayName);

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_LightAffectRimLightColor, _LightAffectRimLightColor.displayName);

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_RimLightColor, _RimLightColor.displayName);
                materialEditor.ShaderProperty(_RimLightColorPower, _RimLightColorPower.displayName);

                GUILayout.Space(10);
                materialEditor.ShaderProperty(_RimLightInLight, _RimLightInLight.displayName);

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

        }

        #endregion

        // Depth

        #region Depth

        if (_N_F_D != null)
        {

            if (_N_F_D.floatValue == 1)
            {

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_depth = EditorGUILayout.BeginVertical("Button");
                ShowDepth = EditorGUILayout.Foldout(ShowDepth, "(Depth)", true, EditorStyles.foldout);

                if (ShowDepth)
                {
                    Rect r_inner_depth = EditorGUILayout.BeginVertical("TextField");

                    materialEditor.ShaderProperty(_Depth, _Depth.displayName);
                    materialEditor.ShaderProperty(_DepthEdgeHardness, _DepthEdgeHardness.displayName);
                    materialEditor.ShaderProperty(_DepthColor, _DepthColor.displayName);
                    materialEditor.ShaderProperty(_DepthColorPower, _DepthColorPower.displayName);

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();

            }

        }
        #endregion

        //Tessellation

        #region Tessellation

        if (shader_name == "tessellation_d" || shader_name == "tessellation_ft" || shader_name == "tessellation_ref")
        {

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Rect r_tessellation = EditorGUILayout.BeginVertical("Button");
            ShowTessellation = EditorGUILayout.Foldout(ShowTessellation, "(Tessellation)", true, EditorStyles.foldout);

            if (ShowTessellation)
            {
                Rect r_inner_tessellation = EditorGUILayout.BeginVertical("TextField");

                materialEditor.ShaderProperty(_TessellationSmoothness, _TessellationSmoothness.displayName);
                materialEditor.ShaderProperty(_TessellationTransition, _TessellationTransition.displayName);
                materialEditor.ShaderProperty(_TessellationNear, _TessellationNear.displayName);
                materialEditor.ShaderProperty(_TessellationFar, _TessellationFar.displayName);

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }

        #endregion

        //See Through

        #region See Through

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        Rect r_seethrough = EditorGUILayout.BeginVertical("Button");
        ShowSeeThrough = EditorGUILayout.Foldout(ShowSeeThrough, "(See Through)", true, EditorStyles.foldout);

        if (ShowSeeThrough)
        {
            Rect r_inner_seethrough = EditorGUILayout.BeginVertical("TextField");

            materialEditor.ShaderProperty(_RefVal, _RefVal.displayName);
            materialEditor.ShaderProperty(_Oper, _Oper.displayName);
            materialEditor.ShaderProperty(_Compa, _Compa.displayName);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();

        GUILayout.Space(20);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        #endregion

        //Disable/Enable Features

        #region Disable/Enable Features

        Rect r_disableenablefeature = EditorGUILayout.BeginVertical("Button");
        ShowDisableEnable = EditorGUILayout.Foldout(ShowDisableEnable, "(Disable/Enable Features)", true, EditorStyles.foldout);

        if (ShowDisableEnable)
        {
            if (shader_name == "default_d" || shader_name == "tessellation_d" || shader_name == "default_ft" || shader_name == "tessellation_ft")
            {

                Rect r_ou = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_N_F_O, _N_F_O.displayName);
                EditorGUILayout.EndVertical();

                if (shader_name == "default_d" || shader_name == "tessellation_d")
                {
                    Rect r_co = EditorGUILayout.BeginVertical("HelpBox");
                    materialEditor.ShaderProperty(_N_F_CO, _N_F_CO.displayName);
                    EditorGUILayout.EndVertical();
                }
            }

            Rect r_ca = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_CA, _N_F_CA.displayName);
            EditorGUILayout.EndVertical();

            Rect r_sl = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_SL, _N_F_SL.displayName);
            EditorGUILayout.EndVertical();

            Rect r_o = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_GLO, _N_F_GLO.displayName);
            EditorGUILayout.EndVertical();

            Rect r_glot = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_GLOT, _N_F_GLOT.displayName);
            EditorGUILayout.EndVertical();

            Rect r_ss = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_SS, _N_F_SS.displayName);
            EditorGUILayout.EndVertical();

            Rect r_son = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_SON, _N_F_SON.displayName);
            EditorGUILayout.EndVertical();

            if (_N_F_SS.floatValue == 0)
            {
                _N_F_SON.floatValue = 0;
            }

            Rect r_sct = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_SCT, _N_F_SCT.displayName);
            EditorGUILayout.EndVertical();

            Rect r_st = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_ST, _N_F_ST.displayName);
            EditorGUILayout.EndVertical();

            Rect r_pt = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_PT, _N_F_PT.displayName);
            EditorGUILayout.EndVertical();

            Rect r_cld = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_CLD, _N_F_CLD.displayName);
            EditorGUILayout.EndVertical();

            Rect r_r = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_R, _N_F_R.displayName);
            EditorGUILayout.EndVertical();

            Rect r_fr = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_FR, _N_F_FR.displayName);
            EditorGUILayout.EndVertical();

            Rect r_rl = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_N_F_RL, _N_F_RL.displayName);
            EditorGUILayout.EndVertical();

            if (shader_name == "default_ref" || shader_name == "tessellation_ref")
            {
                Rect r_d = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_N_F_D, _N_F_D.displayName);
                EditorGUILayout.EndVertical();
            }

        }

        EditorGUILayout.EndVertical();

        #endregion

        GUILayout.Space(10);

        if (shader_name == "default_d" || shader_name ==  "tessellation_d")
        {
            materialEditor.ShaderProperty(_N_F_HDLS, _N_F_HDLS.displayName);
            materialEditor.ShaderProperty(_N_F_HPSS, _N_F_HPSS.displayName);
        }
        else if (shader_name == "default_ft" || shader_name == "tessellation_ft" || shader_name == "default_ref" || shader_name == "tessellation_ref")
        {
            materialEditor.ShaderProperty(_ZWrite, _ZWrite.displayName);
        }

        GUILayout.Space(10);

        materialEditor.RenderQueueField();
        materialEditor.EnableInstancingField();

        #endregion

    }
}