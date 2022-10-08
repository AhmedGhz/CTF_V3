Shader "ErbGameArt/Particles/Add_Trail"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_SpeedMainTexUVNoiseZW("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
		_StartColor("StartColor", Color) = (1,0,0,1)
		_EndColor("EndColor", Color) = (1,1,0,1)
		_Colorpower("Color power", Float) = 1
		_Colorrange("Color range", Float) = 1
		_Noise("Noise", 2D) = "white" {}
		[Toggle(_USEDEPTH_ON)] _Usedepth("Use depth?", Float) = 0
		_Depthpower("Depth power", Float) = 1
		_Emission("Emission", Float) = 2
		[Toggle(_USEDARK_ON)] _Usedark("Use dark", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true" "PreviewType"="Plane" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		//#pragma target 3.0
		#pragma shader_feature _USEDARK_ON
		#pragma shader_feature _USEDEPTH_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		uniform float4 _StartColor;
		uniform float4 _EndColor;
		uniform float _Colorrange;
		uniform float _Colorpower;
		uniform float _Emission;
		uniform sampler2D _MainTexture;
		uniform float4 _SpeedMainTexUVNoiseZW;
		uniform float4 _MainTexture_ST;
		uniform sampler2D _Noise;
		uniform float4 _Noise_ST;
		uniform sampler2D _CameraDepthTexture;
		uniform float _Depthpower;

		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};
		
		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float U6 = i.uv_texcoord.x;
			float clampResult66 = clamp( pow( ( U6 * _Colorrange ) , _Colorpower ) , 0.0 , 1.0 );
			float4 lerpResult3 = lerp( _StartColor , _EndColor , clampResult66);
			float4 temp_cast_0 = (1.0).xxxx;
			float2 appendResult32 = (float2(_SpeedMainTexUVNoiseZW.x , _SpeedMainTexUVNoiseZW.y));
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float4 Main57 = tex2D( _MainTexture, ( ( appendResult32 * _Time.y ) + uv_MainTexture ) );
			float2 uv_Noise = i.uv_texcoord * _Noise_ST.xy + _Noise_ST.zw;
			float2 appendResult29 = (float2(_SpeedMainTexUVNoiseZW.z , _SpeedMainTexUVNoiseZW.w));
			float clampResult44 = clamp( ( pow( ( 1.0 - U6 ) , 0.8 ) * 1.0 ) , 0.2 , 0.6 );
			float4 temp_cast_1 = (U6).xxxx;
			float4 clampResult48 = clamp( ( ( tex2D( _Noise, ( uv_Noise + ( _Time.y * appendResult29 ) ) ) + clampResult44 ) - temp_cast_1 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 Dissolve49 = clampResult48;
			float V17 = i.uv_texcoord.y;
			float clampResult24 = clamp( ( (1.0 + (U6 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) * (1.0 + (V17 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) * V17 * 6.0 ) , 0.0 , 1.0 );
			float4 temp_output_51_0 = ( i.vertexColor.a * Main57 * Dissolve49 * clampResult24 );
			#ifdef _USEDARK_ON
				float4 staticSwitch55 = temp_output_51_0;
			#else
				float4 staticSwitch55 = temp_cast_0;
			#endif
			o.Emission = ( ( lerpResult3 * i.vertexColor * _Emission ) * staticSwitch55 ).rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth62 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth62 = abs( ( screenDepth62 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depthpower ) );
			float clampResult63 = clamp( distanceDepth62 , 0.0 , 1.0 );
			#ifdef _USEDEPTH_ON
				float4 staticSwitch65 = ( temp_output_51_0 * clampResult63 );
			#else
				float4 staticSwitch65 = temp_output_51_0;
			#endif
			o.Alpha = staticSwitch65.r;
		}
		ENDCG
	}
}