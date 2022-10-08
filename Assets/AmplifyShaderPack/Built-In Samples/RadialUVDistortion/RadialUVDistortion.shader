// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyShaderPack/Built-in/RadialUVDistortion"
{
	Properties
	{
		_NoiseMap("Noise Map", 2D) = "white" {}
		_NoiseMapStrength("NoiseMapStrength", Range( 0 , 1)) = 0
		_RingPannerSpeed("RingPannerSpeed", Vector) = (0,0,0,0)
		_NoiseMapSize("NoiseMapSize", Vector) = (512,512,0,0)
		_NoiseMapPannerSpeed("NoiseMapPannerSpeed", Vector) = (0,0,0,0)
		_BaseTexture("Base Texture", 2D) = "white" {}
		_Tint("Tint", Color) = (0,0,0,0)
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Tint;
		uniform sampler2D _BaseTexture;
		uniform sampler2D _NoiseMap;
		uniform float2 _NoiseMapSize;
		uniform float2 _NoiseMapPannerSpeed;
		uniform float _NoiseMapStrength;
		uniform float2 _RingPannerSpeed;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_output_1_0_g33 = _NoiseMapSize;
			float2 appendResult10_g33 = (float2(( (temp_output_1_0_g33).x * i.uv_texcoord.x ) , ( i.uv_texcoord.y * (temp_output_1_0_g33).y )));
			float2 temp_output_11_0_g33 = _NoiseMapPannerSpeed;
			float2 panner18_g33 = ( ( (temp_output_11_0_g33).x * _Time.y ) * float2( 1,0 ) + i.uv_texcoord);
			float2 panner19_g33 = ( ( _Time.y * (temp_output_11_0_g33).y ) * float2( 0,1 ) + i.uv_texcoord);
			float2 appendResult24_g33 = (float2((panner18_g33).x , (panner19_g33).y));
			float2 temp_output_47_0_g33 = _RingPannerSpeed;
			float2 uv_TexCoord78_g33 = i.uv_texcoord * float2( 2,2 );
			float2 temp_output_31_0_g33 = ( uv_TexCoord78_g33 - float2( 1,1 ) );
			float2 appendResult39_g33 = (float2(frac( ( atan2( (temp_output_31_0_g33).x , (temp_output_31_0_g33).y ) / 6.28318548202515 ) ) , length( temp_output_31_0_g33 )));
			float2 panner54_g33 = ( ( (temp_output_47_0_g33).x * _Time.y ) * float2( 1,0 ) + appendResult39_g33);
			float2 panner55_g33 = ( ( _Time.y * (temp_output_47_0_g33).y ) * float2( 0,1 ) + appendResult39_g33);
			float2 appendResult58_g33 = (float2((panner54_g33).x , (panner55_g33).y));
			o.Emission = ( _Tint * tex2D( _BaseTexture, ( ( (tex2D( _NoiseMap, ( appendResult10_g33 + appendResult24_g33 ) )).rg * _NoiseMapStrength ) + ( float2( 1,1 ) * appendResult58_g33 ) ) ) ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"

}
/*ASEBEGIN
Version=18914
231;73;1024;745;-922.009;361.3452;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;234;-388.6922,186.1725;Float;False;Property;_NoiseMapStrength;NoiseMapStrength;1;0;Create;True;0;0;0;False;0;False;0;0.6162086;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;241;-399.0201,64.6485;Float;False;Property;_NoiseMapPannerSpeed;NoiseMapPannerSpeed;4;0;Create;True;0;0;0;False;0;False;0,0;1.61,-0.43;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;240;-375.4798,451.084;Float;False;Property;_RingPannerSpeed;RingPannerSpeed;2;0;Create;True;0;0;0;False;0;False;0,0;0.1,-1.27;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;279;-346.5933,297.4621;Float;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;233;-368.8911,-258.1053;Float;True;Property;_NoiseMap;Noise Map;0;0;Create;True;0;0;0;False;0;False;None;937c2d88ef9b4e889d5a0a6a952c1a4b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.Vector2Node;239;-357.6543,-64.65796;Float;False;Property;_NoiseMapSize;NoiseMapSize;3;0;Create;True;0;0;0;False;0;False;512,512;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;316;97.76781,66.88756;Inherit;False;RadialUVDistortion;-1;;33;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;;False;1;FLOAT2;0,0;False;11;FLOAT2;0,0;False;65;FLOAT;0;False;68;FLOAT2;0,0;False;47;FLOAT2;0,0;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;276;591.1954,69.80499;Inherit;True;Property;_BaseTexture;Base Texture;5;0;Create;True;0;0;0;False;0;False;-1;None;468c5859e2a9496ea1afdbc5cfce02ee;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;317;564.5071,-182.9351;Float;False;Property;_Tint;Tint;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0.682353,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;318;1063.514,-27.07267;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;319;1362.009,36.15475;Inherit;False;Property;_Smoothness;Smoothness;7;0;Create;True;0;0;0;False;0;False;0;0.75;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;230;1656.835,-174.857;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;AmplifyShaderPack/Built-in/RadialUVDistortion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;32;10;25;False;0.8;True;2;5;False;-1;10;False;-1;0;1;False;-1;1;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;316;60;233;0
WireConnection;316;1;239;0
WireConnection;316;11;241;0
WireConnection;316;65;234;0
WireConnection;316;68;279;0
WireConnection;316;47;240;0
WireConnection;276;1;316;0
WireConnection;318;0;317;0
WireConnection;318;1;276;0
WireConnection;230;2;318;0
WireConnection;230;4;319;0
ASEEND*/
//CHKSM=7B50390ED5B2A0CDB1191E88AF52CF8B864D2BA3