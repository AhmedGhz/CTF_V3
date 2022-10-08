// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyShaderPack/Built-in/Screen"
{
	Properties
	{
		_Overlay("Overlay", 2D) = "white" {}
		_Base("Base", 2D) = "white" {}
		_NoiseMap("Noise Map", 2D) = "white" {}
		_RGB("RGB", 2D) = "white" {}
		_NoiseMapStrength("NoiseMapStrength", Range( 0 , 1)) = 0
		_Intensity("Intensity", Float) = 20
		_RingPannerSpeed("RingPannerSpeed", Vector) = (0,0,0,0)
		_NoiseMapSize("NoiseMapSize", Vector) = (512,512,0,0)
		_NoiseMapPannerSpeed("NoiseMapPannerSpeed", Vector) = (0,0,0,0)
		[HDR]_Tint("Tint", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Overlay;
		uniform sampler2D _NoiseMap;
		uniform float2 _NoiseMapSize;
		uniform float2 _NoiseMapPannerSpeed;
		uniform float _NoiseMapStrength;
		uniform float2 _RingPannerSpeed;
		uniform sampler2D _Base;
		uniform float4 _Base_ST;
		uniform sampler2D _RGB;
		uniform float _Intensity;
		uniform float4 _Tint;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 temp_output_1_0_g48 = _NoiseMapSize;
			float2 appendResult10_g48 = (float2(( (temp_output_1_0_g48).x * i.uv_texcoord.x ) , ( i.uv_texcoord.y * (temp_output_1_0_g48).y )));
			float2 temp_output_11_0_g48 = _NoiseMapPannerSpeed;
			float2 panner18_g48 = ( ( (temp_output_11_0_g48).x * _Time.y ) * float2( 1,0 ) + i.uv_texcoord);
			float2 panner19_g48 = ( ( _Time.y * (temp_output_11_0_g48).y ) * float2( 0,1 ) + i.uv_texcoord);
			float2 appendResult24_g48 = (float2((panner18_g48).x , (panner19_g48).y));
			float2 temp_output_47_0_g48 = _RingPannerSpeed;
			float2 uv_TexCoord78_g48 = i.uv_texcoord * float2( 2,2 );
			float2 temp_output_31_0_g48 = ( uv_TexCoord78_g48 - float2( 1,1 ) );
			float2 appendResult39_g48 = (float2(frac( ( atan2( (temp_output_31_0_g48).x , (temp_output_31_0_g48).y ) / 6.28318548202515 ) ) , length( temp_output_31_0_g48 )));
			float2 panner54_g48 = ( ( (temp_output_47_0_g48).x * _Time.y ) * float2( 1,0 ) + appendResult39_g48);
			float2 panner55_g48 = ( ( _Time.y * (temp_output_47_0_g48).y ) * float2( 0,1 ) + appendResult39_g48);
			float2 appendResult58_g48 = (float2((panner54_g48).x , (panner55_g48).y));
			float2 uv_Base = i.uv_texcoord * _Base_ST.xy + _Base_ST.zw;
			float clampResult147 = clamp( sin( _Time.y ) , 0.2 , 0.8 );
			float4 lerpResult142 = lerp( tex2D( _Overlay, ( ( (tex2D( _NoiseMap, ( appendResult10_g48 + appendResult24_g48 ) )).rg * _NoiseMapStrength ) + ( float2( 1,1 ) * appendResult58_g48 ) ) ) , tex2D( _Base, uv_Base ) , clampResult147);
			float4 clampResult53 = clamp( lerpResult142 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 break44 = clampResult53;
			float4 tex2DNode37 = tex2D( _RGB, ( i.uv_texcoord * float2( 350,350 ) ) );
			float4 appendResult47 = (float4(( break44.r * tex2DNode37.r ) , ( break44.g * tex2DNode37.g ) , ( break44.b * tex2DNode37.b ) , break44.a));
			float mulTime155 = _Time.y * 3.0;
			float4 clampResult55 = clamp( ( appendResult47 * ( _Intensity + sin( mulTime155 ) ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			o.Emission = ( clampResult55 * _Tint ).xyz;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18901
1237;73;682;926;-2261.666;751.123;1.696781;True;False
Node;AmplifyShaderEditor.Vector2Node;140;-264.2746,-283.0395;Float;False;Property;_NoiseMapSize;NoiseMapSize;7;0;Create;True;0;0;0;False;0;False;512,512;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;137;-282.1,232.7026;Float;False;Property;_RingPannerSpeed;RingPannerSpeed;6;0;Create;True;0;0;0;False;0;False;0,0;0.02,-0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;136;-305.6404,-153.733;Float;False;Property;_NoiseMapPannerSpeed;NoiseMapPannerSpeed;8;0;Create;True;0;0;0;False;0;False;0,0;0.2,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;135;-295.3124,-32.20903;Float;False;Property;_NoiseMapStrength;NoiseMapStrength;4;0;Create;True;0;0;0;False;0;False;0;0.117;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;145;415.4646,475.4937;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;138;-253.2135,79.08066;Float;False;Constant;_Vector2;Vector 2;7;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;139;-281.2529,-479.9698;Float;True;Property;_NoiseMap;Noise Map;2;0;Create;True;0;0;0;False;0;False;None;f801b5d60f6341cd885f97f23afdf8d8;True;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SinOpNode;144;645.7819,395.7685;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;141;107.039,-237.392;Inherit;False;RadialUVDistortion;-1;;48;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;;False;1;FLOAT2;0,0;False;11;FLOAT2;0,0;False;65;FLOAT;0;False;68;FLOAT2;0,0;False;47;FLOAT2;0,0;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;147;944.7515,-59.32968;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;143;488.4344,91.125;Inherit;True;Property;_Base;Base;1;0;Create;True;0;0;0;False;0;False;-1;None;9580df54da1e4d87b0f89f2e36519caf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;34;624.9684,-345.9105;Inherit;True;Property;_Overlay;Overlay;0;0;Create;True;0;0;0;False;0;False;-1;None;109f6beb94e9454981d4600a7be9d87b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;990.321,73.1501;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;39;1021.25,212.8401;Inherit;False;Constant;_Vector1;Vector 1;1;0;Create;True;0;0;0;False;0;False;350,350;350,350;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.LerpOp;142;1007.867,-206.5999;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;1250.24,144.9401;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;53;1230.81,-139.8547;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;155;1753.708,624.3957;Inherit;False;1;0;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;44;1521.49,-136.0499;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;37;1378.1,115.54;Inherit;True;Property;_RGB;RGB;3;0;Create;True;0;0;0;False;0;False;-1;None;ae2fc962c2d555344ae50444519518a3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;1861.49,56.95007;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;1860.49,-54.04993;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;1861.49,171.9501;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;156;1904.611,471.6944;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;1910.72,288.9221;Inherit;False;Property;_Intensity;Intensity;5;0;Create;True;0;0;0;False;0;False;20;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;47;2039.49,-13.04993;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;158;2100.673,263.6529;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;2284.04,23.02007;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;162;2535.082,-377.1044;Inherit;False;Property;_Tint;Tint;9;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;2.996078,2.996078,2.996078,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;55;2481.6,24.43979;Inherit;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;2886.583,-248.1045;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;174;3135.215,-170.4068;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;AmplifyShaderPack/Built-in/Screen;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;144;0;145;0
WireConnection;141;60;139;0
WireConnection;141;1;140;0
WireConnection;141;11;136;0
WireConnection;141;65;135;0
WireConnection;141;68;138;0
WireConnection;141;47;137;0
WireConnection;147;0;144;0
WireConnection;34;1;141;0
WireConnection;142;0;34;0
WireConnection;142;1;143;0
WireConnection;142;2;147;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;53;0;142;0
WireConnection;44;0;53;0
WireConnection;37;1;40;0
WireConnection;42;0;44;1
WireConnection;42;1;37;2
WireConnection;41;0;44;0
WireConnection;41;1;37;1
WireConnection;43;0;44;2
WireConnection;43;1;37;3
WireConnection;156;0;155;0
WireConnection;47;0;41;0
WireConnection;47;1;42;0
WireConnection;47;2;43;0
WireConnection;47;3;44;3
WireConnection;158;0;49;0
WireConnection;158;1;156;0
WireConnection;48;0;47;0
WireConnection;48;1;158;0
WireConnection;55;0;48;0
WireConnection;160;0;55;0
WireConnection;160;1;162;0
WireConnection;174;2;160;0
ASEEND*/
//CHKSM=65944A7FBA50648EF79CBE286FD088F28D7D8CBB