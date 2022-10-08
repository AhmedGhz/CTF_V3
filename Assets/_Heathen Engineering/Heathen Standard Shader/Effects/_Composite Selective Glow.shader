// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Composite Selective Glow " {
Properties {
	_MainTex ("", RECT) = "white" {}
	_BlurTex ("", RECT) = "white" {}
	_BlurRamp ("", 2D) = "gray" {}
	_Outter ("Intensity", Range(0.1,10)) = 6.0 
	
	[HideInInspector] _Mode ("__mode", Float) = 0.0
}

SubShader { 
		
	Pass {
		ZTest Always Cull Off ZWrite Off Fog { Mode off }

CGPROGRAM
	//#pragma exclude_renderers gles
	#pragma vertex vert
	#pragma fragment frag nofog
	#pragma fragmentoption ARB_precision_hint_fastest 
	#include "UnityCG.cginc"

	uniform sampler2D _MainTex : register(s0);
	uniform sampler2D _BlurTex : register(s1);
	uniform sampler2D _BlurRamp : register(s2);
	float _Outter;
	float _Mode;

	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};

	uniform float4 _MainTex_TexelSize;
	v2f vert (appdata_img v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos (v.vertex);
		o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
		
		return o;
	}

	half4 frag (v2f i) : COLOR
	{		
			
		fixed4 original = tex2D(_MainTex, i.uv);
		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
		        i.uv.y = 1-i.uv.y;
		#endif
		fixed4 unbluredAlpha = tex2D(_BlurRamp, i.uv);	
		fixed4 combineAlpha = tex2D(_BlurTex, i.uv);
		combineAlpha *= _Outter;
		half4 color = original + (unbluredAlpha + combineAlpha);
		if(_Mode < 1)
		{
			color = original + (unbluredAlpha + combineAlpha); 
		}
		else if(_Mode < 2)
		{
			color = combineAlpha;
		}
		else
		{
			color = unbluredAlpha;
		}
		
		return color;
	}
ENDCG
	}		
}

Fallback off

}