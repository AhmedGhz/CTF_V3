Shader "Hidden/Render Selective Glow" {
Properties {
	_GlowMap ("Glow (A)", 2D) = "bump" {}
	_MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
	_Inner ("Inner Intensity", Range(0.1,10)) = 2.0
	_Outter ("Outter Intensity", Range(0.1,10)) = 2.0
	_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
	[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
	_MetallicGlossMap("Metallic", 2D) = "white" {}
	_EmissionColor("Color", Color) = (0,0,0)
	_EmissionMap("Emission", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

	// Blending state
	[HideInInspector] _Mode("__mode", Float) = 0.0
	[HideInInspector] _SrcBlend("__src", Float) = 1.0
	[HideInInspector] _DstBlend("__dst", Float) = 0.0
	[HideInInspector] _ZWrite("__zw", Float) = 1.0
	[HideInInspector] _Cull("__cu", Float) = 0.0
}
//==========================================================================================================
//			Legacy Shader Support
//==========================================================================================================
	//Standard selective glow replacer shader; on camera render we blackout all non-SelectiveGlow shaders then draw only the _GlowTex along with the _GlowColor
	SubShader 
	{
		Tags { "RenderType"="SelectiveGlow" "Queue"="Transparent"}
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf BlinnPhong alpha nofog

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _GlowMap;
		half4 _GlowColor;
		float _Inner, _Outter;

		struct Input {
			float2 uv_GlowMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 glowAlpha = tex2D (_GlowMap, IN.uv_GlowMap) * _GlowColor;
			o.Albedo = (((_Outter * _GlowColor) * (_Inner * glowAlpha)) * (glowAlpha.a * _GlowColor.a)).rgb;
			o.Alpha = 1;//glowAlpha.a;
			o.Emission = o.Albedo;
			//return ((_Outter * _GlowColor) * (_Inner * glowAlpha)) * (glowAlpha.a * _GlowColor.a);
		}
		ENDCG
	}
	//Transparent glow same as above but does not "blackout" non-glow rather we blend ontop of what we have
	//This works well for true transparent shaders however cutout (Alpha Test) shaders will allow the glow through the opaque areas of the main tex
	SubShader {
		ZTest Always 
		ZWrite Off
		//Cull Off
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="TransparentGlow"}	
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alpha:fade nofog

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _GlowMap;
		sampler2D _MainTex;
		half4 _GlowColor;
		float _Inner, _Outter;

		struct Input {
		    float2 uv_MainTex;
			float2 uv_GlowMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			half4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 glowAlpha = tex2D (_GlowMap, IN.uv_GlowMap) * _GlowColor;
			if(glowAlpha.a > 0)
				o.Albedo = (((_Outter * _GlowColor) * (_Inner * glowAlpha)) * (glowAlpha.a * _GlowColor.a)).rgb;
			else
			    o.Albedo = fixed3(0,0,0);
			    
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			
			if(tex.a > 0)
				o.Alpha = tex.a;
			else
				o.Alpha = glowAlpha.a;
			
			o.Emission = o.Albedo;
			//return ((_Outter * _GlowColor) * (_Inner * glowAlpha)) * (glowAlpha.a * _GlowColor.a);
		}
		ENDCG
	}
	//Experamental TriplanarGlow this is a WIP as of this version and will offset glow texture oddly 
	//from some angles works well enough though terrain and flat walls
	SubShader {
	
		//Write black over our whole area we will comback with the glow
		Pass {
			Fog { Mode Off }
			Color (0,0,0,0)
		}
		
		Tags { "RenderType"="TriplanarGlow" }
		LOD 300
		Colormask RGBA
		CGPROGRAM
		#pragma surface surf Lambert alpha vertex:vert nofog
		#pragma exclude_renderers flash
		half4 _XGlowMap_ST;
		half4 _YGlowMap_ST;
		half4 _ZGlowMap_ST;
		sampler2D _XGlowMap, _YGlowMap, _ZGlowMap;
		half4 _XGlowColor,_YGlowColor,_ZGlowColor;
		float _Inner, _Outter;

		struct Input { 
			half4 xyv;
			half4 zvb;
			//half4 xyb;
			half3 norm;
		};
		
		void vert (inout appdata_full v, out Input o) {
			o.xyv.xy = TRANSFORM_TEX(v.vertex.zy, _XGlowMap);
			o.xyv.zw = TRANSFORM_TEX(v.vertex.zx, _YGlowMap);
			o.zvb.xy = TRANSFORM_TEX(v.vertex.xy, _ZGlowMap);
			//Fully initalize ... we keep this unused value for compatability with the other Triplanar shaders
			o.zvb.zw = float2(0,0);
			
			o.norm = (abs(v.normal));
			o.norm = (o.norm - 0.2) * 7;  
			o.norm = max(o.norm, 0);
			o.norm /= (o.norm.x + o.norm.y + o.norm.z ).xxx;   
		}
		
		void surf (Input IN, inout SurfaceOutput o) {

			half3 norm = IN.norm;
			//This needs to be ramped for inner and outter settings at current its a flat glow
			//((_Inner * glowAlpha) * (_Outter * _GlowColor))
			half4 output = 
			_XGlowColor*tex2D(_XGlowMap, IN.xyv.xy)*norm.xxxx+
			_YGlowColor*tex2D(_YGlowMap, IN.xyv.zw)*norm.yyyy+
			_ZGlowColor*tex2D(_ZGlowMap, IN.zvb.xy)*norm.zzzz;			
			
			o.Albedo = output.rgb;
			o.Alpha = output.a*4;
		}
		ENDCG
	}
//==============================================================================================
//				End Legacy Start Heathen Standard
//==============================================================================================
	
	SubShader 
	{
		Tags { "RenderType"="StandardGlow" "IgnoreProjector"="True" "Queue"="Transparent"}
		//ZTest Off
		//ZWrite Off
		//Offset -1, -1
		//Blend[_SrcBlend][_DstBlend]
		//ZWrite[_ZWrite]
		//Cull[_Cull]
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf BlinnPhong alphatest:_Cutoff nofog
		#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _EmissionMap;
		sampler2D _MainTex;
		half4 _EmissionColor;
		float _Mode;

		struct Input {
			float2 uv_MainTex;
			float2 uv_EmissionMap;
		};

		void surf(Input IN, inout SurfaceOutput o) 
		{
			half4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 glowAlpha = tex2D(_EmissionMap, IN.uv_MainTex) * _EmissionColor;
			float aproxValue = (glowAlpha.r + glowAlpha.r + glowAlpha.b + glowAlpha.g + glowAlpha.g + glowAlpha.g) / 6;
			if (aproxValue > 0.0)
				o.Albedo = ((_EmissionColor * glowAlpha) * aproxValue).rgb * glowAlpha.a;
			else
				o.Albedo = fixed3(0, 0, 0);

			if (_Mode < 1)
			{
				o.Alpha = 1;
			}
			else
			{
				o.Alpha = aproxValue + tex.a;
			}
			
			o.Emission = o.Albedo;
		}
		ENDCG
	}
	
	//If its not glow black it out
	SubShader {
		Fog { Mode off }
		Tags {
			"RenderType" = "Opaque"
		}
		CGPROGRAM
		#pragma surface surf Lambert nofog
		struct Input {
			float2 uv_Control : TEXCOORD0;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			
			o.Albedo = fixed3(0,0,0);
			o.Alpha = 0.0;
		}
		ENDCG  
	}  
}
