Shader "SupGames/Mobile/Blur"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "" {}
		_MaskTex("Base (RGB)", 2D) = "white" {}
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata 
	{
		fixed4 pos : POSITION;
		fixed2 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f
	{
		fixed4 pos : SV_POSITION;
		fixed2  uv : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	struct v2fb
	{
		fixed4 pos : SV_POSITION;
		fixed4  uv : TEXCOORD0;
#if defined(KERNEL)
		fixed4  uv1 : TEXCOORD1;
#endif
		UNITY_VERTEX_OUTPUT_STEREO
	};

	UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
	UNITY_DECLARE_SCREENSPACE_TEXTURE(_MaskTex);
	UNITY_DECLARE_SCREENSPACE_TEXTURE(_BlurTex);
	fixed4 _MainTex_TexelSize;
	fixed _BlurAmount;

	v2f vert(appdata v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_OUTPUT(v2f, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.pos = UnityObjectToClipPos(v.pos);
		o.uv = v.uv;
		return o;
	}

	v2fb vertb(appdata v)
	{
		v2fb o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_OUTPUT(v2fb, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.pos = UnityObjectToClipPos(v.pos);
		fixed2 offset = _MainTex_TexelSize.xy * _BlurAmount;
		o.uv = fixed4(v.uv - offset, v.uv + offset);
#if defined(KERNEL)
		offset *= 2.0h;
		o.uv1 = fixed4(v.uv - offset, v.uv + offset);
#endif
		return o;
	}

	fixed4 fragb(v2fb i) : COLOR
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		fixed4 result = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv.xy);
		result += UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv.xw);
		result += UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv.zy);
		result += UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv.zw);
#if defined(KERNEL)
		result += UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv1.xy);
		result += UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv1.xw);
		result += UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv1.zy);
		result += UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv1.zw);
		return result * 0.125h;;
#endif
		return result * 0.25h;
	}

	fixed4 frag(v2f i) : COLOR
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		fixed4 c = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, UnityStereoTransformScreenSpaceTex(i.uv));
		fixed4 b = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_BlurTex, UnityStereoTransformScreenSpaceTex(i.uv));
		fixed4 m = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MaskTex, i.uv);
		return lerp(c, b, m.r);
	}

	ENDCG

	Subshader
	{
		Pass //0
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			CGPROGRAM
			#pragma shader_feature KERNEL
			#pragma vertex vertb
			#pragma fragment fragb
			#pragma fragmentoption ARB_precision_hint_fastest
			ENDCG
		}
		Pass //1
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			ENDCG
		}
	}
	Fallback off
}